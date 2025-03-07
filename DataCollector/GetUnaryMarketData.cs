﻿using DataCollector.TinkoffAdapterLegacy;
using MarketDataModules;
using MarketDataModules.Candles;
using MarketDataModules.Instruments;
using MarketDataModules.Orderbooks;
using MarketDataModules.Portfolio;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.InvestApi.V1;
using DataCollector.TinkoffAdapterGrpc;
using Google.Protobuf.WellKnownTypes;
using DataCollector.FinamAdapterOpenApi;

namespace DataCollector
{

    public static class GetUnaryMarketData
    {
        public static CandleList GetCandles(string uid, MarketDataModules.Candles.CandleInterval candleInterval, int candlesCount)
        {
            Tinkoff.InvestApi.V1.CandleInterval tinkoffInterval;
            switch (candleInterval)
            {
                case MarketDataModules.Candles.CandleInterval.Minute:
                    tinkoffInterval = Tinkoff.InvestApi.V1.CandleInterval._1Min; break;
                case MarketDataModules.Candles.CandleInterval.FiveMinutes:
                    tinkoffInterval = Tinkoff.InvestApi.V1.CandleInterval._5Min; break;
                case MarketDataModules.Candles.CandleInterval.QuarterHour:
                    tinkoffInterval = Tinkoff.InvestApi.V1.CandleInterval._15Min; break;
                case MarketDataModules.Candles.CandleInterval.Hour:
                    tinkoffInterval = Tinkoff.InvestApi.V1.CandleInterval.Hour; break;
                case MarketDataModules.Candles.CandleInterval.Day:
                    tinkoffInterval = Tinkoff.InvestApi.V1.CandleInterval.Day; break;
                default:
                    throw new InvalidOperationException();
            }
            GetCandlesRequest getCandlesRequest = new GetCandlesRequest() { InstrumentId = uid, From = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-10)), Interval = tinkoffInterval, To = Timestamp.FromDateTime(DateTime.UtcNow) };
            //var client = GetClient.Grpc;
            //GetTinkoffCandles getTinkoffCandles = new GetTinkoffCandles() { Client = client, CandlesRequest = getCandlesRequest, CandleCount = candlesCount };
            var historicCandles = GetTinkoffData.GetCandles(getCandlesRequest, candlesCount);

            if (historicCandles == null) return null;

