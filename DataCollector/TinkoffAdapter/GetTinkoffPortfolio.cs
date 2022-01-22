using DataCollector.RetryPolicy;
using DataCollector.TinkoffAdapter.Authority;
using DataCollector.TinkoffAdapter.DataHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
