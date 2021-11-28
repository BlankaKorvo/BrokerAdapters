using MarketDataModules.Models.Candles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketDataModules.Models.Portfolio
{
    public class MoneyAmount
    {
        public Currency Currency { get; }
        public decimal Value { get; }

        public MoneyAmount(Currency currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }

    }
}
