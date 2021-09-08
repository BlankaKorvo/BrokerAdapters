using Analysis.Screeners.Helpers;
using DataCollector;
using MarketDataModules;
using MarketDataModules.Models.Candles;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingAlgorithms.IndicatorSignals;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace Analysis.Screeners.CandlesScreener
{
    public class OrderbookScreener : GetStocksHistory
    {
        //MarketDataCollector dataCollector = new MarketDataCollector();

        public List<CandlesList> OrderbookLong(List<Orderbook> orderbooks)
        {
            Log.Information("Start OrderbookLong");
            List<CandlesList> result = new List<CandlesList> { };
            foreach (Orderbook item in orderbooks)
            {
                Log.Information("Start OrderbookLong Analisys: " + item.Figi);
                decimal asks = item.Asks.Select(x => x.Quantity).Sum();
                decimal bids = item.Bids.Select(x => x.Quantity).Sum();

                Log.Information("asksQuantity = " + asks);
                Log.Information("bidsQuantity = " + bids);

               
            }
            Log.Information("Stop TrandUp");
            return result;
        }
    }
}

