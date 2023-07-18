using Serilog;
using System.Collections.Generic;
using Tinkoff.InvestApi.V1;
using Tinkoff.Trading.OpenApi.Models;

namespace DataCollector.TinkoffAdapterLegacy
{
    class TinkoffModelComparators : IEqualityComparer<CandlePayload>, IEqualityComparer<HistoricCandle>
    {
        public bool Equals(HistoricCandle c1, HistoricCandle c2)
        {
            Log.Debug("Start Equals method");
            if (c1.Time == c2.Time)
            {
                Log.Debug("{0} {1} candle = {2} {3} candle", c1.Time, c2.Time);
                return true;
            }
            else
            {
                Log.Debug("{0} {1} candle != {2} {3} candle", c1.Time, c2.Time);
                return false;
            }
        }

        public int GetHashCode(HistoricCandle c)
        {
            string hCode = (c.High * c.Low * c.Open * c.Close).ToString() + c.Volume.ToString() + c.Time.ToString();
            return hCode.GetHashCode();
        }

        public bool Equals(CandlePayload c1, CandlePayload c2)
        {
            Log.Debug("Start Equals method");
            if (c1.Time == c2.Time)
            {
                Log.Debug("{0} {1} candle = {2} {3} candle", c1.Figi, c1.Time, c2.Figi, c2.Time);
                return true;
            }
            else
            {
                Log.Debug("{0} {1} candle != {2} {3} candle", c1.Figi, c1.Time, c2.Figi, c2.Time);
                return false;
            }
        }

        public int GetHashCode(CandlePayload c)
        {
            string hCode = (c.High * c.Low * c.Open).ToString() + c.Figi + c.Interval.ToString() + c.Time.ToString();
            return hCode.GetHashCode();
        }
    }

    //class ComparatorTinkoffCandles : IEqualityComparer<HistoricCandle>
    //{
    //    public bool Equals(HistoricCandle c1, HistoricCandle c2)
    //    {
    //        Log.Debug("Start Equals method");
    //        if (c1.Time == c2.Time)
    //        {
    //            Log.Debug("{0} {1} candle = {2} {3} candle", c1.Time, c2.Time);
    //            return true;
    //        }
    //        else
    //        {
    //            Log.Debug("{0} {1} candle != {2} {3} candle", c1.Time, c2.Time);
    //            return false;
    //        }
    //    }

    //    public int GetHashCode(HistoricCandle c)
    //    {
    //        string hCode = (c.High * c.Low * c.Open * c.Close).ToString() + c.Volume.ToString() + c.Time.ToString();
    //        return hCode.GetHashCode();
    //    }
    //}
}
