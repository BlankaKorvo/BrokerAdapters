using MarketDataModules;
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

        internal TradeTarget OrderbookSignal(Orderbook orderbook)
        {
            Log.Information("Start OrderbookSignal. Figi: " + orderbook.Figi);
            decimal asks = orderbook.Asks.Select(x => x.Quantity).Sum();
            decimal bids = orderbook.Bids.Select(x => x.Quantity).Sum();
            Log.Information("asksQuantity = " + asks);
            Log.Information("bidsQuantity = " + bids);
            // decimal overAsk = 100 - (bids * 100 / asks);

            if (asks < bids)
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
