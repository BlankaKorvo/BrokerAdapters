using MarketDataModules.Models.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketDataModules.Models
{
    public class TransactionModel : TransactionModelBase
    {
        public int Quantity { get; set; } //кол-во акций
        //public decimal Purchase { get; set; } //объем покупки в валюте
    }

    public class TransactionModelBase
    {
        public string Figi { get; set; }
        public decimal Price { get; set; }
        public TradingOperationType TradeOperation { get; set; }

    }
    public enum TradeTarget
    {
        notTrading,
        toLong,
        fromLong,
        toShort,
        fromShort        
    }
}

