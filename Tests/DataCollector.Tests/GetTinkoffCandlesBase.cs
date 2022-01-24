using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Threading.Tasks;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesBase
    {
        protected static async Task<ICandlesList> GetCandles(CandleInterval candleInterval, DateTime dateTime)
        {
            var candles = await GetMarketData.GetCandlesAsync("BBG000BVPV84", candleInterval, dateTime, Provider.Tinkoff);
            return candles;
        }
        protected static async Task<ICandlesList> GetCandles(CandleInterval candleInterval, int candleCount)
        {
            var candles = await GetMarketData.GetCandlesAsync("BBG000BVPV84", candleInterval, candleCount, Provider.Tinkoff);
            return candles;
        }
    }
}