using DataCollector.RetryPolicy;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapter
{
    public class GetTinkoffCurrentInstrument
    {
        internal async Task<MarketInstrument> GetMarketInstrumentByFigi(string figi)
        {
            MarketInstrument instrument = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketSearchByFigiAsync(figi)));
            return instrument;
        }
        internal async Task<MarketInstrumentList> GetMarketInstrumentListByTicker(string ticker)
        {
            MarketInstrumentList instrument = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketSearchByTickerAsync(ticker)));
            return instrument;
        }
    }
}
