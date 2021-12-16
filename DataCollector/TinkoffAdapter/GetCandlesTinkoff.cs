using MarketDataModules.Models.Candles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.TinkoffAdapter
{
    internal class GetCandlesTinkoff : CandlesList
    {
        public GetCandlesTinkoff(string figi, CandleInterval interval, List<CandleStructure> candles) : base(figi, interval, candles)
        {
        }
    }
}
