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
        public async Task GetOrderbookTest()
        {
            //arrange
            int count = 0;
            //act
            IInstrumentList testData = await GetMarketData.GetInstrumentListAsync(Provider.Tinkoff);
            //assert
            Assert.True(testData.Instruments.Count > count);
        }
    }
}
