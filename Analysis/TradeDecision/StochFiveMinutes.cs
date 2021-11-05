using Analysis.Signals;
using MarketDataModules;
using MarketDataModules.Models.Candles;
using MarketDataModules.Models.Orderbooks;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.TradeDecision
{
    public class StochFiveMinutes
    {
        Signal signal = new Signal();

        //Передаваемые при создании объекта параметры
        public CandlesList candleList { get; set; }
        public decimal deltaPrice { get; set; }
        public Orderbook orderbook { get; set; }

        //Тюнинг индикаторов


        public bool Long()
        {
            if (
                signal.StochGapSignalLongSignal(candleList, deltaPrice)
                )
            {
                Log.Information("StochFiveMinutes Algoritms: Long - true " + candleList.Figi);
                return true; 
            }
            else 
            {
                Log.Information("StochFiveMinutes Algoritms: Long - false " + candleList.Figi);
                return false;
            }
        }
        //public bool FromLong()
        //{
        //    if (
        //        signal.AdxFromLongSignal(candleList, deltaPrice)
        //        ||
        //        signal.AroonFromLongSignal(candleList, deltaPrice)
        //        )
        //    {
        //        Log.Information("StochFiveMinutes Algoritms: FromLong - true " + candleList.Figi);
        //        return true; 
        //    }
        //    else
        //    {
        //        Log.Information("StochFiveMinutes Algoritms: FromLong - false " + candleList.Figi);
        //        return false; 
        //    }
        //}

        public bool Short()
        {
            throw new NotImplementedException();
        }

        public bool FromShort()
        {
            throw new NotImplementedException();
        }
    }
}
