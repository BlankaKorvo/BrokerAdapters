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

        public List<string> OrderbookLong(List<Orderbook> orderbooks, int stocksReturn)
        {
            Log.Information("Start OrderbookLong");
            List<OrderbookQuantity> orderbookQuantitys = GetOrderbookQuantity(orderbooks);
            var resultOrderbook = orderbookQuantitys.OrderByDescending(x => x.Bids / x.Asks).Take(stocksReturn);
            List<string> result = resultOrderbook.Select(x => x.Figi).ToList();
            Log.Information("Stop OrderbookLong");
            return result;
        }

        public List<string> OrderbookShort(List<Orderbook> orderbooks, int stocksReturn)
        {
            Log.Information("Start OrderbookShort");
            List<OrderbookQuantity> orderbookQuantitys = GetOrderbookQuantity(orderbooks);
            var resultOrderbook = orderbookQuantitys.OrderByDescending(x => x.Asks / x.Bids ).Take(stocksReturn);
            List<string> result = resultOrderbook.Select(x => x.Figi).ToList();
            Log.Information("Stop OrderbookShort");
            return result;
        }

        private static List<OrderbookQuantity> GetOrderbookQuantity(List<Orderbook> orderbooks)
        {
            List<OrderbookQuantity> orderbookQuantitys = new List<OrderbookQuantity> { };
            foreach (Orderbook item in orderbooks)
            {
                Log.Information("Start OrderbookLong Analisys: " + item.Figi);
                int asks = item.Asks.Select(x => x.Quantity).Sum();
                int bids = item.Bids.Select(x => x.Quantity).Sum();

                Log.Information("asksQuantity = " + asks);
                Log.Information("bidsQuantity = " + bids);
                orderbookQuantitys.Add(new OrderbookQuantity() { Figi = item.Figi, Asks = asks, Bids = bids });
                Log.Information("Stop OrderbookLong Analisys: " + item.Figi);
            }
            return orderbookQuantitys;
        }
    }

    class OrderbookQuantity
    {
        internal string Figi { get; set; }
        internal int Asks { get; set; }
        internal int Bids { get; set; }
    }
}

