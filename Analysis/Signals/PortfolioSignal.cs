using MarketDataModules;
using MarketDataModules.Models;
using MarketDataModules.Models.Candles;
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

        internal TradeTarget PortfolioSignal(Orderbook orderbook, Portfolio.Position portfolioPosition/*, LastTransactionModel lastTransactionModel, CandleInterval candleInterval*/)
        {
            Log.Information("Start PortfolioSignal. Figi: " + orderbook.Figi);
            decimal ask = orderbook.Asks.First().Price;
            decimal bid = orderbook.Bids.First().Price;
            Log.Information("asksQuantity = " + ask);
            Log.Information("bidsQuantity = " + bid);
            decimal procent = 0.05m;
            decimal delta = portfolioPosition.AveragePositionPrice.Value * procent / 100;
            decimal fromLongPrice = portfolioPosition.AveragePositionPrice.Value - delta;
            decimal fromShortPrice = portfolioPosition.AveragePositionPrice.Value + delta;
            if
                (
                bid < fromLongPrice
                )
            { 
                return TradeTarget.fromLong; 
            }
            else if
                (
                ask > fromShortPrice
                )
            {
                return TradeTarget.fromShort;
            }
            else
            {
                return TradeTarget.notTrading;
            }

        }


    }
}
