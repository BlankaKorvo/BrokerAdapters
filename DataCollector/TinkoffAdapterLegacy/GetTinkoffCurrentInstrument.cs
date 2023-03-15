//using DataCollector.RetryPolicy;
//using System.Threading.Tasks;
//using Tinkoff.Trading.OpenApi.Models;

//namespace DataCollector.TinkoffAdapterLegacy
//{
//    public class GetTinkoffCurrentInstrument
//    {
//        /// <summary>
//        /// Получение инструмента по figi
//        /// </summary>
//        /// <param name="figi"></param>
//        /// <returns></returns>
//        internal async Task<MarketInstrument> GetMarketInstrumentByFigi(string figi)
//        {
//            MarketInstrument instrument = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketSearchByFigiAsync(figi)));
//            return instrument;
//        }
//        /// <summary>
//        /// Получение инструмента по ticker
//        /// </summary>
//        /// <param name="ticker"></param>
//        /// <returns></returns>
//        internal async Task<MarketInstrumentList> GetMarketInstrumentListByTicker(string ticker)
//        {
//            MarketInstrumentList instrument = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketSearchByTickerAsync(ticker)));
//            return instrument;
//        }
//    }
//}
