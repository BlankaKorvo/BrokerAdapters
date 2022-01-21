using Xunit;

using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Linq;

namespace DataCollector.Tests
{
    public class GetTinkoffDataNegativeTest
    {
        
        ICandlesList GetCandles(CandleInterval candleInterval, int candleCount)
        {
            var candles = MarketDataProvider.GetCandlesAsync("BBG000BVPV84", candleInterval, candleCount, Provider.Tinkoff).GetAwaiter().GetResult();
            return candles;
        }

        ICandlesList GetCandles(CandleInterval candleInterval, DateTime dateTime)
        {
            var candles = MarketDataProvider.GetCandlesAsync("BBG000BVPV84", candleInterval, dateTime, Provider.Tinkoff).GetAwaiter().GetResult();
            return candles;
        }

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
            var testData = GetCandles(CandleInterval.Hour, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }

        [Fact]
        public void HourCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-5000);
            //act
            DateTime testData = GetCandles(CandleInterval.Hour, dateTime).Candles.FirstOrDefault().Time;
            //assert
            Assert.True(DateTime.Compare(testData, dateTime) < 0);
        }

        [Fact]
        public void DayCandlesCount()
        {
            //arrange
            int candleCount = 500000;
            //act
            var testData = GetCandles(CandleInterval.Day, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }

        [Fact]
        public void DayCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-50000);
            //act
            DateTime testData = GetCandles(CandleInterval.Day, dateTime).Candles.FirstOrDefault().Time;
            //assert
            Assert.True(DateTime.Compare(testData, dateTime) < 0);
 
        }

        [Fact]
        public void WeekCandlesCount()
        {
            //arrange
            int candleCount = 50000;
            //act
            var testData = GetCandles(CandleInterval.Week, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }

        [Fact]
        public void WeekCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-100000);
            //act
            DateTime testData = GetCandles(CandleInterval.Week, dateTime).Candles.FirstOrDefault().Time;
            //assert
            Assert.True(DateTime.Compare(testData, dateTime) < 0);
        }

        [Fact]
        public void MonthCandlesCount()
        {
            //arrange
            int candleCount = 500000;
            //act
            var testData = GetCandles(CandleInterval.Month, candleCount).Candles.Count;
            //assert
            Assert.Equal(testData, candleCount);
        }

        [Fact]
        public void MonthCandlesDate()
        {
            //arrange
            DateTime dateTime = DateTime.Now.AddDays(-181000);
            //act
            DateTime testData = GetCandles(CandleInterval.Month, dateTime).Candles.FirstOrDefault().Time;
            //assert
            //Assert.True(DateTime.Compare(testData, dateTime) < 0);
            Assert.ThrowsAny<ArgumentException>(() => GetCandles(CandleInterval.Month, dateTime).Candles.FirstOrDefault().Time);
        }
    }
}
