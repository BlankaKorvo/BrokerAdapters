using MarketDataModules;
using MarketDataModules.Models;
using MarketDataModules.Models.Candles;
using MarketDataModules.Models.Operation;
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
using Analysis.Signals.Helpers;

namespace Analysis.Signals
{
    public partial class Signal : IndicatorSignalsHelper
    {

        internal TradeTarget SafeMoneySignal(Orderbook orderbook, Portfolio.Position portfolioPosition, List<TradeOperation> tradeOperations, CandlesList candlesList, int candelsCountParam)
        {
            Log.Information("Start SafeMoneySignal MethodFigi: " + orderbook.Figi);

            if (
                portfolioPosition == null
                ||
                portfolioPosition?.AveragePositionPrice?.Value == null
                ||
                candlesList.Candles == null
                ||
                tradeOperations?.Last().Date == null
                )
            {
                Log.Information("portfolioPosition = " + portfolioPosition);
                Log.Information("portfolioPosition?.AveragePositionPrice?.Value = " + portfolioPosition?.AveragePositionPrice?.Value);
                Log.Information("candlesList.Candles = " + candlesList?.Candles);
                Log.Information("tradeOperations?.Last().Date = " + tradeOperations.Count);                
                Log.Information("Stop SafeMoneySignal MethodFigi: " + orderbook?.Figi);
                Log.Information("portfolioPosition, portfolioPosition?.AveragePositionPrice?.Value, candlesList.Candles, tradeOperations?.Last().Date == null");
                return TradeTarget.notTrading;
            }

            TradeTarget tradeTarget = TradeTarget.notTrading;
            decimal ask = orderbook.Asks.First().Price;
            decimal bid = orderbook.Bids.First().Price;


            Log.Information("asksQuantity = " + ask);
            Log.Information("bidsQuantity = " + bid);

            int countCandlesAfterTrade = CountCandlesAfterTrade(tradeOperations, candlesList);

            Log.Information("countCandlesAfterTrade = " + countCandlesAfterTrade);


            if ( countCandlesAfterTrade > 0 && countCandlesAfterTrade <= candelsCountParam)
            {
                //decimal procent = 0.08m;
                //decimal delta = portfolioPosition.AveragePositionPrice.Value * procent / 100;
                //decimal fromLongPrice = portfolioPosition.AveragePositionPrice.Value - delta;
                //decimal fromShortPrice = portfolioPosition.AveragePositionPrice.Value + delta;

                //Log.Information("procent = " + procent);                
                //Log.Information("delta = " + delta);                
                //Log.Information("fromLongPrice = " + fromLongPrice);                
                //Log.Information("fromShortPrice = " + fromShortPrice);

                //if
                //    (
                //    bid < fromLongPrice
                //    )
                //{
                //    Log.Information("bid < fromLongPrice. tradeTarget = TradeTarget.fromLong;");
                //    tradeTarget = TradeTarget.fromLong;
                //}
                //else if
                //    (
                //    ask > fromShortPrice
                //    )
                //{
                //    Log.Information("ask > fromShortPrice. tradeTarget = TradeTarget.fromShort;");
                //    tradeTarget = TradeTarget.fromShort;
                //}
                //Log.Information("Start SafeMoneySignal MethodFigi: " + orderbook.Figi + " TradeTarget= " + tradeTarget);
                //return tradeTarget;
                return TradeTarget.notTrading;
            }
            else
            {
                decimal procent = 0.08m;
                decimal delta = 2 * portfolioPosition.AveragePositionPrice.Value * procent / 100 / (1 - procent / 100);
                decimal fromLongPrice = portfolioPosition.AveragePositionPrice.Value + delta;
                decimal fromShortPrice = portfolioPosition.AveragePositionPrice.Value - delta;

                Log.Information("procent = " + procent);
                Log.Information("delta = " + delta);
                Log.Information("fromLongPrice = " + fromLongPrice);
                Log.Information("fromShortPrice = " + fromShortPrice);

                if
                    (
                    bid < fromLongPrice
                    )
                {
                    Log.Information("bid < fromLongPrice. tradeTarget = TradeTarget.fromLong;");
                    tradeTarget = TradeTarget.fromLong;
                }
                else if
                    (
                    ask > fromShortPrice
                    )
                {
                    Log.Information("ask > fromShortPrice. tradeTarget = TradeTarget.fromShort;");
                    tradeTarget = TradeTarget.fromShort;
                }
                Log.Information("Start SafeMoneySignal MethodFigi: " + orderbook.Figi + " TradeTarget= " + tradeTarget);
                return tradeTarget;
            }    
        }

        private int CountCandlesAfterTrade(List<TradeOperation> tradeOperations, CandlesList candlesList)
        {
            Log.Information("Start CountCandlesAfterTrade method");
            Log.Information("tradeOperations.Last().Date = " + tradeOperations.Last().Date);

            for (int i = 1; i <= candlesList.Candles.Count; i++)
            {
                Log.Information("candlesList.Candles[^i].Time = " + candlesList.Candles[^i].Time);
                if (candlesList.Candles[^i].Time <= tradeOperations.Last().Date)
                {
                    Log.Information("Stop CountCandlesAfterTrade method. Return " + i);
                    return i;
                }
            }
            Log.Information("Stop CountCandlesAfterTrade method. Return 0");
            return 0;
        }
    }
}
