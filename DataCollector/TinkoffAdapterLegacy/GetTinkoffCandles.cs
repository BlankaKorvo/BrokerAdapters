using DataCollector.RetryPolicy;
using Google.Protobuf.WellKnownTypes;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
//using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.InvestApi.V1;

namespace DataCollector.TinkoffAdapterLegacy
{
    public class GetTinkoffCandles
    {
        private readonly string figi;
        private readonly Tinkoff.Trading.OpenApi.Models.CandleInterval intervalLegasy;
        private readonly CandleInterval candleInterval;
        private readonly int candlesCount;
        private readonly DateTime dateFrom;
        /// <summary>
        /// Заполняемый список в методе GetCandlesTinkoffAsync
        /// </summary>
        private List<Tinkoff.Trading.OpenApi.Models.CandlePayload> candlePayloads = new();
        private List<HistoricCandle> historicCandles = new();

        /// <summary>
        /// Получение опредленного кол-ва свечей.
        /// </summary>
        /// <param name="figi"></param> FIGI
        /// <param name="intervalLegasy"></param> Интервал свечей
        /// <param name="candlesCount"></param> Кол-во свечей, которые нужно получить.
        public GetTinkoffCandles(string figi, Tinkoff.Trading.OpenApi.Models.CandleInterval intervalLegasy, int candlesCount)
        {
            this.figi = figi;
            this.intervalLegasy = intervalLegasy;
            this.candlesCount = candlesCount;
        }

        public GetTinkoffCandles(string figi, CandleInterval candleInterval, int candlesCount)
        {
            this.figi = figi;
            this.candleInterval = candleInterval;
            this.candlesCount = candlesCount;
        }

        /// <summary>
        /// Получение свечей не позже указанной даты.
        /// </summary>
        /// <param name="figi"></param>
        /// <param name="intervalLegasy"></param>
        /// <param name="dateFrom"></param>
        public GetTinkoffCandles(string figi, Tinkoff.Trading.OpenApi.Models.CandleInterval intervalLegasy, DateTime dateFrom)
        {
            this.figi = figi;
            this.intervalLegasy = intervalLegasy;
            this.dateFrom = dateFrom;
        }

        public GetTinkoffCandles(string figi, CandleInterval candleInterval, DateTime dateFrom)
        {
            this.figi = figi;
            this.candleInterval = candleInterval;
            this.dateFrom = dateFrom;
        }

        public async Task<Tinkoff.Trading.OpenApi.Models.CandleList> GetCandlesTinkoffAsync()
        {
            DateTime date = DateTime.UtcNow;

            ///Максимально допустимое кол-во холостых циклов while подряд.
            int notTradeDayLimit = 1;
            ///Кол-во дней 
            TimeSpan timeSpan = default;

            if (
                candleInterval == CandleInterval._1Min ||
                candleInterval == CandleInterval._5Min ||
                candleInterval == CandleInterval._15Min
                )
            {
                ///Максимально допустимый интервал за один запрос в Tinkoff
                timeSpan = TimeSpan.FromHours(23);
                notTradeDayLimit = 7;
            }
            else if (candleInterval == CandleInterval.Hour)
            {
                timeSpan = TimeSpan.FromDays(6);
            }
            else if (candleInterval == CandleInterval.Day)
            {
                timeSpan = TimeSpan.FromDays(364);
            }

            await GetPayloadLegasy(date, timeSpan, notTradeDayLimit);

            candlePayloads = (from u in candlePayloads orderby u.Time select u).ToList();

            candlePayloads = candlesCount > 0 ? candlePayloads.Skip((candlePayloads?.Count ?? 0) - candlesCount).ToList() : candlePayloads;

            Tinkoff.Trading.OpenApi.Models.CandleList candleList = new(figi, intervalLegasy, candlePayloads);

            return candleList;
        }

