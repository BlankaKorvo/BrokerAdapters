using DataCollector.TinkoffAdapter;
using MarketDataModules;
using MarketDataModules.Candles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataCollector
{
    /// <summary>
    /// Провайдер обмена данных с брокерами
    /// </summary>
    public static class MarketDataProvider
    {
        /// <summary>
        /// Получение определенного кол-во свечей
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="candlesCount"></param> Кол-во свечей в запрашиваемом временном ряду
        /// <param name="providers"></param> Брокер
        /// <returns></returns>
        static public async Task<ICandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, int candlesCount, Provider providers = Provider.Tinkoff)
        {
            switch (providers)
            {
                case Provider.Tinkoff:
                    return await GetTinkoffCandles(figi,candleInterval, candlesCount);
                case Provider.Finam:
                    return null;
            }
            return null;
        }

        /// <summary>
        /// Получение свечей не позже определенной даты
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="dateFrom"></param> Дата, с которой запрашивается временной ряд (округление в зависимости от длины свечи по размеру максимального сета)
        /// <param name="providers"></param> Брокер
        /// <returns></returns>
        static public async Task<ICandlesList> GetCandlesAsync(string figi, CandleInterval candleInterval, DateTime dateFrom, Provider providers = Provider.Tinkoff)
        {
            switch (providers)
            {
                case Provider.Tinkoff:
                    return await GetTinkoffCandles(figi, candleInterval, dateFrom);
                case Provider.Finam:
                    return null;
            }
            return null;
        }

        /// <summary>
        /// Получение свечей у брокера Tinkoff по указанному кол-ву элементов временного ряда
        /// </summary>
        /// <param name="figi"></param> Идентификатор инструмента
        /// <param name="candleInterval"></param> Длина одной свечи
        /// <param name="candlesCount"></param> Кол-во свечей в запрашиваемом временном ряду
        /// <returns></returns>
        static async Task<CandlesList> GetTinkoffCandles(string figi, CandleInterval candleInterval, int candlesCount)
        {
            Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles = await new GetTinkoffCandles(figi, (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval, candlesCount).GetCandlesTinkoffAsync(); ;

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
            Tinkoff.Trading.OpenApi.Models.CandleList tinkoffCandles = await new GetTinkoffCandles(figi, (Tinkoff.Trading.OpenApi.Models.CandleInterval)candleInterval, dateTime).GetCandlesTinkoffAsync(); ;

            CandlesList candlesList = TinkoffCandlesMapper(tinkoffCandles);
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
                new List<CandleStructure>(tinkoffCandles?.Candles.Select(x =>
                    new CandleStructure(x.Open, x.Close, x.High, x.Low, x.Volume, x.Time, (CandleInterval)x.Interval, x.Figi)).Distinct());

            CandlesList candlesList = new CandlesList(tinkoffCandles.Figi, (CandleInterval)tinkoffCandles.Interval, candles);
            return candlesList;
        }
    }


}
