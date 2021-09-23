using MarketDataModules;
using MarketDataModules.Models.Candles;
using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinkoffData;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace TradingAlgorithms.IndicatorSignals
{
    public partial class Signal : IndicatorSignalsHelper
    {
        int emaLookbackPeriod = 8;
        const decimal emaPriceDeltaCount = 8M;
        internal bool EmaLongSignal(CandlesList candleList, decimal deltaPrice)
        {
            Log.Information("Start Sma LongSignal. Figi: " + candleList.Figi);
            List<EmaResult> ema = Mapper.EmaData(candleList, deltaPrice, smaLookbackPeriod);
            decimal? emaPriceDelta = ((deltaPrice * 100) / ema.Last().Ema) - 100; //Насколько далеко убежала цена от Sma
            if (
                emaPriceDelta < emaPriceDeltaCount
               )
            {
                Log.Information("Checking for the absence of a gap via SMA");
                Log.Information("Sma = " + ema.Last().Ema + "LPrice = " + deltaPrice);
                Log.Information("smaPriceDelta = " + emaPriceDelta);
                Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                Log.Information("Should be: smaPriceDelta < smaPriceDeltaCount");
                Log.Information("Sma = Long - true for: " + candleList.Figi);
                return true; 
            }
            else
            {
                Log.Information("Checking for the absence of a gap via SMA");
                Log.Information("Sma = " + ema.Last().Ema + "LPrice = " + deltaPrice);
                Log.Information("smaPriceDelta = " + emaPriceDelta);
                Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                Log.Information("Should be: smaPriceDelta < smaPriceDeltaCount");
                Log.Information("Sma = Long - falce for: " + candleList.Figi);
                return false; 
            }
        }


    }
}
