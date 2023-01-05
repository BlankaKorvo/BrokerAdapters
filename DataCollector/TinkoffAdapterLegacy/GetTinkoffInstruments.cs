using DataCollector.RetryPolicy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapterLegacy
{
    public class GetTinkoffInstruments
    {
        /// <summary>
        /// Получение всех акций
        /// </summary>
        /// <returns></returns>
        internal async Task<MarketInstrumentList> GetStocksAsync()
        {
            MarketInstrumentList instruments = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketStocksAsync()));
            return instruments;
        }

        /// <summary>
        /// Получение всех bonds
        /// </summary>
        /// <returns></returns>
        internal async Task<MarketInstrumentList> GetBondsAsync()
        {
            MarketInstrumentList instruments = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketBondsAsync()));
            return instruments;
        }
        /// <summary>
        /// Получение всех ETF
        /// </summary>
        /// <returns></returns>
        internal async Task<MarketInstrumentList> GetEtfsAsync()
        {
            MarketInstrumentList instruments = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketEtfsAsync()));
            return instruments;
        }

        internal async Task<MarketInstrumentList> GetAllAsync()
        {
            MarketInstrumentList stocks = await GetStocksAsync();
            MarketInstrumentList bonds = await GetBondsAsync();
            MarketInstrumentList etfs = await GetEtfsAsync();
            List<MarketInstrument> allInstruments = stocks.Instruments.Union(bonds.Instruments.Union(etfs.Instruments)).ToList();
            MarketInstrumentList instruments = new(allInstruments.Count, allInstruments);
            return instruments;
        }
    }
}
