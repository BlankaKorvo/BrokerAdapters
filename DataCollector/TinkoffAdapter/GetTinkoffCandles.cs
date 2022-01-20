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
        public GetTinkoffCandles(string figi, CandleInterval interval, int candlesCount)
        {
            this.figi = figi;
            this.interval = interval;
            this.candlesCount = candlesCount;
        }
        
        string figi;
        CandleInterval interval;
        int candlesCount;

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

            candlePayloads = candlePayloads.Skip((candlePayloads?.Count ?? 0) - candlesCount).ToList();

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
            while ((candlePayloads?.Count ?? 0) < candlesCount)
            {                
                int counterPayload = (candlePayloads?.Count ?? 0);

                List<CandlePayload> candlePayloadsOneIteration = await GetOneSetCandlesAsync(date);
                candlePayloads = candlePayloads.Union(candlePayloadsOneIteration, new ComparerTinkoffCandles()).ToList();
                Log.Information("candlePayloads count = {0}", candlePayloads?.Count ?? 0);

                if ((candlePayloads?.Count ?? 0) != counterPayload)
                {
                    emptyIteration = default;
                }
                else
                {
                    emptyIteration++;
                }

                StopWhile(emptyIteration, emptyIterationLimit);

                date = date.AddDays(-stepInDays);
            }
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
                    (async () => await Authority.Auth.Context.MarketCandlesAsync(figi, from, dateTo, interval)));

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

        /// <summary>
        /// Проверка для выхода из цикла while. Если emptyIterationLimit циклов нет результата - выход из цикла.
        /// </summary>
        /// <param name="emptyIteration"></param>Кол-во выполненых хлостых операций в цикле.
        /// <param name="emptyIterationLimit"></param>Максимально допустимое кол-во холостых операций.
        /// <exception cref="Exception"></exception>
        void StopWhile(int emptyIteration, int emptyIterationLimit)
        {
            if (emptyIteration > emptyIterationLimit)
            {
                throw new Exception("No more candles. Reduce the number of candles in the request");
            }
        }

    }
}
