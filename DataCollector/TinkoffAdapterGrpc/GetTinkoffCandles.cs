using DataCollector.RetryPolicy;
using Google.Protobuf.WellKnownTypes;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace DataCollector.TinkoffAdapterGrpc
{
    public class GetTinkoffCandles
    {
        public int CandleCount { get; set; }
        public GetCandlesRequest FullCandlesRequest { get; set;  }
        public InvestApiClient Client { get; set; }

        ComparatorTinkoffCandles comparator = new ComparatorTinkoffCandles();


        ///private List<HistoricCandle> historicCandles = new();

        List<HistoricCandle> GetOneSetCandles(ref GetCandlesRequest temptCandlesRequest)
        {
            Log.Debug("Start GetOneSetCandles");
            switch (temptCandlesRequest.Interval)
            {
                case CandleInterval._1Min:
                    temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-1));
                    break;
                case CandleInterval._5Min:
                    temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-1)); 
                    break;
                case CandleInterval._15Min:
                    temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-1)); 
                    break;
                case CandleInterval.Hour:
                    temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-7)); 
                    break;
                case CandleInterval.Day:
                    temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddYears(-1));
                    break;
                default:
                    break;
            }
            Log.Debug("Time periods for candles with figi: {0} from {1} to {2}", temptCandlesRequest.InstrumentId, temptCandlesRequest.From, temptCandlesRequest.InstrumentId);
            try
            {

                GetCandlesResponse result = Client.MarketData.GetCandles(temptCandlesRequest);
                List<HistoricCandle> candles = result.Candles.ToList();
                Log.Debug($"Return {candles?.Count} candles by figi {temptCandlesRequest.InstrumentId}. Interval = {temptCandlesRequest.Interval}. Date interval = {temptCandlesRequest.From} - {temptCandlesRequest.To}");
                return candles;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error($"Return null candles by figi {temptCandlesRequest.InstrumentId}. Interval = {temptCandlesRequest.Interval}. Date interval = {temptCandlesRequest.From} - {temptCandlesRequest.To}");
                return null;
            }
        }

        public List<HistoricCandle> GetSetCandles()
        {
            List<HistoricCandle> historicCandles = new List<HistoricCandle>() { };
            GetCandlesRequest temptCandlesRequest = new() { InstrumentId = FullCandlesRequest.InstrumentId, Interval = FullCandlesRequest.Interval, From = FullCandlesRequest.From, To = FullCandlesRequest.To };
            try
            {
                while (historicCandles.Count < CandleCount)
                {
                    var hcandles = GetOneSetCandles(ref temptCandlesRequest);
                    historicCandles = historicCandles.Union(hcandles, comparator).ToList();
                    temptCandlesRequest.To = temptCandlesRequest.From;
                    temptCandlesRequest.From = FullCandlesRequest.From;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return null;
            }
            return historicCandles.OrderBy(x => x.Time).ToList();
        }            
    }
}
