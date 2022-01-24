using Xunit;
using MarketDataModules.Candles;
using System;
using System.Linq;
using System.Collections.Generic;
using MarketDataModules;
using System.Threading.Tasks;
using MarketDataModules.Orderbooks;

namespace DataCollector.Tests
{
    public class GetTinkoffOrderbookPositiveTest
    {
        [Fact]
        public async Task GetOrderbookTest()
        {
            //arrange
            int depth = 50;
            //act
            IOrderbook testData = await GetMarketData.GetOrderbookAsync("BBG000BVPV84", depth, Provider.Tinkoff);
            //assert
            Assert.True(testData?.Asks.Count > 0);
            Assert.True(testData?.Bids.Count > 0);
        }
    }
}
