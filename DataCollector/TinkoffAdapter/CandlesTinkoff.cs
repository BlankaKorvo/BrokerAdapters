using DataCollector.RetryPolicy;
using DataCollector.TinkoffAdapter.Authority;
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
    public class CandlesTinkoff : CandleList
    {
        public CandlesTinkoff(string figi, CandleInterval interval, int candlesCount) : base(figi, interval, GetCandlesTinkoffAsync(figi, interval, candlesCount))
        {
        }  
        private static List<CandlePayload> GetCandlesTinkoffAsync(string figi, CandleInterval candleInterval, int candlesCount)
        {
            Log.Information("Start GetCandlesTinkoffAsync method. Figi: " + figi);
            DateTime date = DateTime.Now;
            List<CandlePayload> AllCandlePayloadTemp = new List<CandlePayload>();

            ComparerTinkoffCandlePayloadEquality CandlePayloadEqC = new ComparerTinkoffCandlePayloadEquality();

            if (candleInterval == CandleInterval.Minute
                || candleInterval == CandleInterval.TwoMinutes
                || candleInterval == CandleInterval.ThreeMinutes
                || candleInterval == CandleInterval.FiveMinutes
                || candleInterval == CandleInterval.TenMinutes
                || candleInterval == CandleInterval.QuarterHour
                || candleInterval == CandleInterval.HalfHour)
                AllCandlePayloadTemp = GetCandlePayloads(candlesCount, figi, candleInterval, date, CandlePayloadEqC, 1);

            else if (candleInterval == CandleInterval.Hour)
                AllCandlePayloadTemp = GetCandlePayloads(candlesCount, figi, candleInterval, date, CandlePayloadEqC, 7);

            else if (candleInterval == CandleInterval.Day)
                AllCandlePayloadTemp = GetCandlePayloads(candlesCount, figi, candleInterval, date, CandlePayloadEqC, 365);

            else if (candleInterval == CandleInterval.Week)
                AllCandlePayloadTemp = GetCandlePayloads(candlesCount, figi, candleInterval, date, CandlePayloadEqC, 730);

            else if (candleInterval == CandleInterval.Month)
                AllCandlePayloadTemp = GetCandlePayloads(candlesCount, figi, candleInterval, date, CandlePayloadEqC, 3650);


            List<CandlePayload> candlePayloadBefor = (from u in AllCandlePayloadTemp
                                                      orderby u.Time
                                                      select u).ToList();

            int candlesCountNow = candlePayloadBefor.Count();
            List<CandlePayload> candlePayload = candlePayloadBefor.Skip(candlesCountNow - candlesCount).ToList();

            //CandleList candleList = new CandleList(figi, candleInterval, candlePayload);

            Log.Information("Stop GetCandlesTinkoffAsync method. Figi: " + figi + ". Return candle list");
            return candlePayload;
        }

        private static List<CandlePayload> GetCandlePayloads(int candlesCount, string figi, CandleInterval candleInterval, DateTime date, ComparerTinkoffCandlePayloadEquality CandlePayloadEqC, int stepBack)
        {
            List<CandlePayload> result = new List<CandlePayload>();
            int countafter = 0;
            int notTradeDay = 0;
            while (result.Count < candlesCount)
            {
                result = GetUnionCandlesAsync(figi, candleInterval, date, result, CandlePayloadEqC);
                if (countafter == result.Count && notTradeDay == 7) /// Допустимое кол-во дней неработы биржы по инструменту
                {
                    return null;
                }
                date = date.AddDays(-stepBack);
                countafter = result.Count;
                notTradeDay = 0;
            }
            return result;
        }

        private static List<CandlePayload> GetUnionCandlesAsync(string figi, CandleInterval candleInterval, DateTime date, List<CandlePayload> AllCandlePayloadTemp, ComparerTinkoffCandlePayloadEquality CandlePayloadEqC)
        {
            Log.Information("Start GetUnionCandles. Figi: " + figi);
            Log.Information("Count geting candles = " + AllCandlePayloadTemp.Count);

            CandleList candleListTemp = GetOneSetCandlesAsync(figi, candleInterval, date);
            if (candleListTemp == null)
            {
                return null;
            }
            Log.Information(candleListTemp?.Figi + " GetCandleByFigi: " + candleListTemp?.Candles.Count + " candles");

            AllCandlePayloadTemp = AllCandlePayloadTemp.Union(candleListTemp?.Candles, CandlePayloadEqC).ToList();

            Log.Information("GetUnionCandles return: " + AllCandlePayloadTemp.Count + " count candles");
            Log.Information("Stop GetUnionCandles. Figi: " + figi);
            return AllCandlePayloadTemp;
        }

        private static CandleList GetOneSetCandlesAsync(string figi, CandleInterval interval, DateTime to)
        {

            Log.Information("Start GetCandleByFigiAsync method whith figi: " + figi);
            //DateTime to = DateTime.Now;
            to = to.AddMinutes(2);
            DateTime from = to;
            switch (interval)
            {
                case CandleInterval.Minute:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.TwoMinutes:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.ThreeMinutes:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.FiveMinutes:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.TenMinutes:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.QuarterHour:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.HalfHour:
                    from = to.AddDays(-1);
                    break;
                case CandleInterval.Hour:
                    from = to.AddDays(-7);
                    break;
                case CandleInterval.Day:
                    from = to.AddYears(-1);
                    break;
                case CandleInterval.Week:
                    from = to.AddDays(-720);
                    break;
                case CandleInterval.Month:
                    from = to.AddYears(-10);
                    break;
            }
            Log.Information("Time periods for candles with figi: " + figi + " = " + from + " - " + to);
            try
            {
                CandleList candle = PollyRetray.Retry().ExecuteAsync(async () => await PollyRetray.RetryToManyReq().ExecuteAsync(async () => await Auth.Context.MarketCandlesAsync(figi, from, to, interval))).GetAwaiter().GetResult();
                Log.Information("Return " + candle.Candles.Count + " candles by figi: " + figi + " with " + interval + " lenth. Date: " + from + " " + to);
                Log.Information("Stop GetCandleByFigiAsync method whith figi: " + figi);
                return candle;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Information("Stop GetCandleByFigiAsync method. Return null");
                return null;
            }
        }
    }
}
