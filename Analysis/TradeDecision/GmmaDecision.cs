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


        public TradeTarget TradeVariant()
        {
            Log.Information("Start TradeVariant GmmaDecision. Figi: " + candleList.Figi);
            TradeTarget gmmaSignal = signal.GmmaSignal(candleList, bestAsk, bestBid);
            TradeTarget orderbookSignal = signal.OrderbookSignal(orderbook);

            if
                (
                gmmaSignal == TradeTarget.toLong
                &&
                orderbookSignal == TradeTarget.toLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeTarget.toLong");
                return TradeTarget.toLong;
            }
            else if
                (
                gmmaSignal == TradeTarget.toShort
                &&
                orderbookSignal == TradeTarget.toShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeTarget.toShort");
                return TradeTarget.toShort;
            }
            else if
                (
                gmmaSignal == TradeTarget.fromLong
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeTarget.fromLong");
                return TradeTarget.fromLong;
            }

            else if
                (
                gmmaSignal == TradeTarget.fromShort
                )
            {
                Log.Information("Stop TradeVariant GmmaDecision. Figi: " + candleList.Figi + " TradeTarget.fromShort");
                return TradeTarget.fromShort;
            }
            else
            {
                throw new Exception("Error in TradeVariant GmmaDecision");
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
