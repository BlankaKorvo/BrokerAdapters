using Finam.TradeApi.Proto.V1;
using MarketDataModules.Instruments;
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
                    SecurityCode = x?.Figi,
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
                    SecurityCode = x?.Figi,
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

            static bool IsNullOrEmptyMoneyValue(MoneyValue moneyValue)
            {
                return moneyValue == null || string.IsNullOrEmpty(moneyValue?.Currency);
            }
        }

        internal static Portfolio MapPortfolioFromFinam(GetPortfolioResult finamPortfolio)
        {
            var result = new Portfolio()
            {
                TotalAmountCurrencies = new MoneyAmount(MarketDataModules.Candles.Currency.Rub, Convert.ToDecimal(finamPortfolio.Money.FirstOrDefault(m => m.Currency == "RUB").Balance)),
                AccountId = finamPortfolio.ClientId,
                ExpectedYield = default,
                TotalAmountBonds = default,
                TotalAmountEtf = default,
                TotalAmountFutures = default,
                TotalAmountOptions = default,
                TotalAmountShares = default,
                TotalAmountPortfolio = IsNullOrEmptyMoneyValue(finamPortfolio?.Equity) ? null : new MoneyAmount(GetCurrency("RUB"), Convert.ToDecimal(finamPortfolio?.Equity)),
                TotalAmountSp = default,
                VirtualPositions = default,
                Positions = finamPortfolio?.Positions?.Select(x => new PortfolioPositionList()
                {
                    SecurityCode = x.SecurityCode,
                    AveragePositionPrice = IsNullOrEmptyMoneyValue(x?.AveragePrice) ? null : new MoneyAmount(GetCurrency(x?.AveragePriceCurrency), Convert.ToDecimal(x?.AveragePrice)),
                    AveragePositionPriceFifo = IsNullOrEmptyMoneyValue(x?.AveragePrice) ? null : new MoneyAmount(GetCurrency(x?.AveragePriceCurrency), Convert.ToDecimal(x?.AveragePrice)),
                    CurrentPrice = IsNullOrEmptyMoneyValue(x?.CurrentPrice) ? null : new MoneyAmount(GetCurrency(x?.Currency), Convert.ToDecimal(x?.CurrentPrice)),
                    InstrumentType = x?.Market.ToString(),
                    Quantity = Convert.ToDecimal(x?.Balance),
                    QuantityLots = Convert.ToDecimal(x?.Balance),
                    PositionUid = x?.SecurityCode
                }).ToList()
            };
            return result;

            static bool IsNullOrEmptyMoneyValue<T>(T moneyValue)
            {
                return moneyValue == null || string.IsNullOrEmpty(moneyValue.ToString());
            }
        }

        private static MarketDataModules.Candles.Currency GetCurrency(string currency) => currency.ToUpper() switch
        {
            "RUB" => MarketDataModules.Candles.Currency.Rub,
            "USD" => MarketDataModules.Candles.Currency.Usd,
            "EUR" => MarketDataModules.Candles.Currency.Eur,
            "HKD" => MarketDataModules.Candles.Currency.Hkd,
            _ => MarketDataModules.Candles.Currency.Rub,
        };

        internal static InstrumentList MapInstrumentsFromTinkoffShares(List<Share> shareList)
        {
            var result = new InstrumentList()
            {
                Count = shareList.Count,
                Instruments = shareList.Select(x => new MarketDataModules.Instruments.Instrument()
                {
                    Figi = x.Figi,
                    ClassCode = x.ClassCode,
                    Exchange = x.Exchange,
                    Isin = x.Isin,
                    Lot = x.Lot,
                    //MinPriceIncrement = x.MinPriceIncrement,
                    Name = x.Name,
                    RealExchange = x.RealExchange.ToString(),
                    ShortEnabledFlag = x.ShortEnabledFlag,
                    Ticker = x.Ticker,
                    TinkoffUid = x.Uid,
                    Type = MarketDataModules.Instruments.InstrumentType.Stock,
                    Currency = GetCurrency(x.Currency)

                }).ToList()
            };            
            return result;
        }

        internal static InstrumentList MapInstrumentsFromTinkoffEtfs(List<Etf> etfList)
        {
            var result = new InstrumentList()
            {
                Count = etfList.Count,
                Instruments = etfList.Select(x => new MarketDataModules.Instruments.Instrument()
                {
                    Figi = x.Figi,
                    ClassCode = x.ClassCode,
                    Exchange = x.Exchange,
                    Isin = x.Isin,
                    Lot = x.Lot,
                    //MinPriceIncrement = x.MinPriceIncrement,
                    Name = x.Name,
                    RealExchange = x.RealExchange.ToString(),
                    ShortEnabledFlag = x.ShortEnabledFlag,
                    Ticker = x.Ticker,
                    TinkoffUid = x.Uid,
                    Type = MarketDataModules.Instruments.InstrumentType.Etf,
                    Currency = GetCurrency(x.Currency)

                }).ToList()
            };
            return result;
        }

        internal static InstrumentList MapInstrumentsFromTinkoffBonds(List<Bond> bondList)
        {
            var Instrumend = bondList.Select(x => new MarketDataModules.Instruments.Instrument()
            {
                Figi = x.Figi,
                ClassCode = x.ClassCode,
                Exchange = x.Exchange,
                Isin = x.Isin,
                Lot = x.Lot,
                //MinPriceIncrement = x.MinPriceIncrement,
                Name = x.Name,
                RealExchange = x.RealExchange.ToString(),
                ShortEnabledFlag = x.ShortEnabledFlag,
                Ticker = x.Ticker,
                TinkoffUid = x.Uid,
                Type = MarketDataModules.Instruments.InstrumentType.Bond,
                Currency = GetCurrency(x.Currency)

            }).ToList();
            var result = new InstrumentList()
            {
                Count = bondList.Count,
                Instruments = Instrumend,
            };
            return result;
        }
        internal static InstrumentList MapInstrumentsFromTinkoffFutures(List<Future> futureList)
        {
            var Instrumend = futureList.Select(x => new MarketDataModules.Instruments.Instrument()
            {
                Figi = x.Figi,
                ClassCode = x.ClassCode,
                Exchange = x.Exchange,
                //Isin = x.Isin,
                Lot = x.Lot,
                //MinPriceIncrement = x.MinPriceIncrement,
                Name = x.Name,
                RealExchange = x.RealExchange.ToString(),
                ShortEnabledFlag = x.ShortEnabledFlag,
                Ticker = x.Ticker,
                TinkoffUid = x.Uid,
                Type = MarketDataModules.Instruments.InstrumentType.Bond,
                Currency = GetCurrency(x.Currency)

            }).ToList();
            var result = new InstrumentList()
            {
                Count = futureList.Count,
                Instruments = Instrumend,
            };
            return result;
        }

        internal static MarketDataModules.Instruments.Instrument MapInstrumentFromTinkoff(Tinkoff.InvestApi.V1.InstrumentResponse instrumentResponse)
        {
            var result = new MarketDataModules.Instruments.Instrument()
            {
                Figi = instrumentResponse.Instrument.Figi,
                ClassCode = instrumentResponse.Instrument.ClassCode,   
                Exchange = instrumentResponse.Instrument.Exchange,
                Lot = instrumentResponse.Instrument.Lot,
                Name = instrumentResponse.Instrument.Name,
                RealExchange = instrumentResponse.Instrument.RealExchange.ToString(),
                ShortEnabledFlag = instrumentResponse.Instrument.ShortEnabledFlag,
                Ticker = instrumentResponse.Instrument.Ticker,
                TinkoffUid = instrumentResponse.Instrument.Uid,
                Currency = GetCurrency(instrumentResponse.Instrument.Currency),
                Isin = instrumentResponse.Instrument.Isin,
            };          
            return result;
        }
    }
}
