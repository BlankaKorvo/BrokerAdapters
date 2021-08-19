using DataCollector;
using MarketDataModules;
using MarketDataModules.Models.Candles;
using ScreenerStocks.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingAlgorithms.IndicatorSignals;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace Analysis.Screeners
{
    public class TwoEmaScreener : GetStocksHistory
    {
        //MarketDataCollector dataCollector = new MarketDataCollector();
        IndicatorSignalsHelper indicatorSignalsHelper = new IndicatorSignalsHelper();
        Signal signal = new Signal();
        public List<CandlesList> TrandUp(List<CandlesList> candlesLists)
        {
            Log.Information("Start TrandUp");
            List<CandlesList> result = new List<CandlesList> { };
            foreach (CandlesList item in candlesLists)
            {
                Log.Information("Start TrandUp Analisys: " + item.Figi);


                if (
                    signal.TwoEmaLongSignal(item, item.Candles.Last().Close)
                   )
                {
                    Log.Information("Stop TrandUp Analisys: " + item.Figi + " Add to List");
                    result.Add(item);
                }
                else
                {
                    Log.Information("Stop TrandUp Analisys: " + item.Figi + " Not add to List");
                }
               
            }
            Log.Information("Stop TrandUp");
            return result;
        }
    }
}

