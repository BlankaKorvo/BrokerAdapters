using Xunit;
using MarketDataModules.Candles;
using System;
using System.Linq;
using System.Collections.Generic;
using MarketDataModules;
using System.Threading.Tasks;
using MarketDataModules.Orderbooks;
using MarketDataModules.Instruments;

namespace DataCollector.Tests
{
    public class GetTinkoffInstrumentsPositiveTest
    {
        [Fact]
        public async Task GeinstrumentsTest()
        {
            //arrange
            int count = 0;
            //act
            IInstrumentList testData = await GetMarketData.GetInstrumentListAsync(Provider.Tinkoff);
            //assert
            Assert.True(testData.Instruments.Count > count);
        }

        [Fact]
        public async Task GetInstrumentByFigi()
        {
            //arrage
            string ticker = "AMZN";
            string figi = "BBG000BVPV84";
            //act
            IInstrument testData = await GetMarketData.GetInstrumentByFigiAsync(figi, Provider.Tinkoff);
            //assert
            Assert.Equal(ticker, testData.Ticker.ToString());
        }

        [Fact]
        public async Task GetInstrumentByTicker()
        {
            //arrage
            string ticker = "AMZN";
            string figi = "BBG000BVPV84";
            //act
            IInstrument testData = await GetMarketData.GetInstrumentByTickerAsync(ticker, Provider.Tinkoff);
            //assert
            Assert.Equal(figi, testData.Figi.ToString());
        }

    }
}
