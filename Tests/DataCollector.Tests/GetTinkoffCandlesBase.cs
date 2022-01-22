using MarketDataModules;
using MarketDataModules.Candles;
using System;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesBase
    {
        protected static ICandlesList GetCandles(CandleInterval candleInterval, DateTime dateTime)
        {
            var candles = MarketDataProvider.GetCandlesAsync("BBG000BVPV84", candleInterval, dateTime, Provider.Tinkoff).GetAwaiter().GetResult();
            return candles;
        }
        protected static ICandlesList GetCandles(CandleInterval candleInterval, int candleCount)
        {
            var candles = MarketDataProvider.GetCandlesAsync("BBG000BVPV84", candleInterval, candleCount, Provider.Tinkoff).GetAwaiter().GetResult();
            return candles;
        }
    }
}