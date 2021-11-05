using System;
using Xunit;
using MarketDataModules.Models.Candles;
using Tinkoff.Trading.OpenApi.Models;
using DataCollector.TinkoffAdapter;

namespace DataCollector.Tests
{
    public class GetTinkoffDataTest
    {
        static int candleCount = 1000;
        CandleList GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval candleInterval)
        {
            return new GetTinkoffData().GetCandlesTinkoffAsync("BBG000BVPV84", candleInterval, candleCount).GetAwaiter().GetResult();
        }
            

        [Fact]
        public void MinuteCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.Minute));
        }

        [Fact]
        public void TwoMinutesCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.TwoMinutes));
        }
        [Fact]
        public void ThreeMinutesCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.ThreeMinutes));
        }
        [Fact]
        public void FiveMinutesCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.FiveMinutes));
        }
        [Fact]
        public void TenMinutesCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.TenMinutes));
        }
        [Fact]
        public void QuarterHourCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.QuarterHour));
        }
        [Fact]
        public void HourCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.Hour));
        }
        [Fact]
        public void HalfHourCandlesNotNull()
        {
            Assert.NotNull(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.HalfHour));
        }

        [Fact]
        public void TestCandlesCount()
        {
            Assert.Equal(GetMinuteCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval.Minute).Candles.Count, candleCount);
        }
    }
}
