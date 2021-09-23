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
        int firstEmaLookbackPeriod = 60;
        int SecondEmaLookbackPeriod = 200;
        const decimal firstEmaDeltaCount = 8M;
        const decimal secondEmaDeltaCount = 8M;
        internal bool TwoEmaLongSignal(CandlesList candleList, decimal deltaPrice)
        {
            Log.Information("Start TwoEma LongSignal. Figi: " + candleList.Figi);
            List<EmaResult> firstEma = Mapper.EmaData(candleList, deltaPrice, firstEmaLookbackPeriod);
            List<EmaResult> secondEma = Mapper.EmaData(candleList, deltaPrice, SecondEmaLookbackPeriod);
            decimal? firstEmaPriceDelta = ((deltaPrice * 100) / firstEma.Last().Ema) - 100; //Насколько далеко убежала цена от первой Ema
            decimal? secondEmaDeltaCount = ((deltaPrice * 100) / firstEma.Last().Ema) - 100; //Насколько далеко убежала цена от второй Ema
            bool persech=false;
            for (int i = 1; i <= 5; i++)
            {
                if (firstEma[firstEma.Count - i].Ema >= secondEma[secondEma.Count - i].Ema
                    &&
                    firstEma[firstEma.Count - i - 1].Ema <= secondEma[secondEma.Count - i - 1].Ema)
                {
                    persech = true;
                }
            }
            if (
                firstEma.Last().Ema > secondEma.Last().Ema
                &&
                deltaPrice > firstEma.Last().Ema
                &&
                firstEmaPriceDelta < firstEmaDeltaCount
                &&
                persech==true
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
