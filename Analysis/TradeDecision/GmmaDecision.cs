using MarketDataModules;
using MarketDataModules.Models;
using MarketDataModules.Models.Candles;
using MarketDataModules.Models.Operation;
using MarketDataModules.Models.Orderbooks;
using MarketDataModules.Models.Portfolio;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Tinkoff.Trading.OpenApi.Models;
using TradingAlgorithms.IndicatorSignals;

namespace Analysis.TradeDecision
{
    public class GmmaDecision
    {
        Signal signal = new Signal();


        //Передаваемые при создании объекта параметры
        public CandlesList candleListMin { get; set; }
        //public CandlesList candleListMax { get; set; }
        public decimal bestAsk { get; set; }
        public decimal bestBid { get; set; }
        public Orderbook orderbook { get; set; }
        public Portfolio.Position portfolioPosition { get; set; }
        public List<TradeOperation> tradeOperations { get; set; }

        //Тюнинг индикаторов

        public TradeTarget TradeVariant()
        {
            Log.Information("Start TradeVariant GmmaDecision. Figi: " + candleListMin.Figi);
            TradeTarget gmmaSignalMin = signal.GmmaSignal(candleListMin, bestAsk, bestBid);
            //TradeTarget gmmaSignalMax = signal.GmmaSignal(candleListMax, bestAsk, bestBid);
            TradeTarget orderbookSignal = signal.OrderbookSignal(orderbook);
            //TradeTarget safeMoneySignal = signal.SafeMoneySignal(orderbook, portfolioPosition, tradeOperations, candleList, 3);
            TradeTarget stochOutTradeSignal = signal.StochOutTradeSignal(candleListMin, orderbook);
            Log.Information("gmmaSignal = " + gmmaSignalMin);
            Log.Information("orderbookSignal = " + orderbookSignal);
            Log.Information("stochOutTradeSignal = " + stochOutTradeSignal);
            if
                (
                gmmaSignalMin == TradeTarget.toLong
                &&
                orderbookSignal == TradeTarget.toLong
                &&
                stochOutTradeSignal == TradeTarget.toLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.toLong. Figi: " + candleListMin.Figi + " Price: " + bestAsk);
                return TradeTarget.toLong;
            }
            else if
                (
                gmmaSignalMin == TradeTarget.toShort
                &&
                orderbookSignal == TradeTarget.toShort
                &&
                stochOutTradeSignal == TradeTarget.toShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.toShort. Figi: " + candleListMin.Figi + " Price: " + bestAsk);
                return TradeTarget.toShort;
            }
            else if
                (
                (gmmaSignalMin == TradeTarget.fromLong && stochOutTradeSignal != TradeTarget.fromShort) ///пиздецкий костыль. переделать
                ||
                (stochOutTradeSignal == TradeTarget.fromLong)
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.fromLong. Figi: " + candleListMin.Figi + " Price: " + bestAsk);
                return TradeTarget.fromLong;
            }
            else if
                (
                gmmaSignalMin == TradeTarget.fromShort
                ||
                stochOutTradeSignal == TradeTarget.fromShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.fromShort. Figi: " + candleListMin.Figi + " Price: " + bestAsk);
                return TradeTarget.fromShort;
            }
            else
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleListMin.Figi + " TradeTarget.notTrading");
                return TradeTarget.notTrading;
            }
        }
    }
}
