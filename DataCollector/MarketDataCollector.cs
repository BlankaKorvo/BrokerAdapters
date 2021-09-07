using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketDataModules;
using Serilog;
using Tinkoff.Trading.OpenApi.Models;
using TinkoffAdapter.Authority;
using TinkoffAdapter.DataHelper;
using CandleInterval = MarketDataModules.CandleInterval;
using Currency = MarketDataModules.Currency;
using InstrumentType = MarketDataModules.InstrumentType;
using Orderbook = MarketDataModules.Orderbook;
using OrderbookEntry = MarketDataModules.OrderbookEntry;
using TradeStatus = MarketDataModules.TradeStatus;
using Portfolio = MarketDataModules.Models.Portfolio.Portfolio;
using MoneyAmount = MarketDataModules.Models.Portfolio.MoneyAmount;


namespace DataCollector
{


    public class MarketDataCollector// : GetTinkoffData // : ICandlesList
    {
        GetTinkoffData getTinkoffData = new GetTinkoffData();

        public async Task<Portfolio> GetPortfolioAsync(Provider provider = Provider.Tinkoff)
        {
            if (provider == Provider.Tinkoff)
            {
                Tinkoff.Trading.OpenApi.Models.Portfolio portfolioTinkoff = await getTinkoffData.GetPortfolioAsync();
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
                    Currency currencyExpectedYield = (Currency)item.ExpectedYield.Currency;
                    MoneyAmount ExpectedYield = new MoneyAmount (currencyExpectedYield, item.ExpectedYield.Value);
                    int lots = item.Lots;
                    Currency currencyAveragePositionPrice = (Currency)item.AveragePositionPrice.Currency;
                    MoneyAmount AveragePositionPrice = new MoneyAmount(currencyAveragePositionPrice, item.AveragePositionPrice.Value);
                    Currency currencyAveragePositionPriceNoNkd = (Currency)item.AveragePositionPriceNoNkd.Currency;
                    MoneyAmount AveragePositionPriceNoNkd = new MoneyAmount(currencyAveragePositionPriceNoNkd, item.AveragePositionPriceNoNkd.Value);

                    Portfolio.Position position = new Portfolio.Position(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);

                    positions.Add(position);
                }
                Portfolio portfolio = new Portfolio(positions);
                return portfolio;
            }
            else
                throw new NotImplementedException();
        }

        public async Task<Instrument> GetInstrumentByFigi(string figi, Provider provider)
        {
            MarketInstrument instrumentT = await getTinkoffData.GetMarketInstrumentByFigi(figi);
            Instrument instrument = new Instrument(instrumentT.Figi, instrumentT.Ticker,
                instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
                (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
            return instrument;
        }

        public async Task<Instrument> GetInstrumentByTickerAsync(string ticker, Provider provider)
        {
            if (provider == Provider.Tinkoff)
            {
                MarketInstrument instrumentT = null;
                MarketInstrumentList instrumentList = await getTinkoffData.GetMarketInstrumentListByTicker(ticker);
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
        public async Task<InstrumentList> GetInstrumentListAsync(Provider provider)
        {
            if (provider == Provider.Tinkoff)
                return await TinkoffInstrumentList();
            else
                throw new NotImplementedException();

        }

        public async Task<Orderbook> GetOrderbookAsync(string figi, Provider provider, int depth = 20)
        {
            if (provider == Provider.Tinkoff)
                return await TinkoffOrderbook(figi, depth);
            else
                throw new NotImplementedException();
        }

        //Получение списка candlelist из списка инструментов
        public async Task<List<CandlesList>> GetListCandlesByInstrumentsAsync(InstrumentList instrumentList, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
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
        async Task<CandlesList> TinkoffCandles(string figi, CandleInterval candleInterval, int candlesCount)
        {
            Tinkoff.Trading.OpenApi.Models.CandleInterval interval = (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval;

            CandleList tinkoffCandles = await getTinkoffData.GetCandlesTinkoffAsync(figi, interval, candlesCount);
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

        async Task<CandlesList> TinkoffCandles(string figi, CandleInterval candleInterval, DateTime dateTime)
        {
            Tinkoff.Trading.OpenApi.Models.CandleInterval interval = (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval;

            CandleList tinkoffCandles = await getTinkoffData.GetCandlesTinkoffAsync(figi, interval, dateTime);
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

        async Task<InstrumentList> TinkoffInstrumentList()
        {
            MarketInstrumentList tinkoffStocks = await RetryPolicy.Model.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.MarketStocksAsync());
            InstrumentList stocks = 
                new InstrumentList(tinkoffStocks.Total, tinkoffStocks.Instruments.Select(x => 
                    new Instrument(x.Figi, x.Ticker, x.Isin, x.MinPriceIncrement, x.Lot, (Currency)x.Currency, x.Name, (InstrumentType)x.Type)).ToList());
            return stocks;
        }

        private async Task<Orderbook> TinkoffOrderbook(string figi, int depth)
        {
            Log.Information("Tinkoff. Start  get orderbook");
            Tinkoff.Trading.OpenApi.Models.Orderbook tinOrderbook = await getTinkoffData.GetOrderbookAsync(figi, depth);
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

        public async Task<CandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
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

        public async Task<CandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, DateTime dateTime, Provider providers = Provider.Tinkoff)
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


    }
}

