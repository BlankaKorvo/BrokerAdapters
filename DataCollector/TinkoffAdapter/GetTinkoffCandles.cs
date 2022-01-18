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
    public class GetTinkoffCandles
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

        List<CandlePayload> candlePayloads = new List<CandlePayload>();

        public async Task<CandleList> GetCandlesTinkoffAsync()
        {
            Log.Information("Start GetCandlesTinkoffAsync method. Figi: " + figi);
            DateTime date = DateTime.Now;

            if (interval == CandleInterval.Minute
                || interval == CandleInterval.TwoMinutes
                || interval == CandleInterval.ThreeMinutes
                || interval == CandleInterval.FiveMinutes
                || interval == CandleInterval.TenMinutes
                || interval == CandleInterval.QuarterHour
                || interval == CandleInterval.HalfHour)
                while ((candlePayloads?.Count ?? 0) < candlesCount)
                {
                    List<CandlePayload> candlePayloadsOneIteration = await GetOneSetCandlesAsync(date);
                    candlePayloads = candlePayloads.Union(candlePayloadsOneIteration, new ComparerTinkoffCandles()).ToList();
                    date = date.AddDays(-1);
                }

            //else if (interval == CandleInterval.Hour)
            //    AllCandlePayload = await GetCandlePayloads(candlesCount, figi, interval, date, CandlePayloadEqC, 7);

            //else if (interval == CandleInterval.Day)
            //    AllCandlePayload = await GetCandlePayloads(candlesCount, figi, interval, date, CandlePayloadEqC, 365);

            //else if (interval == CandleInterval.Week)
            //    AllCandlePayload = await GetCandlePayloads(candlesCount, figi, interval, date, CandlePayloadEqC, 730);

            //else if (interval == CandleInterval.Month)
            //    AllCandlePayload = await GetCandlePayloads(candlesCount, figi, interval, date, CandlePayloadEqC, 3650);


            candlePayloads = (from u in candlePayloads orderby u.Time select u).ToList();

            candlePayloads = candlePayloads.Skip((candlePayloads?.Count ?? 0) - candlesCount).ToList();

            CandleList candleList = new CandleList(figi, interval, candlePayloads);

            Log.Information("Stop GetCandlesTinkoffAsync method. Figi: " + figi + ". Return candle list");
            return candleList;
        }

        //private static async Task<List<CandlePayload>> GetCandlePayloads(int candlesCount, string figi, CandleInterval candleInterval, DateTime date, ComparerTinkoffCandlePayloadEquality CandlePayloadEqC, int stepBack)
        //{
        //    List<CandlePayload> result = new List<CandlePayload>();
        //    int countafter = 0;
        //    int notTradeDay = 0;
        //    while (result.Count < candlesCount)
        //    {
        //        result = await GetUnionCandlesAsync(figi, candleInterval, date, result, CandlePayloadEqC);
        //        if (countafter == result.Count && notTradeDay == 7) /// Допустимое кол-во дней неработы биржы по инструменту
        //        {
        //            return null;
        //        }
        //        date = date.AddDays(-stepBack);
        //        countafter = result.Count;
        //        notTradeDay = 0;
        //    }
        //    return result;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CandlePayloadEqC"></param>
        /// <returns></returns>
        //private async Task<List<CandlePayload>> GetUnionCandlesAsync(ComparerTinkoffCandlePayloadEquality CandlePayloadEqC)
        //{
        //    Log.Information("Befor union CandlePayloads contain {0} candles", candlePayloads?.Count);

        //    List<CandlePayload> candleListTemp = await GetOneSetCandlesAsync();
        //    if (candleListTemp == null)
        //    {
        //        return null;
        //    }
        //    candlePayloads = candlePayloads.Union(candleListTemp, CandlePayloadEqC).ToList();
        //    Log.Information("After union CandlePayloads contain {0} candles", candlePayloads?.Count);
        //    return candlePayloads;
        //}

        /// <summary>
        /// Получение максимально возможного кол-во свечей по выбранному интервалу, с учетом ограничений на один запрос от API Tinkoff
        /// </summary>
        /// <returns>Поток Tinkoff CandleList</returns>
        private async Task<List<CandlePayload>> GetOneSetCandlesAsync(DateTime dateTo)
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
    }
}
