using MarketDataModules.Portfolio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.InvestApi.V1;

namespace DataCollector
{
    internal static class Mapping
    {
        internal static Portfolio MapPortfolioFromTinkoff(PortfolioResponse followPortfolioResponse)
        {
            var result = new Portfolio()
            {
                TotalAmountCurrencies = new MoneyAmount(MarketDataModules.Candles.Currency.Rub, followPortfolioResponse.TotalAmountCurrencies),
                AccountId = followPortfolioResponse.AccountId,
                ExpectedYield = followPortfolioResponse.ExpectedYield,
                TotalAmountBonds = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountBonds) ? null : 
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountBonds?.Currency), followPortfolioResponse?.TotalAmountBonds),
                TotalAmountEtf = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountEtf) ? null :
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountEtf?.Currency), followPortfolioResponse?.TotalAmountEtf),
                TotalAmountFutures = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountFutures) ? null :
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountFutures?.Currency), followPortfolioResponse?.TotalAmountFutures),
                TotalAmountOptions = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountOptions) ? null :
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountOptions?.Currency), followPortfolioResponse?.TotalAmountOptions),
                TotalAmountShares = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountShares) ? null :
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountShares?.Currency), followPortfolioResponse?.TotalAmountShares),
                TotalAmountPortfolio = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountPortfolio) ? null :
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountPortfolio?.Currency), followPortfolioResponse?.TotalAmountPortfolio),
                TotalAmountSp = IsNullOrEmptyMoneyValue(followPortfolioResponse?.TotalAmountSp) ? null :
                    new MoneyAmount(GetCurrency(followPortfolioResponse?.TotalAmountSp?.Currency), followPortfolioResponse?.TotalAmountSp),
                VirtualPositions = followPortfolioResponse?.VirtualPositions?.Select(x => new PortfolioPositionList()
                {
                    AveragePositionPrice = IsNullOrEmptyMoneyValue(x?.AveragePositionPrice) ? null : new MoneyAmount(GetCurrency(x?.AveragePositionPrice?.Currency), x?.AveragePositionPrice),
                    AveragePositionPriceFifo = IsNullOrEmptyMoneyValue(x?.AveragePositionPriceFifo) ? null : new MoneyAmount(GetCurrency(x?.AveragePositionPriceFifo?.Currency), x?.AveragePositionPriceFifo),
                    AveragePositionPricePt = default, //HZ
                    Figi = x?.Figi,
                    Blocked = false,
                    BlockedLots = default,
                    CurrentNkd = default,
                    CurrentPrice = IsNullOrEmptyMoneyValue(x?.CurrentPrice) ? null : new MoneyAmount(GetCurrency(x?.CurrentPrice?.Currency), x.CurrentPrice),
                    ExpectedYield = x?.ExpectedYield,
                    ExpectedYieldFifo = x?.ExpectedYieldFifo,
                    InstrumentType = x?.InstrumentType,
                    InstrumentUid = x?.InstrumentUid,
                    PositionUid = x?.PositionUid,
                    Quantity = x?.Quantity,
                    QuantityLots = x?.Quantity,
                    VarMargin = default
                }).ToList(),
                Positions = followPortfolioResponse?.Positions?.Select(x => new PortfolioPositionList()
                {
                    Figi = x?.Figi,
                    AveragePositionPrice = IsNullOrEmptyMoneyValue(x?.AveragePositionPrice) ? null : new MoneyAmount(GetCurrency(x?.AveragePositionPrice?.Currency), x?.AveragePositionPrice),
                    AveragePositionPriceFifo = IsNullOrEmptyMoneyValue(x?.AveragePositionPriceFifo) ? null : new MoneyAmount(GetCurrency(x?.AveragePositionPriceFifo?.Currency), x?.AveragePositionPriceFifo),
                    AveragePositionPricePt = x?.AveragePositionPricePt,
                    Blocked = x?.Blocked,
                    BlockedLots = x?.BlockedLots,
                    CurrentNkd = IsNullOrEmptyMoneyValue(x?.CurrentNkd) ? null : new MoneyAmount(GetCurrency(x?.CurrentNkd?.Currency), x?.CurrentNkd),
                    CurrentPrice = IsNullOrEmptyMoneyValue(x?.CurrentPrice) ? null : new MoneyAmount(GetCurrency(x?.CurrentPrice?.Currency), x?.CurrentPrice),
                    ExpectedYield = x?.ExpectedYield,
                    ExpectedYieldFifo = x?.ExpectedYieldFifo,
                    InstrumentType = x?.InstrumentType,
                    InstrumentUid = x?.InstrumentUid,
                    PositionUid = x?.PositionUid,
                    Quantity = x?.Quantity,
                    QuantityLots = x?.QuantityLots,
                    VarMargin = IsNullOrEmptyMoneyValue(x?.VarMargin) ? null : new MoneyAmount(GetCurrency(x?.VarMargin.Currency), x?.VarMargin)
                }).ToList()
            };
            return result;

            static MarketDataModules.Candles.Currency GetCurrency(string currency) => currency.ToUpper() switch
            {
                "RUB" => MarketDataModules.Candles.Currency.Rub,
                "USD" => MarketDataModules.Candles.Currency.Usd,
                "EUR" => MarketDataModules.Candles.Currency.Eur,
                _ => throw new NotImplementedException()
            };

            static bool IsNullOrEmptyMoneyValue(MoneyValue moneyValue)
            {
                return moneyValue == null || string.IsNullOrEmpty(moneyValue?.Currency);
            }
        }
    }
}
