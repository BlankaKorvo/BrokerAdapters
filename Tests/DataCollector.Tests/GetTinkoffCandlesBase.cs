using DataCollector.RetryPolicy;
using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Threading.Tasks;

namespace DataCollector.Tests
{
    public class GetTinkoffCandlesBase
    {
        string figi = "BBG000BVPV84";
        DateTime dateFrom = DateTime.Now;

        protected async Task<CandlesList> GetCandles(CandleInterval candleInterval, DateTime dateTime)
        {
            var candles = await GetMarketData.GetCandlesAsync(figi, candleInterval, dateTime, Provider.Tinkoff);
            return candles;
        }
        protected async Task<Tinkoff.Trading.OpenApi.Models.CandleList> GetRealCandles(Tinkoff.Trading.OpenApi.Models.CandleInterval candleInterval)
        {
            if (candleInterval < Tinkoff.Trading.OpenApi.Models.CandleInterval.Day)
                dateFrom = dateFrom.AddHours(-1);
            else if (candleInterval == Tinkoff.Trading.OpenApi.Models.CandleInterval.Day)
                dateFrom = dateFrom.AddDays(-2);
            else if (candleInterval == Tinkoff.Trading.OpenApi.Models.CandleInterval.Week)
                dateFrom = dateFrom.AddDays(-14);
            else
                dateFrom = dateFrom.AddMonths(-2);
            return await PollyRetrayPolitics.Retry().ExecuteAsync(async () => 
                            await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => 
                            await TinkoffAdapter.Authorisation.Context.MarketCandlesAsync(figi, dateFrom, DateTime.Now, candleInterval)));
        }

        protected async Task<CandlesList> GetCandles(CandleInterval candleInterval, int candleCount)
        {
            var candles = await GetMarketData.GetCandlesAsync("BBG000BVPV84", candleInterval, candleCount, Provider.Tinkoff);
            return candles;
        }
    }
}