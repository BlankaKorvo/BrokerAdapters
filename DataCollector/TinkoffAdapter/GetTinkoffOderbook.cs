using DataCollector.RetryPolicy;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapter
{
    internal class GetTinkoffOrderbook
    {
        readonly string figi;
        readonly int depth;
        public GetTinkoffOrderbook(string figi, int depth)
        {
            this.figi = figi;
            this.depth = depth;
        }
        /// <summary>
        /// Получение "стакана".
        /// </summary>
        /// <returns></returns>
        internal async Task<Orderbook> GetOrderbookAsync()
        {
                Orderbook orderbook = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.MarketOrderbookAsync(figi, depth)));
                return orderbook;
        }

    }
}
