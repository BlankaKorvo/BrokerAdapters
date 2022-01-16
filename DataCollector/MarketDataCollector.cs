using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketDataModules;
using Serilog;
using Tinkoff.Trading.OpenApi.Models;
using CandleInterval = MarketDataModules.Candles.CandleInterval;
using Currency = MarketDataModules.Candles.Currency;
using InstrumentType = MarketDataModules.Instruments.InstrumentType;
using Orderbook = MarketDataModules.Orderbooks.Orderbook;
using OrderbookEntry = MarketDataModules.Orderbooks.OrderbookEntry;
using TradeStatus = MarketDataModules.Orderbooks.TradeStatus;
using Portfolio = MarketDataModules.Portfolio.Portfolio;
using MoneyAmount = MarketDataModules.Portfolio.MoneyAmount;
using MarketDataModules.Candles;
using MarketDataModules;
using MarketDataModules.Instruments;
using MarketDataModules.Operation;
using DataCollector.TinkoffAdapter;

namespace DataCollector
{


    public static class MarketDataCollector// : GetTinkoffData // : ICandlesList
    {
        //GetTinkoffData getTinkoffData = new GetTinkoffData();

        public static async Task<CandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
        {
            switch (providers)
            {
                case Provider.Tinkoff:
                    return await TinkoffCandles(figi, candleInterval, candlesCount);
                case Provider.Finam:
                    return null;
            }
            return null;
        }

        static public async Task<CandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, DateTime dateTime, Provider providers = Provider.Tinkoff)
        {
            switch (providers)
            {
                case Provider.Tinkoff:
                    return await TinkoffCandles(figi, candleInterval, dateTime);
                case Provider.Finam:
                    return null;
            }
            return null;
        }
        static public async Task<Portfolio> GetPortfolioAsync(Provider provider = Provider.Tinkoff)
        {
            if (provider == Provider.Tinkoff)
            {
                Tinkoff.Trading.OpenApi.Models.Portfolio portfolioTinkoff = await GetTinkoffData.GetPortfolioAsync();
                //Lis
                //Portfolio.Position position = new Portfolio.Position(portfolioTinkoff.Positions.Select(x=> x.Name), 
                List<Portfolio.Position> positions = new List<Portfolio.Position> { };
                foreach (var item in portfolioTinkoff.Positions)
                {
                    string name = item.Name;
                    string figi = item.Figi;
                    string ticker = item.Ticker;
                    string isin = item.Isin;
                    InstrumentType instrumentType = (InstrumentType)item.InstrumentType;
                    decimal balance = item.Balance;
                    decimal blocked = item.Blocked;
                    int lots = item.Lots;
                    if (item?.AveragePositionPrice != null)
                    {
                        Currency currencyExpectedYield = (Currency)item.ExpectedYield.Currency;
                        MoneyAmount ExpectedYield = new MoneyAmount(currencyExpectedYield, item.ExpectedYield.Value);

                        Currency currencyAveragePositionPrice = (Currency)item.AveragePositionPrice.Currency;
                        MoneyAmount AveragePositionPrice = new MoneyAmount(currencyAveragePositionPrice, item.AveragePositionPrice.Value);
                        Currency currencyAveragePositionPriceNoNkd = (Currency)item.AveragePositionPriceNoNkd.Currency;

                        MoneyAmount AveragePositionPriceNoNkd = new MoneyAmount(currencyAveragePositionPriceNoNkd, item.AveragePositionPriceNoNkd.Value);
                        Portfolio.Position position = new Portfolio.Position(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);


                        positions.Add(position);
                    }
                    else
                    {
                        MoneyAmount ExpectedYield = null;
                        MoneyAmount AveragePositionPrice = null;
                        MoneyAmount AveragePositionPriceNoNkd = null;
                        Portfolio.Position position = new Portfolio.Position(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);


                        positions.Add(position);
                    }

                }
                Portfolio portfolio = new Portfolio(positions);
                return portfolio;
            }
            else
                throw new NotImplementedException();
        }

