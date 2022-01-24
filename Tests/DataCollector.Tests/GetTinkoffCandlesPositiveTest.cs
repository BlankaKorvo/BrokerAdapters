using Xunit;
using MarketDataModules.Candles;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesPositiveTest : GetTinkoffCandlesBase
    {

        [Fact]
        public void MinuteCandlesCount()
        {
            //arrange
            int candleCount = 1441;
            //act
            ICandlesList testData = GetCandles(CandleInterval.Minute, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void MinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = GetCandles(CandleInterval.Minute, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void TwoMinutesCandlesCount()
        {
            //arrange
            int candleCount = 721;
            //act
            ICandlesList testData = GetCandles(CandleInterval.TwoMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void TwoMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = GetCandles(CandleInterval.TwoMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void ThreeMinutesCandlesCount()
        {
            //arrange
            int candleCount = 481;
            //act
            ICandlesList testData = GetCandles(CandleInterval.ThreeMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void ThreeMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = GetCandles(CandleInterval.ThreeMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void FiveMinutesCandlesCount()
        {
            //arrange
            int candleCount = 289;
            //act
            ICandlesList testData = GetCandles(CandleInterval.FiveMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void FiveMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = GetCandles(CandleInterval.FiveMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void TenMinutesCandlesCount()
        {
            //arrange
            int candleCount = 145;
            //act
            ICandlesList testData = GetCandles(CandleInterval.TenMinutes, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void TenMinuteCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = GetCandles(CandleInterval.TenMinutes, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void QuarterHourCandlesCount()
        {
            //arrange
            int candleCount = 97;
            //act
            ICandlesList testData = GetCandles(CandleInterval.QuarterHour, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void QuarterHourCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-8);
            //act
            ICandlesList testData = GetCandles(CandleInterval.QuarterHour, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void HourCandlesCount()
        {
            //arrange
            int candleCount = 169;
            //act
            ICandlesList testData = GetCandles(CandleInterval.Hour, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void HourCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-15);
            //act
            ICandlesList testData = GetCandles(CandleInterval.Hour, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void DayCandlesCount()
        {
            //arrange
            int candleCount = 366;
            //act
            ICandlesList testData = GetCandles(CandleInterval.Day, candleCount);
            //assert
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void DayCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-374);
            //act
            ICandlesList testData = GetCandles(CandleInterval.Day, dateTime);
            //assert
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void WeekCandlesCount()
        {
            //arrange
            int candleCount = 106;
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = GetCandles(CandleInterval.Week, candleCount);
            //assert
            for (int i = 1; i < testData.Candles.Count; i++)
            {
                timeSpanList.Add(testData.Candles[i].Time - testData.Candles[i - 1].Time);
            }

            Assert.Equal(TimeSpan.FromDays(7), timeSpanList.Max());
            Assert.Equal(candleCount, testData.Candles.Count);
        }

        [Fact]
        public void WeekCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-735);
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = GetCandles(CandleInterval.Week, dateTime);
            //assert
            for (int i = 1; i < testData.Candles.Count; i++)
            {
                timeSpanList.Add(testData.Candles[i].Time - testData.Candles[i - 1].Time);
            }

            Assert.Equal(TimeSpan.FromDays(7), timeSpanList.Max());
            Assert.True(DateTime.Compare(testData.Candles.FirstOrDefault().Time, dateTime) < 0);
        }

        [Fact]
        public void MonthCandlesCount()
        {
            //arrange
            int candleCount = 121;
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = GetCandles(CandleInterval.Month, candleCount);
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
        public void MonthCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-3653);
            List<TimeSpan> timeSpanList = new();
            //act
            ICandlesList testData = GetCandles(CandleInterval.Month, dateTime);
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