            /// <summary>
            /// Получение свечей по инструменту. Вводные задаются в конструкторе.
            /// </summary>
            /// <returns></returns>
        public async Task<Tinkoff.Trading.OpenApi.Models.CandleList> GetCandlesTinkoffLegasyAsync()
        {
            DateTime date = DateTime.Now;

            ///Максимально допустимое кол-во холостых циклов while подряд.
            int notTradeDayLimit = 1;
            ///Кол-во дней 
            TimeSpan timeSpan = default;

            if (intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.Minute
                || intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.TwoMinutes
                || intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.ThreeMinutes
                || intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.FiveMinutes
                || intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.TenMinutes
                || intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.QuarterHour
                || intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.HalfHour)
            {
                ///Максимально допустимый интервал за один запрос в Tinkoff
                timeSpan = TimeSpan.FromHours(23);
                notTradeDayLimit = 7;
            }
            else if (intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.Hour)
            {
                timeSpan = TimeSpan.FromDays(6);
            }
            else if (intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.Day)
            {
                timeSpan = TimeSpan.FromDays(364);
            }
            else if (intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.Week)
            {
                timeSpan = TimeSpan.FromDays(720);
            }
            else if (intervalLegasy == Tinkoff.Trading.OpenApi.Models.CandleInterval.Month)
            {
                timeSpan = TimeSpan.FromDays(3600);
            }

            await GetPayloadLegasy(date, timeSpan, notTradeDayLimit);

            candlePayloads = (from u in candlePayloads orderby u.Time select u).ToList();

            candlePayloads = candlesCount > 0 ? candlePayloads.Skip((candlePayloads?.Count ?? 0) - candlesCount).ToList() : candlePayloads;

            Tinkoff.Trading.OpenApi.Models.CandleList candleList = new(figi, intervalLegasy, candlePayloads);
            
            return candleList;
        }

        /// <summary>
        /// Набор свечей максимально возможными партиями за один запрос к API до нужного кол-ва.
        /// </summary>
        /// <param name="date"></param>Дата, до которой будет происходить набор. 
        /// <param name="timeSpan"></param> За какое кол-во дней можно получить максимально возможное кол-во свечей. Ограниечение API tinkoff
        /// <param name="emptyIterationLimit"></param>Максимально допустимое кол-во холостых циклов while подряд.
        /// <returns></returns>
        async Task GetPayloadLegasy(DateTime date, TimeSpan timeSpan, int emptyIterationLimit)
        {
            int emptyIteration = default;

            while(
                    (candlesCount != default && (candlePayloads?.Count ?? 0) < candlesCount)
                    ||
                    (dateFrom != default && date.CompareTo(dateFrom - timeSpan) > 0)
                 )
            {
                List<Tinkoff.Trading.OpenApi.Models.CandlePayload> candlePayloadsOneIteration = await GetOneSetCandlesLegasyAsync(date);
                StopWhile(emptyIterationLimit, emptyIteration, candlePayloadsOneIteration?.Count ?? 0);
                if (candlePayloadsOneIteration == null || candlePayloadsOneIteration.Count == 0)
                {
                    continue;
                }

                candlePayloads = candlePayloads.Union(candlePayloadsOneIteration, new ComparatorTinkoffCandlesLegasy()).ToList(); // Дефолтный компаратор НЕ РАБОТАЕТ c классами тинькофф!!!
                Log.Debug("candlePayloads count = {0}", candlePayloads?.Count ?? 0);

                date -= timeSpan;
            }
        }


