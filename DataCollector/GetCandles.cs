using MarketDataModules.Models;
using MarketDataModules.Models.Candles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.TinkoffAdapter
{
    internal class GetCandles : CandlesList
    {
        public GetCandles(string figi, CandleInterval interval, List<CandleStructure> candles) : base(figi, interval, candles)
        {
        }

        static public async Task<CandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
        {
            switch (providers)
            {
                case Provider.Tinkoff:
                    //return await TinkoffCandles(figi, candleInterval, candlesCount);
                case Provider.Finam:
                    return null;
            }
            return null;
        }
    }
}
