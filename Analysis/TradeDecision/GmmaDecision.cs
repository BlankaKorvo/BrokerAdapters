using MarketDataModules;
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
        public CandlesList candleList { get; set; }
        public decimal bestAsk { get; set; }
        public decimal bestBid { get; set; }
        public Orderbook orderbook { get; set; }
        

        //Тюнинг индикаторов


        public TradeOperation TradeVariant()
        {
            Log.Information("Start TradeVariant GmmaDecision. Figi: " + candleList.Figi);
            TradeOperation gmmaSignal = signal.GmmaSignal(candleList, bestAsk, bestBid);
            TradeOperation orderbookSignal = signal.OrderbookSignal(orderbook);

            if
                (
                gmmaSignal == TradeOperation.toLong
                &&
                orderbookSignal == TradeOperation.toLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeOperation.toLong");
                return TradeOperation.toLong;
            }
            else if
                (
                gmmaSignal == TradeOperation.toShort
                &&
                orderbookSignal == TradeOperation.toShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeOperation.toShort");
                return TradeOperation.toShort;
            }
            else if
                (
                gmmaSignal == TradeOperation.fromLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeOperation.fromLong");
                return TradeOperation.fromLong;
            }

            else if
                (
                gmmaSignal == TradeOperation.fromShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeOperation.fromShort");
                return TradeOperation.fromShort;
            }
            else
            {
                return TradeOperation.notTrading;
            }
//                throw new Exception("Error in TradeVariant GmmaDecision");
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
