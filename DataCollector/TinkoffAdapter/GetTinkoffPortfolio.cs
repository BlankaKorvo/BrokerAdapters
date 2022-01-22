using DataCollector.RetryPolicy;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapter
{
    internal class GetTinkoffPortfolio
    {
        internal async Task<Portfolio> GetPortfolioAsync()
        {
            Portfolio portfolio = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PortfolioAsync()));
            return portfolio;
        }

        internal async Task<PortfolioCurrencies> GetPortfolioAsyncCurrenciesAsync()
        {
            PortfolioCurrencies portfolio = await PollyRetrayPolitics.Retry().ExecuteAsync(async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync(async () => await Authorisation.Context.PortfolioCurrenciesAsync()));
            return portfolio;
        }


    }
}
