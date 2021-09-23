using MarketDataModules;
using MarketDataModules.Models;
using MarketDataModules.Models.Candles;
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
    public class GmmaDecisionOneMinutesShortLines
    {
        Signal signal = new Signal();


        //Передаваемые при создании объекта параметры
        public CandlesList candleList { get; set; }
        public decimal bestAsk { get; set; }
        public decimal bestBid { get; set; }
        public Orderbook orderbook { get; set; }
        public Portfolio portfolio { get; set; }


        //Тюнинг индикаторов


        public TradeTarget TradeVariant()
        {
            Log.Information("Start TradeVariant GmmaDecision. Figi: " + candleList.Figi);
            TradeTarget gmmaSignalOneMinutesShortLines = signal.GmmaSignalOneMinutesShortLines(candleList, bestAsk, bestBid);
            TradeTarget orderbookSignal = signal.OrderbookSignal(orderbook);

            if
                (
                gmmaSignalOneMinutesShortLines == TradeTarget.toLong
                &&
                orderbookSignal == TradeTarget.toLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.toLong. Figi: " + candleList.Figi + " Price: " + bestAsk);
                return TradeTarget.toLong;
            }
            else if
                (
                gmmaSignalOneMinutesShortLines == TradeTarget.toShort
                &&
                orderbookSignal == TradeTarget.toShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.toShort. Figi: " + candleList.Figi + " Price: " + bestAsk);
                return TradeTarget.toShort;
            }
            else if
                (
                gmmaSignalOneMinutesShortLines == TradeTarget.fromLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.fromLong. Figi: " + candleList.Figi + " Price: " + bestAsk);
                return TradeTarget.fromLong;
            }

            else if
                (
                gmmaSignalOneMinutesShortLines == TradeTarget.fromShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. TradeTarget.fromShort. Figi: " + candleList.Figi + " Price: " + bestAsk);
                return TradeTarget.fromShort;
            }
            else
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeTarget.notTrading");
                return TradeTarget.notTrading;
            }

        }

    //    public bool Long()
    //    {
    //        if (
    //            signal.StochGapSignalLongSignal(candleList, price)
    //            )
    //        {
    //            Log.Information("StochFiveMinutes Algoritms: Long - true " + candleList.Figi);
    //            return true; 
    //        }
    //        else 
    //        {
    //            Log.Information("StochFiveMinutes Algoritms: Long - false " + candleList.Figi);
    //            return false;
    //        }
    //    }
    //    //public bool FromLong()
    //    //{
    //    //    if (
    //    //        signal.AdxFromLongSignal(candleList, deltaPrice)
    //    //        ||
    //    //        signal.AroonFromLongSignal(candleList, deltaPrice)
    //    //        )
    //    //    {
    //    //        Log.Information("StochFiveMinutes Algoritms: FromLong - true " + candleList.Figi);
    //    //        return true; 
    //    //    }
    //    //    else
    //    //    {
    //    //        Log.Information("StochFiveMinutes Algoritms: FromLong - false " + candleList.Figi);
    //    //        return false; 
    //    //    }
    //    //}

    //    public bool Short()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool FromShort()
    //    {
    //        throw new NotImplementedException();
    //    }
    }
}
