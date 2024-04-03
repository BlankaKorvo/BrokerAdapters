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

namespace DataCollector.AlorAdapterOpenApi

{
    public static class GetAlorData
    {
        //static ComparatorTinkoffCandles comparator = new ComparatorTinkoffCandles();
        //public static List<HistoricCandle> GetCandles(GetCandlesRequest CandlesRequest, int CandleCount)
        //{
        //    List<HistoricCandle> historicCandles = new List<HistoricCandle>() { };
        //    GetCandlesRequest temptCandlesRequest = new() { InstrumentId = CandlesRequest.InstrumentId, Interval = CandlesRequest.Interval, /*From = FullCandlesRequest.From,*/ To = CandlesRequest.To };
        //    try
        //    {
        //        while (historicCandles.Count < CandleCount)
        //        {
        //            var hcandles = GetOneSetCandles(ref temptCandlesRequest);
        //            historicCandles = historicCandles.Union(hcandles, comparator).ToList();
        //            temptCandlesRequest.To = temptCandlesRequest.From;
        //            //temptCandlesRequest.From = FullCandlesRequest.From;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        return null;
        //    }
        //    return historicCandles.OrderBy(x => x.Time).ToList();

        //    List<HistoricCandle> GetOneSetCandles(ref GetCandlesRequest temptCandlesRequest)
        //    {
        //        Log.Debug("Start GetOneSetCandles");
        //        switch (temptCandlesRequest.Interval)
        //        {
        //            case CandleInterval._1Min:
        //                temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-1));
        //                break;
        //            case CandleInterval._5Min:
        //                temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-1));
        //                break;
        //            case CandleInterval._15Min:
        //                temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-1));
        //                break;
        //            case CandleInterval.Hour:
        //                temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddDays(-7));
        //                break;
        //            case CandleInterval.Day:
        //                temptCandlesRequest.From = Timestamp.FromDateTime(temptCandlesRequest.To.ToDateTime().AddYears(-1));
        //                break;
        //            default:
        //                break;
        //        }
        //        Log.Debug("Time periods for candles with figi: {0} from {1} to {2}", temptCandlesRequest.InstrumentId, temptCandlesRequest.From, temptCandlesRequest.InstrumentId);
        //        try
        //        {
        //            var req = temptCandlesRequest;
        //            GetCandlesResponse result = PollyRetrayPolitics.RetryTinkoff().Execute(() => PollyRetrayPolitics.RetryToManyReqTinkoff().Execute(() => GetClient.Grpc.MarketData.GetCandles(req)));
        //            //GetCandlesResponse result = GetClient.Grpc.MarketData.GetCandles(temptCandlesRequest);
        //            List<HistoricCandle> candles = result.Candles.ToList();
        //            Log.Debug($"Return {candles?.Count} candles by figi {temptCandlesRequest.InstrumentId}. Interval = {temptCandlesRequest.Interval}. Date interval = {temptCandlesRequest.From} - {temptCandlesRequest.To}");
        //            return candles;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error(ex.Message);
        //            Log.Error(ex.StackTrace);
        //            Log.Error($"Return null candles by figi {temptCandlesRequest.InstrumentId}. Interval = {temptCandlesRequest.Interval}. Date interval = {temptCandlesRequest.From} - {temptCandlesRequest.To}");
        //            return null;
        //        }
        //    }
        //}
        //public static GetOrderBookResponse GetOrderbook(GetOrderBookRequest getOrderBookRequest)
        //{

        //    try
        //    {
        //        GetOrderBookResponse orderbook = PollyRetrayPolitics.RetryTinkoff().Execute(() => PollyRetrayPolitics.RetryToManyReqTinkoff().Execute(() => GetClient.Grpc.MarketData.GetOrderBook(getOrderBookRequest)));
        //        return orderbook;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        return null;
        //    }
        //}
        //public static PortfolioResponse GetPortfolio(PortfolioRequest portfolioRequest)
        //{


        //}
        //public static PositionsResponse GetPositions(PositionsRequest positionRequest)
        //{
        //    try
        //    {
        //        var positions = PollyRetrayPolitics.RetryAll().Execute(() => GetClient.Grpc.Operations.GetPositions(positionRequest));
        //        Log.Debug($"GetPositions {positionRequest?.AccountId} success return {positionRequest?.AccountId}");
        //        return positions;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        Log.Debug($"GetPositions {positionRequest?.AccountId} return null");
        //        return null;
        //    }
        //}

        //public static AssetsResponse GetAssets(AssetsRequest assetRequest)
        //{
        //    try
        //    {
        //        var assets = PollyRetrayPolitics.RetryAll().Execute(() => GetClient.Grpc.Instruments.GetAssets(assetRequest));
        //        Log.Debug($"GetAssets {assetRequest?.InstrumentType.ToString()} success");
        //        return assets;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        Log.Debug($"GetPositions {assetRequest?.InstrumentType.ToString()} return null");
        //        return null;
        //    }
        //}
        //public static SharesResponse GetShares()
        //{
        //    try
        //    {
        //        var shares = PollyRetrayPolitics.RetryAll().Execute(() => GetClient.Grpc.Instruments.Shares());
        //        Log.Debug($"GetShares success");
        //        return shares;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        Log.Debug($"GetShares return null");
        //        return null;
        //    }
        //}
        //public static BondsResponse GetBonds()
        //{
        //    try
        //    {
        //        var bonds = PollyRetrayPolitics.RetryAll().Execute(() => GetClient.Grpc.Instruments.Bonds());
        //        Log.Debug($"GetBonds success");
        //        return bonds;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        Log.Debug($"GetBonds return null");
        //        return null;
        //    }
        //}
        //public static EtfsResponse GetEtfs()
        //{
        //    try
        //    {
        //        var etfs = PollyRetrayPolitics.RetryAll().Execute(() => GetClient.Grpc.Instruments.Etfs());
        //        Log.Debug($"GetEtfs success");
        //        return etfs;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.Message);
        //        Log.Error(ex.StackTrace);
        //        Log.Debug($"GetEtfs return null");
        //        return null;
        //    }
        //}
    }
}
