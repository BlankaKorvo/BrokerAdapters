using MarketDataModules;
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
        int firstEmaLookbackPeriod = 60;
        int SecondEmaLookbackPeriod = 200;
        const decimal DeltaCount = 8M;
        internal bool TwoEmaLongSignal(CandlesList candleList, decimal deltaPrice)
        {
            Log.Information("Start TwoEma LongSignal. Figi: " + candleList.Figi);
            List<EmaResult> firstEma = Mapper.EmaData(candleList, deltaPrice, firstEmaLookbackPeriod);
            List<EmaResult> secondEma = Mapper.EmaData(candleList, deltaPrice, SecondEmaLookbackPeriod);
            decimal? emaPriceDelta = ((deltaPrice * 100) / firstEma.Last().Ema) - 100; //Насколько далеко убежала цена от Ema
            if (
                firstEma.Last().Ema > secondEma.Last().Ema
                &&
                deltaPrice > firstEma.Last().Ema
                &&
                emaPriceDelta < DeltaCount
               )
            {
                //Log.Information("Checking for the absence of a gap via SMA");
                //Log.Information("Sma = " + ema.Last().Ema + "LPrice = " + deltaPrice);
                //Log.Information("smaPriceDelta = " + emaPriceDelta);
                //Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                //Log.Information("Should be: smaPriceDelta < smaPriceDeltaCount");
                //Log.Information("Sma = Long - true for: " + candleList.Figi);
                return true; 
            }
            else
            {
                //Log.Information("Checking for the absence of a gap via SMA");
                //Log.Information("Sma = " + ema.Last().Ema + "LPrice = " + deltaPrice);
                //Log.Information("smaPriceDelta = " + emaPriceDelta);
                //Log.Information("smaPriceDeltaCount = " + smaPriceDeltaCount);
                //Log.Information("Should be: smaPriceDelta < smaPriceDeltaCount");
                //Log.Information("Sma = Long - falce for: " + candleList.Figi);
                return false; 
            }
        }


    }
}
