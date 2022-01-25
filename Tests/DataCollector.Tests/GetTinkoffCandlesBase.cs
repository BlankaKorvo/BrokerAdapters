using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Threading.Tasks;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesBase
    {
        string figi = "BBG000BVPV84";
        DateTime dateFrom = default;
        protected async Task<ICandlesList> GetCandles(CandleInterval candleInterval, DateTime dateTime)
        {
            var candles = await GetMarketData.GetCandlesAsync(figi, candleInterval, dateTime, Provider.Tinkoff);
            return candles;
        }
        //protected async Task<Tinkoff.Trading.OpenApi.Models.CandleList> GetRealCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval candleInterval)
        //    => candleInterval switch
        //    {
        //        Tinkoff.Trading.OpenApi.Models.CandleInterval.Minute => dateFrom = DateTime.Now.AddHours(-23)
        //        _ => null
        //    };

        //    return await TinkoffAdapter.Authorisation.Context.MarketCandlesAsync(figi, DateTime.Now.AddHours(-23), DateTime.Now, candleInterval);
        //}

        protected async Task<ICandlesList> GetCandles(CandleInterval candleInterval, int candleCount)
        {
            var candles = await GetMarketData.GetCandlesAsync("BBG000BVPV84", candleInterval, candleCount, Provider.Tinkoff);
            return candles;
        }
    }
}