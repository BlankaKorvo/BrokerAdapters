using Xunit;
using MarketDataModules.Models.Candles;

using MarketDataModules.Models;

namespace DataCollector.Tests
{
    public class GetTinkoffDataTest
    {
        CandlesList GetCandles(CandleInterval candleInterval, int candleCount)
        {
            return MarketDataCollector.GetCandlesAsync("BBG000BVPV84", candleInterval, candleCount, Provider.Tinkoff).GetAwaiter().GetResult();
        }

        [Fact]
        public void MinuteCandlesCount()
        {
            //arrange
            int candleCount = 500;
            //act
            var testData = GetCandles(CandleInterval.Minute, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }

        [Fact]
        public void TwoMinutesCandlesCount()
        {   
            //arrange
            int candleCount = 500;
            //act
            var testData = GetCandles(CandleInterval.TwoMinutes, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void ThreeMinutesCandlesCount()
        {
            //arrange
            int candleCount = 300;
            //act
            var testData = GetCandles(CandleInterval.ThreeMinutes, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void FiveMinutesCandlesCount()
        {
            //arrange
            int candleCount = 200;
            //act
            var testData = GetCandles(CandleInterval.FiveMinutes, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void TenMinutesCandlesCount()
        {
            //arrange
            int candleCount = 100;
            //act
            var testData = GetCandles(CandleInterval.TenMinutes, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void QuarterHourCandlesCount()
        {
            //arrange
            int candleCount = 70;
            //act
            var testData = GetCandles(CandleInterval.QuarterHour, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void HourCandlesCount()
        {
            //arrange
            int candleCount = 10;
            //act
            var testData = GetCandles(CandleInterval.Hour, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void DayCandlesCount()
        {
            //arrange
            int candleCount = 400;
            //act
            var testData = GetCandles(CandleInterval.Day, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void WeekCandlesCount()
        {
            //arrange
            int candleCount = 200;
            //act
            var testData = GetCandles(CandleInterval.Week, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }
        [Fact]
        public void MonthCandlesCount()
        {
            //arrange
            int candleCount = 130;
            //act
            var testData = GetCandles(CandleInterval.Month, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }

    }
}