        async Task GetPayload(DateTime date, TimeSpan timeSpan, int emptyIterationLimit)
        {
            int emptyIteration = default;

            while (
                    (candlesCount != default && (historicCandles?.Count ?? 0) < candlesCount)
                    ||
                    (dateFrom != default && date.CompareTo(dateFrom - timeSpan) > 0)
                 )
            {
                List<HistoricCandle> candlePayloadsOneIteration = await GetOneSetCandlesAsync(date);
                StopWhile(emptyIterationLimit, emptyIteration, candlePayloadsOneIteration?.Count ?? 0);
                if (candlePayloadsOneIteration == null || candlePayloadsOneIteration.Count == 0)
                {
                    continue;
                }

                historicCandles = historicCandles.Union(candlePayloadsOneIteration, new ComparatorTinkoffCandles()).ToList(); // Дефолтный компаратор НЕ РАБОТАЕТ c классами тинькофф!!!
                Log.Debug("candlePayloads count = {0}", candlePayloads?.Count ?? 0);

                date -= timeSpan;
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
        static void StopWhile(int emptyIterationLimit, int emptyIteration, int candlePayloadsOneIterationCount)
        {
            emptyIteration = candlePayloadsOneIterationCount > 0 ? default : ++emptyIteration;

            if (emptyIteration >= emptyIterationLimit)           
                throw new Exception("No more candles. Reduce the number of candles in the request");
        }

        async Task<List<HistoricCandle>> GetOneSetCandlesAsync(DateTime dateTo)
        {
            Log.Debug("Start GetOneSetCandlesAsync");
            DateTime from = default;
            switch (candleInterval)
            {
                case CandleInterval._1Min:
                    from = dateTo.AddDays(-1).AddMinutes(1);
                    break;
                case CandleInterval._5Min:
                    from = dateTo.AddDays(-1).AddMinutes(5);
                    break;
                case CandleInterval._15Min:
                    from = dateTo.AddDays(-1).AddMinutes(15);
                    break;
                case CandleInterval.Hour:
                    from = dateTo.AddDays(-7).AddHours(1);
                    break;
                case CandleInterval.Day:
                    from = dateTo.AddYears(-1).AddDays(1);
                    break;
                default:
                    break;
            }
            Log.Debug("Time periods for candles with figi: {0} from {1} to {2}", figi, from, dateTo);
            try
            {
                string token = File.ReadAllLines("toksann.dll")[0].Trim();
                InvestApiClient client = InvestApiClientFactory.Create(token);
                GetCandlesRequest getCandlesRequest = new GetCandlesRequest() { Figi = figi, From = Timestamp.FromDateTime(from), Interval = candleInterval, To = Timestamp.FromDateTime(dateTo) };
                GetCandlesResponse result = client.MarketData.GetCandles(getCandlesRequest);
                List<HistoricCandle> candles = result.Candles.ToList();
                Log.Debug($"Return {candles?.Count} candles by figi {figi}. Interval = {intervalLegasy}. Date interval = {from} - {dateTo}");
                return candles;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error($"Return null candles by figi {figi}. Interval = {intervalLegasy}. Date interval = {from} - {dateTo}");
                return null;
            }
        }



            /// <summary>
            /// Получение максимально возможного сета свечей за один запрос, с учетом ограничений tinkoff API.
            /// </summary>
            /// <param name="dateTo"></param> Дата до которой нужно получить свечи.
            /// <returns></returns>
        async Task<List<Tinkoff.Trading.OpenApi.Models.CandlePayload>> GetOneSetCandlesLegasyAsync(DateTime dateTo)
        {
            Log.Debug("Start GetOneSetCandlesAsync");
            DateTime from = default;
            switch (intervalLegasy)
            {
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.Minute:
                    from = dateTo.AddDays(-1).AddMinutes(1);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.TwoMinutes:
                    from = dateTo.AddDays(-1).AddMinutes(2);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.ThreeMinutes:
                    from = dateTo.AddDays(-1).AddMinutes(3);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.FiveMinutes:
                    from = dateTo.AddDays(-1).AddMinutes(5);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.TenMinutes:
                    from = dateTo.AddDays(-1).AddMinutes(10);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.QuarterHour:
                    from = dateTo.AddDays(-1).AddMinutes(15);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.HalfHour:
                    from = dateTo.AddDays(-1).AddMinutes(30);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.Hour:
                    from = dateTo.AddDays(-7).AddHours(1);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.Day:
                    from = dateTo.AddYears(-1).AddDays(1);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.Week:
                    from = dateTo.AddYears(-2).AddDays(7);
                    break;
                case Tinkoff.Trading.OpenApi.Models.CandleInterval.Month:
                    from = dateTo.AddYears(-10).AddMonths(1);
                    break;
                default:
                    break;
            }
            Log.Debug("Time periods for candles with figi: {0} from {1} to {2}", figi, from, dateTo);
            try
            {
                Tinkoff.Trading.OpenApi.Models.CandleList candle = await PollyRetrayPolitics.Retry().ExecuteAsync
                    (async () => await PollyRetrayPolitics.RetryToManyReq().ExecuteAsync
                    (async () => await Authorisation.Context.MarketCandlesAsync(figi, from, dateTo, intervalLegasy)));
                Log.Debug($"Return {candle?.Candles.Count} candles by figi {figi}. Interval = {intervalLegasy}. Date interval = {from} - {dateTo}");
                return candle.Candles;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error($"Return null candles by figi {figi}. Interval = {intervalLegasy}. Date interval = {from} - {dateTo}");
                return null;
            }
        }
    }
}
