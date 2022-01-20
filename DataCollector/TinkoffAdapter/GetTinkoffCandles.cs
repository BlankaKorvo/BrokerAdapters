using DataCollector.RetryPolicy;
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
    internal class GetTinkoffCandles
    {
        /// <summary>
        /// Получение опредленного кол-ва свечей.
        /// </summary>
        /// <param name="figi"></param> FIGI
        /// <param name="interval"></param> Интервал свечей
        /// <param name="candlesCount"></param> Кол-во свечей, которые нужно получить.
        public GetTinkoffCandles(string figi, CandleInterval interval, int candlesCount)
        {
            this.figi = figi;
            this.interval = interval;
            this.candlesCount = candlesCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="figi"></param>
        /// <param name="interval"></param>
        /// <param name="dateFrom"></param>
        public GetTinkoffCandles(string figi, CandleInterval interval, DateTime dateFrom)
        {
            this.figi = figi;
            this.interval = interval;
            this.dateFrom = dateFrom;
        }

        string figi;
        CandleInterval interval;
        int candlesCount;
        DateTime dateFrom;

        /// <summary>
        /// Заполняемый список в методе GetCandlesTinkoffAsync
        /// </summary>
        List<CandlePayload> candlePayloads = new List<CandlePayload>();


        /// <summary>
        /// Получение свечей по инструменту. Вводные задаются в конструкторе.
        /// </summary>
        /// <returns></returns>
        internal async Task<CandleList> GetCandlesTinkoffAsync()
        {
            DateTime date = DateTime.Now;

            ///Максимально допустимое кол-во холостых циклов while подряд.
            int notTradeDayLimit = 1;

            if (interval == CandleInterval.Minute
                || interval == CandleInterval.TwoMinutes
                || interval == CandleInterval.ThreeMinutes
                || interval == CandleInterval.FiveMinutes
                || interval == CandleInterval.TenMinutes
                || interval == CandleInterval.QuarterHour
                || interval == CandleInterval.HalfHour)
            {
                ///Максимально допустимый интервал за один запрос в Tinkoff
                int daysInterval = 1;
                notTradeDayLimit = 7;

                await GetPayload(date, daysInterval, notTradeDayLimit);

            }
            else if (interval == CandleInterval.Hour)
            {
                int daysInterval = 7;
                await GetPayload(date, daysInterval, notTradeDayLimit);
            }
            else if (interval == CandleInterval.Day)
            {
                int daysInterval = 365;
                await GetPayload(date, daysInterval, notTradeDayLimit);
            }
            else if (interval == CandleInterval.Week)
            {
                int daysInterval = 730;
                await GetPayload(date, daysInterval, notTradeDayLimit);
            }
            else if (interval == CandleInterval.Month)
            {
                int daysInterval = 3650;
                await GetPayload(date, daysInterval, notTradeDayLimit);
            }
            candlePayloads = (from u in candlePayloads orderby u.Time select u).ToList();

            if (candlesCount > 0)
            {
                candlePayloads = candlePayloads.Skip((candlePayloads?.Count ?? 0) - candlesCount).ToList();
            }
            CandleList candleList = new CandleList(figi, interval, candlePayloads);
            
            return candleList;
        }


        /// <summary>
        /// Набор свечей максимально возможными партиями за один запрос к API до нужного кол-ва.
        /// </summary>
        /// <param name="date"></param>Дата, до которой будет происходить набор. 
        /// <param name="stepInDays"></param> За какое кол-во дней можно получить максимально возможное кол-во свечей. Ограниечение API tinkoff
        /// <param name="emptyIterationLimit"></param>Максимально допустимое кол-во холостых циклов while подряд.
        /// <returns></returns>
        async Task GetPayload(DateTime date, int stepInDays, int emptyIterationLimit)
        {
            int emptyIteration = default;
            while(
                    (candlesCount != default && (candlePayloads?.Count ?? 0) < candlesCount)
                    ||
                    (dateFrom != default && date.CompareTo(dateFrom) >= 0)
                 )
            {
                List<CandlePayload> candlePayloadsOneIteration = await GetOneSetCandlesAsync(date);
                StopWhile(emptyIterationLimit, emptyIteration, candlePayloadsOneIteration?.Count ?? 0);

                candlePayloads = candlePayloads.Union(candlePayloadsOneIteration, new ComparatorTinkoffCandles()).ToList();
                Log.Information("candlePayloads count = {0}", candlePayloads?.Count ?? 0);

                date = date.AddDays(-stepInDays);
            }
        }

        /// <summary>
        /// Проверка для выхода из цикла while. Если <emptyIterationLimit> циклов нет результата - выход из цикла.
        /// </summary>
        /// <param name="emptyIterationLimit"></param>Максимально допустимое кол-во холостых операций.
        /// <param name="emptyIteration"></param>Кол-во выполненых хлостых операций в цикле.
        /// <param name="candlePayloadsOneIterationCount"></param> Кол-во свечей полученных в последний цикл.
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        void StopWhile(int emptyIterationLimit, int emptyIteration, int candlePayloadsOneIterationCount)
        {
            emptyIteration = candlePayloadsOneIterationCount > 0 ? default : ++emptyIteration;

            if (emptyIteration >= emptyIterationLimit)           
                throw new Exception("No more candles. Reduce the number of candles in the request");
        }


        /// <summary>
        /// Получение максимально возможного сета свечей за один запрос, с учетом ограничений tinkoff API.
        /// </summary>
        /// <param name="dateTo"></param> Дата до которой нужно получить свечи.
        /// <returns></returns>
        async Task<List<CandlePayload>> GetOneSetCandlesAsync(DateTime dateTo)
        {
            try
            {
                DateTime from;
                switch (interval)
                {
                    case CandleInterval.Minute:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.TwoMinutes:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.ThreeMinutes:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.FiveMinutes:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.TenMinutes:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.QuarterHour:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.HalfHour:
                        from = dateTo.AddDays(-1);
                        break;
                    case CandleInterval.Hour:
                        from = dateTo.AddDays(-7);
                        break;
                    case CandleInterval.Day:
                        from = dateTo.AddYears(-1);
                        break;
                    case CandleInterval.Week:
                        from = dateTo.AddYears(-2);
                        break;
                    case CandleInterval.Month:
                        from = dateTo.AddYears(-10);
                        break;
                    default:
                        from = default;
                        throw new ArgumentOutOfRangeException();
                }
                Log.Information("Time periods for candles with figi: {0} from {1} to {2}", figi, from, dateTo);

                CandleList candle = await PollyRetrayPolitics.Retry().ExecuteAsync
                    (async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync
                    (async () => await Authority.Authorisation.Context.MarketCandlesAsync(figi, from, dateTo, interval)));

                Log.Information("Return {0} candles by figi {1}. Interval = {2}. Date interval = {3} - {4}", candle.Candles.Count, figi, interval, from, dateTo);
                return candle.Candles;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Information("Stop GetOneSetCandlesAsync with error. Return null");
                return null;
            }
        }
    }
}
