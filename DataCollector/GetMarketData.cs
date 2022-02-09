using DataCollector.TinkoffAdapter;
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


namespace DataCollector
{
    /// <summary>
    /// Провайдер получения данных от брокеров
    /// </summary>
    public static class GetMarketData
    {
        /// <summary>
        /// Получение определенного кол-во свечей
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="candlesCount"></param> Кол-во свечей в запрашиваемом временном ряду
        /// <param name="providers"></param> Брокер
        /// <returns></returns>
        public static async Task<ICandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, int candlesCount, Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffCandles(figi, candleInterval, candlesCount),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Получение свечей не позже определенной даты
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="dateFrom"></param> Дата, с которой запрашивается временной ряд (округление в зависимости от длины свечи по размеру максимального сета)
        /// <param name="providers"></param> Брокер
        /// <returns></returns>
        public static async Task<ICandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, DateTime dateFrom, Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffCandles(figi, candleInterval, dateFrom),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Получение портфолио
        /// </summary>
        /// <param name="provider"></param> Брокер
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<IPortfolio> GetPortfolioAsync(Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffPortfolioAsync(),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Получение инструмента по идентификатору FIGI
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="provider"></param> Брокер
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static public async Task<IInstrument> GetInstrumentByFigiAsync(string figi, Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffInstrumentByFigiAsync(figi),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Получение инструмента по имени(тикеру) инструмента
        /// </summary>
        /// <param name="ticker"></param> Имя инструмента(тикер)
        /// <param name="provider"></param> Брокер
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static public async Task<IInstrument> GetInstrumentByTickerAsync(string ticker, Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffInstrumentByTickerAsync(ticker),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Получение списка всех акций, которым предоставляет доступ брокер
        /// </summary>
        /// <param name="provider"></param> Брокер
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static public async Task<IInstrumentList> GetInstrumentListAsync(Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffInstrumentListAsync(),
                _ => throw new NotImplementedException()
            };

        /// <summary>
        /// Получение стакана
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="depth"></param> Глубина стакана
        /// <param name="provider"></param> Брокер
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        static public async Task<IOrderbook> GetOrderbookAsync(string figi, int depth, Provider provider = Provider.Tinkoff)
            => provider switch
            {
                Provider.Finam => throw new NotImplementedException(),
                Provider.Alor => throw new NotImplementedException(),
                Provider.Tinkoff => await GetTinkoffOrderbookAsync(figi, depth),
                _ => throw new NotImplementedException()
            };



        /// <summary>
        /// Получение инструмента по идентификатору FIGI от tinkoff
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <returns></returns>
        private static async Task<Instrument> GetTinkoffInstrumentByFigiAsync(string figi)
        {
            Tinkoff.Trading.OpenApi.Models.MarketInstrument instrumentT = await new GetTinkoffCurrentInstrument().GetMarketInstrumentByFigi(figi);
            Instrument instrument = new(instrumentT.Figi, instrumentT.Ticker,
                instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
                (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
            return instrument;
        }

        /// <summary>
        /// Получение инструмента по имени(тикеру) инструмента от tinkoff
        /// </summary>
        /// <param name="ticker"></param> Тикер инструмента
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static async Task<Instrument> GetTinkoffInstrumentByTickerAsync(string ticker)
        {
            Tinkoff.Trading.OpenApi.Models.MarketInstrumentList instrumentList = await new GetTinkoffCurrentInstrument().GetMarketInstrumentListByTicker(ticker);
            Tinkoff.Trading.OpenApi.Models.MarketInstrument instrumentT;
            if (instrumentList.Instruments.Count == 1)
            {
                instrumentT = instrumentList.Instruments.FirstOrDefault();
            }
            else
            {
                throw new Exception("При поиске по ticker - кол-во найденных инструментов = " + instrumentList.Instruments.Count);
            }
            Instrument instrument = new(instrumentT.Figi, instrumentT.Ticker,
                instrumentT.Isin, instrumentT.MinPriceIncrement, instrumentT.Lot,
                (Currency)instrumentT.Currency, instrumentT.Name, (InstrumentType)instrumentT.Type);
            return instrument;
        }

        /// <summary>
        /// Получение списка всех акций, которым предоставляет доступ tinkoff
        /// </summary>
        /// <returns></returns>
        static async Task<InstrumentList> GetTinkoffInstrumentListAsync()
        {
            Tinkoff.Trading.OpenApi.Models.MarketInstrumentList tinkoffStocks = await new GetTinkoffInstruments().GetAllAsync();
            InstrumentList stocks =
                new(tinkoffStocks.Total, tinkoffStocks.Instruments.Select(x =>
                    new Instrument(x.Figi, x.Ticker, x.Isin, x.MinPriceIncrement, x.Lot, (Currency)x.Currency, x.Name, (InstrumentType)x.Type)).ToList());
            return stocks;
        }

        /// <summary>
        /// Получение портфолио инструментов Tinkoff
        /// </summary>
        /// <returns></returns>
        private static async Task<Portfolio> GetTinkoffPortfolioAsync()
        {
            Tinkoff.Trading.OpenApi.Models.Portfolio portfolioTinkoff = await new GetTinkoffPortfolio().GetPortfolioAsync();
            var positions = new List<Portfolio.Position> { };
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
                    MoneyAmount ExpectedYield = new(currencyExpectedYield, item.ExpectedYield.Value);

                    Currency currencyAveragePositionPrice = (Currency)item.AveragePositionPrice.Currency;
                    MoneyAmount AveragePositionPrice = new(currencyAveragePositionPrice, item.AveragePositionPrice.Value);
                    Currency currencyAveragePositionPriceNoNkd = (Currency)item.AveragePositionPriceNoNkd.Currency;

                    MoneyAmount AveragePositionPriceNoNkd = new(currencyAveragePositionPriceNoNkd, item.AveragePositionPriceNoNkd.Value);
                    Portfolio.Position position = new(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);


                    positions.Add(position);
                }
                //else
                //{
                //    MoneyAmount ExpectedYield = null;
                //    MoneyAmount AveragePositionPrice = null;
                //    MoneyAmount AveragePositionPriceNoNkd = null;
                //    Portfolio.Position position = new(name, figi, ticker, isin, instrumentType, balance, blocked, ExpectedYield, lots, AveragePositionPrice, AveragePositionPriceNoNkd);
                //    positions.Add(position);
                //}
            }
            Portfolio portfolio = new(positions);
            return portfolio;
        }

        /// <summary>
        /// Получение стакана по инструменту от tinkoff
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="depth"></param> Глубина стакана
        /// <returns></returns>
        static private async Task<Orderbook> GetTinkoffOrderbookAsync(string figi, int depth)
        {
            Tinkoff.Trading.OpenApi.Models.Orderbook tinOrderbook = await new GetTinkoffOrderbook(figi, depth).GetOrderbookAsync();

            List<OrderbookEntry> bids = new(tinOrderbook?.Bids.Select(x => new OrderbookEntry(x.Quantity, x.Price)));
            List<OrderbookEntry> asks = new(tinOrderbook?.Asks.Select(x => new OrderbookEntry(x.Quantity, x.Price)));
            TradeStatus tradeStatus = (TradeStatus)tinOrderbook.TradeStatus;
            Orderbook orderbook =
                new(tinOrderbook.Depth, bids, asks, tinOrderbook.Figi, tradeStatus, tinOrderbook.MinPriceIncrement, tinOrderbook.FaceValue,
                    tinOrderbook.LastPrice, tinOrderbook.ClosePrice, tinOrderbook.LimitUp, tinOrderbook.LimitDown);
            return orderbook;
        }

        /// <summary>
        /// Получение свечей Tinkoff по указанному кол-ву элементов временного ряда
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="candlesCount"></param> Кол-во свечей в запрашиваемом временном ряду
        /// <returns></returns>
        static async Task<CandlesList> GetTinkoffCandles(string figi, CandleInterval candleInterval, int candlesCount)
        {
            Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles = await new GetTinkoffCandles(figi, (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval, candlesCount).GetCandlesTinkoffAsync();
            CandlesList candlesList = TinkoffCandlesMapper(tinkoffCandles);
            return candlesList;
        }

        /// <summary>
        /// Получение свечей у брокера Tinkoff по указанной дате начала временного ряда
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="dateTime"></param> Дата, с которой запрашивается временной ряд (округление в зависимости от длины свечи по размеру максимального сета)
        /// <returns></returns>
        static async Task<CandlesList> GetTinkoffCandles(string figi, CandleInterval candleInterval, DateTime dateTime)
        {
            Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles = await new GetTinkoffCandles(figi, (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval, dateTime).GetCandlesTinkoffAsync();
            CandlesList candlesList = TinkoffCandlesMapper(tinkoffCandles);
            Log.Information($"Return {candlesList.Candles.Count} GetTinkoffCandles");
            return candlesList;
        }

        /// <summary>
        /// Приведение объекта от модели Tinkoff.Trading.OpenApi к MarketDataModules
        /// </summary>
        /// <param name="tinkoffCandles"></param> Tinkoff.Trading.OpenApi.Models.CandleList
        /// <returns></returns>
        static CandlesList TinkoffCandlesMapper(Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles)
        {
            List<CandleStructure> candles =
                new(tinkoffCandles?.Candles.Select(x =>
                    new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time, (CandleInterval)x.Interval, x.Figi)).Distinct());
            CandlesList candlesList = new(tinkoffCandles.Figi, (CandleInterval)tinkoffCandles.Interval, candles);
            return candlesList;
        }
    }
}


