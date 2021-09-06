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

        internal TradeOperation OrderbookSignal(Orderbook orderbook)
        {
            decimal asks = orderbook.Asks.Select(x => x.Quantity).Sum();
            decimal bids = orderbook.Bids.Select(x => x.Quantity).Sum();

            // decimal overAsk = 100 - (bids * 100 / asks);

            if (asks < bids)
                return TradeOperation.toLong;
            else
                return TradeOperation.toShort;
        }


    }
}
