using Xunit;
using MarketDataModules.Candles;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesPositiveTest : GetTinkoffCandlesBase
    {

        [Fact]
        public async Task MinuteCandlesCount()
        {
            //arrange
            int candleCount = 1441;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Minute, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task MinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Minute, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task TwoMinutesCandlesCount()
        {
            //arrange
            int candleCount = 721;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.TwoMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task TwoMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.TwoMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task ThreeMinutesCandlesCount()
        {
            //arrange
            int candleCount = 481;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.ThreeMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task ThreeMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.ThreeMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task FiveMinutesCandlesCount()
        {
            //arrange
            int candleCount = 289;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.FiveMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task FiveMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.FiveMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task TenMinutesCandlesCount()
        {
            //arrange
            int candleCount = 145;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.TenMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task TenMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.TenMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task QuarterHourCandlesCount()
        {
            //arrange
            int candleCount = 97;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.QuarterHour, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task QuarterHourCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.QuarterHour, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task HourCandlesCount()
        {
            //arrange
            int candleCount = 169;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Hour, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task HourCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-15);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Hour, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task DayCandlesCount()
        {
            //arrange
            int candleCount = 366;
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Day, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task DayCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-374);
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Day, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task WeekCandlesCount()
        {
            //arrange
            int candleCount = 106;
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Week, candleCount);
            //assert
            for (int i = 1; i < testData.Candles.Count; i++)
            {
                timeSpanList.Add(testData.Candles[i].Time - testData.Candles[i - 1].Time);
            }

            Assert.Equal(TimeSpan.FromDays(7), timeSpanList.Max());
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task WeekCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-735);
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Week, dateTime);
            //assert
            for (int i = 1; i < testData.Candles.Count; i++)
            {
                timeSpanList.Add(testData.Candles[i].Time - testData.Candles[i - 1].Time);
            }

            Assert.Equal(TimeSpan.FromDays(7), timeSpanList.Max());
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public async Task MonthCandlesCount()
        {
            //arrange
            int candleCount = 121;
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Month, candleCount);
            //assert
            for (int i = 1; i < testData.Candles.Count; i++)
            {
                timeSpanList.Add(testData.Candles[i].Time - testData.Candles[i - 1].Time);
            }

            Assert.True(
                TimeSpan.FromDays(31) == timeSpanList.Max() ||
                TimeSpan.FromDays(30) == timeSpanList.Max() ||
                TimeSpan.FromDays(29) == timeSpanList.Max() ||
                TimeSpan.FromDays(28) == timeSpanList.Max()
                );
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public async Task MonthCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-3653);
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = await GetCandles(CandleInterval.Month, dateTime);
            //assert
            for (int i = 1; i < testData.Candles.Count; i++)
            {
                timeSpanList.Add(testData.Candles[i].Time - testData.Candles[i - 1].Time);
            }

            Assert.True(
                TimeSpan.FromDays(31) == timeSpanList.Max() ||
                TimeSpan.FromDays(30) == timeSpanList.Max() ||
                TimeSpan.FromDays(29) == timeSpanList.Max() ||
                TimeSpan.FromDays(28) == timeSpanList.Max()
                );
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }
    }
}