            List<CandleStructure> listCandleStructure = new(historicCandles?.Select(x =>
                   new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time.ToDateTime(), x.IsComplete)));
            CandleList candlesList = new CandleList(uid, candleInterval, listCandleStructure);
            //List<HistoricCandle> candles =
            //    new(tinkoffCandles?.Candles.Select(x =>
            //        new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time, (CandleInterval)x.Interval, x.Figi)).Distinct());
            //CandlesList candlesList = new(tinkoffCandles.Figi, (CandleInterval)tinkoffCandles.Interval, candles);
            return candlesList;
        }
        public static Orderbook GetOrderbook(string id, int depth)
        {
            GetOrderBookRequest getOrderBookRequest = new GetOrderBookRequest() { InstrumentId = id, Depth = depth };
            GetOrderBookResponse orderBookTinkoff = GetTinkoffData.GetOrderbook(getOrderBookRequest);
            if (orderBookTinkoff == null) return null;
            Orderbook orderbook = new Orderbook(id, depth, OrderMap(orderBookTinkoff.Bids?.ToList()),
                OrderMap(orderBookTinkoff.Asks?.ToList()), ToDecimal(orderBookTinkoff.LastPrice),
                ToDecimal(orderBookTinkoff.ClosePrice), ToDecimal(orderBookTinkoff.LimitUp), ToDecimal(orderBookTinkoff.LimitDown),
                orderBookTinkoff.LastPriceTs.ToDateTime(), orderBookTinkoff.ClosePriceTs.ToDateTime(), orderBookTinkoff.OrderbookTs?.ToDateTime());
            return orderbook; 
            static List<MarketDataModules.Orderbooks.Order> OrderMap(List<Tinkoff.InvestApi.V1.Order> ordersT)
            {
                List<MarketDataModules.Orderbooks.Order> orders = new(ordersT.Select(x => new MarketDataModules.Orderbooks.Order(ToDecimal(x.Price), (int)x.Quantity)));
                return orders;
            }
        }

        public static Portfolio GetPortfolio(string accountId, Provider provider) => provider switch
        {
            Provider.Tinkoff => Mapping.MapPortfolioFromTinkoff(GetTinkoffData.GetPortfolio(new PortfolioRequest() { AccountId = accountId })),
            Provider.Finam => Mapping.MapPortfolioFromFinam(GetFinamData.GetPortfolio(accountId)),
            Provider.Alor => new Portfolio(),
            _ => new Portfolio()
        };
        public static InstrumentList GetShareList(Provider provider) => provider switch
        {
            Provider.Tinkoff => Mapping.MapInstrumentsFromTinkoffShares(GetTinkoffData.GetShares().Instruments.ToList()),
            Provider.Finam => new InstrumentList(),
            Provider.Alor => new InstrumentList(),
            _ => new InstrumentList()
        };

        public static InstrumentList GetEtfList(Provider provider) => provider switch
        {
            Provider.Tinkoff => Mapping.MapInstrumentsFromTinkoffEtfs(GetTinkoffData.GetEtfs().Instruments.ToList()),
            Provider.Finam => new InstrumentList(),
            Provider.Alor => new InstrumentList(),
            _ => new InstrumentList()
        };
        public static InstrumentList GetBondList(Provider provider) => provider switch
        {
            Provider.Tinkoff => Mapping.MapInstrumentsFromTinkoffBonds(GetTinkoffData.GetBonds().Instruments.ToList()),
            Provider.Finam => new InstrumentList(),
            Provider.Alor => new InstrumentList(),
            _ => new InstrumentList()
        };

        public static InstrumentList GetFutureList(Provider provider) => provider switch
        {
            Provider.Tinkoff => Mapping.MapInstrumentsFromTinkoffFutures(GetTinkoffData.GetFutures().Instruments.ToList()),
            Provider.Finam => new InstrumentList(),
            Provider.Alor => new InstrumentList(),
            _ => new InstrumentList()
        };

        public static MarketDataModules.Instruments.Instrument GetInstrument(Provider provider, string uid) => provider switch
        {
            Provider.Tinkoff => Mapping.MapInstrumentFromTinkoff(GetTinkoffData.GetInstrument(new InstrumentRequest() { Id = uid , IdType = InstrumentIdType.Uid})),
            Provider.Finam => new MarketDataModules.Instruments.Instrument(),
            Provider.Alor => new MarketDataModules.Instruments.Instrument(),
            _ => new MarketDataModules.Instruments.Instrument()
        };
        
        //public static Portfolio GetPortfolio(string uid)
        //{
        //    PortfolioRequest request = new PortfolioRequest() { AccountId = uid };
        //    PortfolioResponse portfolioResponse = GetTinkoffData.Portfolio(request);
        //    Portfolio p = new Portfolio() { 
        //        AccountId = portfolioResponse.AccountId, 
        //        ExpectedYield = portfolioResponse.ExpectedYield, 
        //        Positions = portfolioResponse.Positions.Select(x => new PortfolioPositionList() {
        //            PositionUid = x.PositionUid, 
        //            BlockedLots = x.BlockedLots, 
        //            Figi = x.Figi,
        //            ),

        //        }).ToList() };
        //}
        //public static Portfolio PortfolioTinkoff (PortfolioRequest portfolioRequest)
        //{
        //    PortfolioResponse portfolioResponce = GetTinkoffData.Portfolio(portfolioRequest);
        //    if (portfolioResponce == null) return null;
        //    Portfolio portfolio = new Portfolio() { TotalAmountShares = new MoneyAmount(Currency = portfolioResponce.TotalAmountShares.Currency) };
        //    return portfolio;
        //    static MoneyAmount MoneyAmountMap(Tinkoff.InvestApi.V1.MoneyValue moneyValue)
        //    {
        //        MoneyAmount ma = new(MarketDataModules.Candles.Currency = );
        //        return orders;
        //    }
        //}

        static decimal ToDecimal(Quotation quotation)
        { 
            decimal result = Convert.ToDecimal($"{quotation.Units},{quotation.Nano}");
            return result;
        }

        /// <summary>
        /// Получение свечей не позже определенной даты
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="dateFrom"></param> Дата, с которой запрашивается временной ряд (округление в зависимости от длины свечи по размеру максимального сета)
        /// <param name="providers"></param> Брокер
        /// <returns></returns>
        //public static CandlesList GetCandles
        //    (string figi, CandleInterval candleInterval, DateTime dateFrom, Provider provider = Provider.Tinkoff)
        //{

        //}
        ////=> provider switch
        ////{
        ////    Provider.Finam => throw new NotImplementedException(),
        ////    Provider.Alor => throw new NotImplementedException(),
        ////    Provider.Tinkoff => await GetTinkoffCandles(figi, candleInterval, dateFrom),
        ////    _ => throw new NotImplementedException()
        ////};

        ///// <summary>
        ///// Получение портфолио
        ///// </summary>
        ///// <param name="provider"></param> Брокер
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //public static async Task<Portfolio> GetPortfolioAsync(Provider provider = Provider.Tinkoff)
        //    => provider switch
        //    {
        //        Provider.Finam => throw new NotImplementedException(),
        //        Provider.Alor => throw new NotImplementedException(),
        //        Provider.Tinkoff => await GetTinkoffPortfolioAsync(),
        //        _ => throw new NotImplementedException()
        //    };

        ///// <summary>
        ///// Получение инструмента по идентификатору FIGI
        ///// </summary>
        ///// <param name="figi"></param> Идентификатор инструмента
        ///// <param name="provider"></param> Брокер
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //static public async Task<Instrument> GetInstrumentByFigiAsync(string figi, Provider provider = Provider.Tinkoff)
        //    => provider switch
        //    {
        //        Provider.Finam => throw new NotImplementedException(),
        //        Provider.Alor => throw new NotImplementedException(),
        //        Provider.Tinkoff => await GetTinkoffInstrumentByFigiAsync(figi),
        //        _ => throw new NotImplementedException()
        //    };

        ///// <summary>
        ///// Получение инструмента по имени(тикеру) инструмента
        ///// </summary>
        ///// <param name="ticker"></param> Имя инструмента(тикер)
        ///// <param name="provider"></param> Брокер
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //static public async Task<IInstrument> GetInstrumentByTickerAsync(string ticker, Provider provider = Provider.Tinkoff)
        //    => provider switch
        //    {
        //        Provider.Finam => throw new NotImplementedException(),
        //        Provider.Alor => throw new NotImplementedException(),
        //        Provider.Tinkoff => await GetTinkoffInstrumentByTickerAsync(ticker),
        //        _ => throw new NotImplementedException()
        //    };

        ///// <summary>
        ///// Получение списка всех акций, которым предоставляет доступ брокер
        ///// </summary>
        ///// <param name="provider"></param> Брокер
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //static public async Task<InstrumentList> GetInstrumentListAsync(Provider provider = Provider.Tinkoff)
        //    => provider switch
        //    {
        //        Provider.Finam => throw new NotImplementedException(),
        //        Provider.Alor => throw new NotImplementedException(),
        //        Provider.Tinkoff => await GetTinkoffInstrumentListAsync(),
        //        _ => throw new NotImplementedException()
        //    };

        ///// <summary>
        ///// Получение стакана
        ///// </summary>
        ///// <param name="figi"></param> Идентификатор инструмента
        ///// <param name="depth"></param> Глубина стакана
        ///// <param name="provider"></param> Брокер
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //static public async Task<Orderbook> GetOrderbookAsync(string figi, int depth, Provider provider = Provider.Tinkoff)
        //    => provider switch
        //    {
        //        Provider.Finam => throw new NotImplementedException(),
        //        Provider.Alor => throw new NotImplementedException(),
        //        Provider.Tinkoff => await GetTinkoffOrderbookAsync(figi, depth),
        //        _ => throw new NotImplementedException()
        //    };



        ///// <summary>
        ///// Получение инструмента по идентификатору FIGI от tinkoff
        ///// </summary>
        ///// <param name="figi"></param> Идентификатор инструмента
        ///// <returns></returns>
        //private static async Task<Instrument> GetTinkoffInstrumentByFigiAsync(string figi)
        //{
        //    Tinkoff.Trading.OpenApi.Models.MarketInstrument instrumentT = await new GetTinkoffCurrentInstrument().GetMarketInstrumentByFigi(figi);
        //    Instrument instrument = new(instrumentT.Figi, instrumentT.Ticker,
        //        instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
        //        (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
        //    return instrument;
        //}

        ///// <summary>
        ///// Получение инструмента по имени(тикеру) инструмента от tinkoff
        ///// </summary>
        ///// <param name="ticker"></param> Тикер инструмента
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //private static async Task<Instrument> GetTinkoffInstrumentByTickerAsync(string ticker)
        //{
        //    Tinkoff.Trading.OpenApi.Models.MarketInstrumentList instrumentList = await new GetTinkoffCurrentInstrument().GetMarketInstrumentListByTicker(ticker);
        //    Tinkoff.Trading.OpenApi.Models.MarketInstrument instrumentT;
        //    if (instrumentList.Instruments.Count == 1)
        //    {
        //        instrumentT = instrumentList.Instruments.FirstOrDefault();
        //    }
        //    else
        //    {
        //        throw new Exception("При поиске по ticker - кол-во найденных инструментов = " + instrumentList.Instruments.Count);
        //    }
        //    Instrument instrument = new(instrumentT.Figi, instrumentT.Ticker,
        //        instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
        //        (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
        //    return instrument;
        //}

        ///// <summary>
        ///// Получение списка всех акций, которым предоставляет доступ tinkoff
        ///// </summary>
        ///// <returns></returns>
        //static async Task<InstrumentList> GetTinkoffInstrumentListAsync()
        //{
        //    Tinkoff.Trading.OpenApi.Models.MarketInstrumentList tinkoffStocks = await new GetTinkoffInstruments().GetAllAsync();
        //    InstrumentList stocks =
        //        new(tinkoffStocks.Total, tinkoffStocks.Instruments.Select(x =>
        //            new Instrument(x.Figi, x.Ticker, x.Isin, x.MinPriceIncrement, x.Lot, (Currency)x.Currency, x.Name, (InstrumentType)x.Type)).ToList());
        //    return stocks;
        //}

        ///// <summary>
        ///// Получение портфолио инструментов Tinkoff
        ///// </summary>
        ///// <returns></returns>
        //private static async Task<Portfolio> GetTinkoffPortfolioAsync()
        //{
        //    Tinkoff.Trading.OpenApi.Models.Portfolio portfolioTinkoff = await new GetTinkoffPortfolio().GetPortfolioAsync();
        //    var positions = new List<Portfolio.Position> { };
        //    foreach (var item in portfolioTinkoff.Positions)
        //    {
        //        string name = item.Name;
        //        string figi = item.Figi;
        //        string ticker = item.Ticker;
        //        string isin = item.Isin;
        //        InstrumentType instrumentType = (InstrumentType)item.InstrumentType;
        //        decimal balance = item.Balance;
        //        decimal blocked = item.Blocked;
        //        int lots = item.Lots;
        //        if (item?.AveragePositionPrice != null)
        //        {
        //            Currency currencyExpectedYield = (Currency)item.ExpectedYield.Currency;
        //            MoneyAmount ExpectedYield = new(currencyExpectedYield, item.ExpectedYield.Value);

        //            Currency currencyAveragePositionPrice = (Currency)item.AveragePositionPrice.Currency;
        //            MoneyAmount AveragePositionPrice = new(currencyAveragePositionPrice, item.AveragePositionPrice.Value);
        //            Currency currencyAveragePositionPriceNoNkd = (Currency)item.AveragePositionPriceNoNkd.Currency;

        //            MoneyAmount AveragePositionPriceNoNkd = new(currencyAveragePositionPriceNoNkd, item.AveragePositionPriceNoNkd.Value);
        //            Portfolio.Position position = new(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);


        //            positions.Add(position);
        //        }
        //        //else
        //        //{
        //        //    MoneyAmount ExpectedYield = null;
        //        //    MoneyAmount AveragePositionPrice = null;
        //        //    MoneyAmount AveragePositionPriceNoNkd = null;
        //        //    Portfolio.Position position = new(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);
        //        //    positions.Add(position);
        //        //}
        //    }
        //    Portfolio portfolio = new(positions);
        //    return portfolio;
        //}

        ///// <summary>
        ///// Получение стакана по инструменту от tinkoff
        ///// </summary>
        ///// <param name="figi"></param> Идентификатор инструмента
        ///// <param name="depth"></param> Глубина стакана
        ///// <returns></returns>
        //static private async Task<Orderbook> GetTinkoffOrderbookAsync(string figi, int depth)
        //{
        //    Tinkoff.Trading.OpenApi.Models.Orderbook tinOrderbook = await new GetTinkoffOrderbook(figi, depth).GetOrderbookAsync();

        //    List<OrderbookEntry> bids = new(tinOrderbook?.Bids.Select(x => new OrderbookEntry(x.Quantity, x.Price)).OrderByDescending(p => p.Price));
        //    List<OrderbookEntry> asks = new(tinOrderbook?.Asks.Select(x => new OrderbookEntry(x.Quantity, x.Price)).OrderBy(p => p.Price));
        //    TradeStatus tradeStatus = (TradeStatus)tinOrderbook.TradeStatus;
        //    Orderbook orderbook =
        //        new(tinOrderbook.Depth, bids, asks, tinOrderbook.Figi, tradeStatus, tinOrderbook.MinPriceIncrement, tinOrderbook.FaceValue,
        //            tinOrderbook.LastPrice, tinOrderbook.ClosePrice, tinOrderbook.LimitUp, tinOrderbook.LimitDown);
        //    return orderbook;
        //}

        ///// <summary>
        ///// Получение свечей Tinkoff по указанному кол-ву элементов временного ряда
        ///// </summary>
        ///// <param name="figi"></param> Идентификатор инструмента
        ///// <param name="candleInterval"></param> Длина одной свечи
        ///// <param name="candlesCount"></param> Кол-во свечей в запрашиваемом временном ряду
        ///// <returns></returns>
        //static async Task<CandlesList?> GetTinkoffCandles()
        //{
        //    List<HistoricCandle> tinkoffCandles = new GetTinkoffCandles().GetCandlesTinkof();
        //    CandlesList candlesList = TinkoffCandlesMapper(tinkoffCandles);
        //    return candlesList;
        //}

        /// <summary>
        /// Получение свечей у брокера Tinkoff по указанной дате начала временного ряда
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="dateTime"></param> Дата, с которой запрашивается временной ряд (округление в зависимости от длины свечи по размеру максимального сета)
        /// <returns></returns>
        //static async Task<CandlesList> GetTinkoffCandles(string figi, CandleInterval candleInterval, DateTime dateTime)
        //{
        //    Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles = await new GetTinkoffCandles(figi, (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval, dateTime).GetCandlesTinkoffLegacyAsync();
        //    CandlesList candlesList = TinkoffCandlesMapper(tinkoffCandles);
        //    Log.Information($"Return {candlesList.Candles.Count} GetTinkoffCandles");
        //    return candlesList;
        //}

        ///// <summary>
        ///// Приведение объекта от модели Tinkoff.Trading.OpenApi к MarketDataModules
        ///// </summary>
        ///// <param name="tinkoffCandles"></param> Tinkoff.Trading.OpenApi.Models.CandleList
        ///// <returns></returns>
        //static CandlesList TinkoffCandlesMapper(Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles)
        //{
        //    List<CandleStructure> candles =
        //        new(tinkoffCandles?.Candles.Select(x =>
        //            new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time, (CandleInterval)x.Interval, x.Figi)).Distinct());
        //    CandlesList candlesList = new(tinkoffCandles.Figi, (CandleInterval)tinkoffCandles.Interval, candles);
        //    return candlesList;
        //}
    }
}


