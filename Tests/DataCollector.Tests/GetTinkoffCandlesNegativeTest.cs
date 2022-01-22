using Xunit;

using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Linq;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesNegativeTest : GetTinkoffCandlesBase
    {
        //[Fact]        
        //public void MinuteCandlesCount()
        //{
        //    //arrange
        //    int candleCount = 5000;
        //    //act
        //    var testData = GetCandles(CandleInterval.Minute, candleCount).Candles.Count;
        //    //assert
        //    Assert.Equal(testData, candleCount);
        //}

        //[Fact]
        //public void MinuteCandlesDate()
        //{
        //    //arrange
        //    DateTime dateTime = DateTime.Now.AddDays(-50);
        //    //act
        //    DateTime testData = GetCandles(CandleInterval.Minute, dateTime).Candles.FirstOrDefault().Time;
        //    //assert
        //    Assert.True(DateTime.Compare(testData, dateTime) < 0);
        //}

        //[Fact]
        //public void TwoMinutesCandlesCount()
        //{   
        //    //arrange
        //    int candleCount = 5000;
        //    //act
        //    var testData = GetCandles(CandleInterval.TwoMinutes, candleCount).Candles.Count;
        //    //assert
        //    Assert.Equal(testData, candleCount);
        //}

        //[Fact]
        //public void TwoMinuteCandlesDate()
        //{
        //    //arrange
        //    DateTime dateTime = DateTime.Now.AddDays(-50);
        //    //act
        //    DateTime testData = GetCandles(CandleInterval.TwoMinutes, dateTime).Candles.FirstOrDefault().Time;
        //    //assert
        //    Assert.True(DateTime.Compare(testData, dateTime) < 0);
        //}

        //[Fact]
        //public void ThreeMinutesCandlesCount()
        //{
        //    //arrange
        //    int candleCount = 5000;
        //    //act
        //    var testData = GetCandles(CandleInterval.ThreeMinutes, candleCount).Candles.Count;
        //    //assert
        //    Assert.Equal(testData, candleCount);
        //}

        //[Fact]
        //public void ThreeMinuteCandlesDate()
        //{
        //    //arrange
        //    DateTime dateTime = DateTime.Now.AddDays(-50);
        //    //act
        //    DateTime testData = GetCandles(CandleInterval.ThreeMinutes, dateTime).Candles.FirstOrDefault().Time;
        //    //assert
        //    Assert.True(DateTime.Compare(testData, dateTime) < 0);
        //}

        //[Fact]
        //public void FiveMinutesCandlesCount()
        //{
        //    //arrange
        //    int candleCount = 5000;
        //    //act
        //    var testData = GetCandles(CandleInterval.FiveMinutes, candleCount).Candles.Count;
        //    //assert
        //    Assert.Equal(testData, candleCount);
        //}

        //[Fact]
        //public void FiveMinuteCandlesDate()
        //{
        //    //arrange
        //    DateTime dateTime = DateTime.Now.AddDays(-50);
        //    //act
        //    DateTime testData = GetCandles(CandleInterval.FiveMinutes, dateTime).Candles.FirstOrDefault().Time;
        //    //assert
        //    Assert.True(DateTime.Compare(testData, dateTime) < 0);
        //}

        //[Fact]
        //public void TenMinutesCandlesCount()
        //{
        //    //arrange
        //    int candleCount = 5000;
        //    //act
        //    var testData = GetCandles(CandleInterval.TenMinutes, candleCount).Candles.Count;
        //    //assert
        //    Assert.Equal(testData, candleCount);
        //}

        //[Fact]
        //public void TenMinuteCandlesDate()
        //{
        //    //arrange
        //    DateTime dateTime = DateTime.Now.AddDays(-50);
        //    //act
        //    DateTime testData = GetCandles(CandleInterval.TenMinutes, dateTime).Candles.FirstOrDefault().Time;
        //    //assert
        //    Assert.True(DateTime.Compare(testData, dateTime) < 0);
        //}

        //[Fact]
        //public void QuarterHourCandlesCount()
        //{
        //    //arrange
        //    int candleCount = 5000;
        //    //act
        //    var testData = GetCandles(CandleInterval.QuarterHour, candleCount).Candles.Count;
        //    //assert
        //    Assert.Equal(testData, candleCount);
        //}

        //[Fact]
        //public void QuarterHourCandlesDate()
        //{
        //    //arrange
        //    DateTime dateTime = DateTime.Now.AddDays(-50);
        //    //act
        //    DateTime testData = GetCandles(CandleInterval.QuarterHour, dateTime).Candles.FirstOrDefault().Time;
        //    //assert
        //    Assert.True(DateTime.Compare(testData, dateTime) < 0);
        //}

        [Fact]
        public void HourCandlesCount()
        {
            //arrange
            int candleCount = 500000;
            //act
            //act
            void act() => GetCandles(CandleInterval.Hour, candleCount);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }

        [Fact]
        public void HourCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-5000);
            //act
            void act() => GetCandles(CandleInterval.Hour, dateTime);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }

        [Fact]
        public void DayCandlesCount()
        {
            //arrange
            int candleCount = 500000;
            //act
            void act() => GetCandles(CandleInterval.Day, candleCount);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }

        [Fact]
        public void DayCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-50000);
            //act
            void act() => GetCandles(CandleInterval.Day, dateTime);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);

        }

        [Fact]
        public void WeekCandlesCount()
        {
            //arrange
            int candleCount = 50000;
            //act
            void act() => GetCandles(CandleInterval.Week, candleCount);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }

        [Fact]
        public void WeekCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-100000);
            //act
            void act() => GetCandles(CandleInterval.Week, dateTime);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }

        [Fact]
        public void MonthCandlesCount()
        {
            //arrange
            int candleCount = 500000;
            //act
            void act() => GetCandles(CandleInterval.Month, candleCount);
            //assert
            Exception exception = Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }

        [Fact]
        public void MonthCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-181000);
            //act
            void act() => GetCandles(CandleInterval.Month, dateTime);
            //assert
            Exception exception =  Assert.ThrowsAny<Exception>(act);
            Assert.Equal("No more candles. Reduce the number of candles in the request", exception.Message);
        }
    }
}