        static public async Task<Instrument> GetInstrumentByFigi(string figi, Provider provider = Provider.Tinkoff)
        {
            if (provider == Provider.Tinkoff)
            {
                MarketInstrument instrumentT = await GetTinkoffData.GetMarketInstrumentByFigi(figi);
                Instrument instrument = new Instrument(instrumentT.Figi, instrumentT.Ticker,
                    instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
                    (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
                return instrument;
            }
            else
                throw new NotImplementedException();
        }

        static public async Task<Instrument> GetInstrumentByTickerAsync(string ticker, Provider provider = Provider.Tinkoff)
        {
            if (provider == Provider.Tinkoff)
            {
                MarketInstrument instrumentT = null;
                MarketInstrumentList instrumentList = await GetTinkoffData.GetMarketInstrumentListByTicker(ticker);
                if (instrumentList.Instruments.Count == 1)
                {
                    instrumentT = instrumentList.Instruments.FirstOrDefault();
                }
                else
                {
                    throw new Exception("При поиске по ticker - кол-во найденных инструментов = " + instrumentList.Instruments.Count);
                }
                Instrument instrument = new Instrument(instrumentT.Figi, instrumentT.Ticker,
                    instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
                    (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
                return instrument;
            }
            else
                throw new NotImplementedException();
        }
        static public async Task<InstrumentList> GetInstrumentListAsync(Provider provider = Provider.Tinkoff)
        {
            if (provider == Provider.Tinkoff)
                return await TinkoffInstrumentList();
            else
                throw new NotImplementedException();

        }

        static public async Task<Orderbook> GetOrderbookAsync(string figi, Provider provider = Provider.Tinkoff, int depth = 20)
        {
            if (provider == Provider.Tinkoff)
                return await TinkoffOrderbook(figi, depth);
            else
                throw new NotImplementedException();
        }

        //Получение списка candlelist из списка инструментов
        static public async Task<List<CandlesList>> GetListCandlesByInstrumentsAsync(InstrumentList instrumentList, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
        {
            List<CandlesList> listCandlesList = new List<CandlesList>();
            foreach (var item in instrumentList.Instruments)
            {
                CandlesList candlesList = await GetCandlesAsync(item.Figi, candleInterval, candlesCount, providers);
                if (candlesList == null)
                {
                    continue;
                }
                else
                {
                    listCandlesList.Add(candlesList);
                }
            }
            return listCandlesList;
        }

        /// <summary>
        /// Получени истории операций по инструменту
        /// </summary>
        /// <param name="figi"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        static public async Task<List<TradeOperation>> GetOperationsAsync(string figi, DateTime dateFrom, DateTime dateTo, Provider provider = Provider.Tinkoff)
        {
            if (provider == Provider.Tinkoff)
                return await TinkoffGetOperationAsync(figi, dateFrom, dateTo);
            else
                throw new NotImplementedException();
        }

        static private async Task<List<TradeOperation>> TinkoffGetOperationAsync(string figi, DateTime dateFrom, DateTime dateTo)
        {
            List<Operation> operations = await GetTinkoffData.GetOperations(figi, dateFrom, dateTo);
            List<TradeOperation> resultOperations = operations.Select
                (x => 
                    new TradeOperation(x.Id, 
                        (MarketDataModules.Operation.OperationStatus)x.Status, 
                        (List<MarketDataModules.Operation.Trade>)x.Trades.Select(y => 
                            new MarketDataModules.Operation.Trade(y.TradeId, y.Date, y.Price, y.Quantity)),
                        new MoneyAmount ((Currency)x.Commission.Currency, x.Commission.Value),
                        (Currency)x.Currency, x.Payment, x.Price, x.Quantity, x.QuantityExecuted, x.Figi,
                        (InstrumentType)x.InstrumentType, x.IsMarginCall, x.Date, 
                        (MarketDataModules.Operation.ExtendedOperationType)x.OperationType)
                ).ToList();
            return resultOperations;
        }

        static async Task<CandlesList> TinkoffCandles(string figi, CandleInterval candleInterval, int candlesCount)
        {
            Tinkoff.Trading.OpenApi.Models.CandleInterval interval = (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval;

            CandleList tinkoffCandles = await GetTinkoffData.GetCandlesTinkoffAsync(figi, interval, candlesCount);
            if (tinkoffCandles == null)
            {
                return null;
            }
            List<CandleStructure> candles =
                new List<CandleStructure>(tinkoffCandles.Candles.Select(x =>
                    new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time, (CandleInterval)x.Interval, x.Figi)).Distinct());

            CandlesList candlesList = new CandlesList(tinkoffCandles.Figi, candleInterval, candles);
            return candlesList;
        }

        static async Task<CandlesList> TinkoffCandles(string figi, CandleInterval candleInterval, DateTime dateTime)
        {
            Tinkoff.Trading.OpenApi.Models.CandleInterval interval = (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval;

            CandleList tinkoffCandles = await GetTinkoffData.GetCandlesTinkoffAsync(figi, interval, dateTime);
            if (tinkoffCandles == null)
            {
                return null;
            }
            List<CandleStructure> candles =
                new List<CandleStructure>(tinkoffCandles.Candles.Select(x =>
                    new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time, (CandleInterval)x.Interval, x.Figi)).Distinct());

            CandlesList candlesList = new CandlesList(tinkoffCandles.Figi, candleInterval, candles);
            return candlesList;
        }

        static async Task<InstrumentList> TinkoffInstrumentList()
        {
            MarketInstrumentList tinkoffStocks = await GetTinkoffData.GetMarketInstrumentList();
            InstrumentList stocks = 
                new InstrumentList(tinkoffStocks.Total, tinkoffStocks.Instruments.Select(x => 
                    new Instrument(x.Figi, x.Ticker, x.Isin, x.MinPriceIncrement, x.Lot, (Currency)x.Currency, x.Name, (InstrumentType)x.Type)).ToList());
            return stocks;
        }

        static private async Task<Orderbook> TinkoffOrderbook(string figi, int depth)
        {
            Log.Information("Tinkoff. Start  get orderbook");
            Tinkoff.Trading.OpenApi.Models.Orderbook tinOrderbook = await GetTinkoffData.GetOrderbookAsync(figi, depth);
            if (tinOrderbook == null)
            {
                return null;
            }
            List<OrderbookEntry> bids = new List<OrderbookEntry>(tinOrderbook.Bids.Select(x => new OrderbookEntry(x.Quantity, x.Price)));
            List<OrderbookEntry> asks = new List<OrderbookEntry>(tinOrderbook.Asks.Select(x => new OrderbookEntry(x.Quantity, x.Price)));
            TradeStatus tradeStatus = (TradeStatus)tinOrderbook.TradeStatus;
            Orderbook orderbook = 
                new Orderbook(tinOrderbook.Depth, bids, asks, tinOrderbook.Figi, tradeStatus, tinOrderbook.MinPriceIncrement, tinOrderbook.FaceValue,
                    tinOrderbook.LastPrice, tinOrderbook.ClosePrice, tinOrderbook.LimitUp, tinOrderbook.LimitDown);
            Log.Information("Tinkoff. Stop  get orderbook");
            return orderbook;
        }
    }
}

