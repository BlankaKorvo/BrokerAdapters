using MarketDataModules;
using MarketDataModules.Models;
using MarketDataModules.Models.Orderbooks;
using MarketDataModules.Models.Portfolio;
using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinkoffData;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace TradingAlgorithms.IndicatorSignals
{
    public partial class Signal : IndicatorSignalsHelper
    {

        internal TradeTarget PortfolioSignal(Orderbook orderbook, Portfolio portfolio)
        {
            Log.Information("Start PortfolioSignal. Figi: " + orderbook.Figi);
            decimal ask = orderbook.Asks.First().Price;
            decimal bid = orderbook.Bids.First().Price;
            Log.Information("asksQuantity = " + ask);
            Log.Information("bidsQuantity = " + bid);
            // decimal overAsk = 100 - (bids * 100 / asks);

            if (ask < bid)
            {
                Log.Information("Stop OrderbookSignal. Figi: " + orderbook.Figi + " asks < bids " + " TradeOperation.toLong");
                return TradeTarget.toLong;
            }
            else
            {
                Log.Information("Stop OrderbookSignal. Figi: " + orderbook.Figi + " asks > bids " + " TradeOperation.toShort");
                return TradeTarget.toShort;
            }
        }


    }
}
