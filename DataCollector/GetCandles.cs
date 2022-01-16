using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.TinkoffAdapter
{
    internal static class GetCandles /*: CandlesList*/
    {
        static public async Task<ICandlesList> CandlesAsync(string figi, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
        {
            switch (providers)
            {
                case Provider.Tinkoff:
                    return await MarketDataCollector.GetCandlesAsync(figi,candleInterval, candlesCount);
                case Provider.Finam:
                    return null;
            }
            return null;
        }
    }
}
