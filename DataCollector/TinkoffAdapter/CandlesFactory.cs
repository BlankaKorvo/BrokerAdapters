using MarketDataModules.Candles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.TinkoffAdapter
{
    internal abstract class CandlesFactory : ICandlesList
    {
        public string Figi => throw new NotImplementedException();

        public CandleInterval Interval => throw new NotImplementedException();

        public List<CandleStructure> Candles => throw new NotImplementedException();
    }
}
