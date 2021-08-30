using MarketDataModules;
using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinkoffData;
using TradingAlgorithms.IndicatorSignals.Helpers;
using static MarketDataModules.CandleStructure;

namespace TradingAlgorithms.IndicatorSignals
{
    public partial class Signal : IndicatorSignalsHelper
    {
        internal bool StochWithObvDiveregentLongSignal(CandlesList candleList, decimal deltaPrice, int CloseCount = 10, int volumeCount = 10, int sctochCount = 10, int obvCount = 10)
        {
            Log.Information("Start StochDiveregentSignal LongSignal. Figi: " + candleList.Figi);
            List<StochResult> stoch = Mapper.StochData(candleList, deltaPrice, stochLookbackPeriod, stochSignalPeriod, stochSmoothPeriod);
            List<ObvResult> obv = Mapper.ObvData(candleList, deltaPrice);

            Log.Information("Oscillator (%K) = " + stoch.Last().Oscillator);
            Log.Information("Signal (%D) = " + stoch.Last().Signal);
            Log.Information("PercentJ = " + stoch.Last().PercentJ);

            //Log.Information("PreLast Oscillator (%K) = " + stoch[stoch.Count() - 2].Oscillator);
            //Log.Information("PreLast Signal (%D) = " + stoch[stoch.Count() - 2].Signal);
            //Log.Information("PreLast PercentJ = " + stoch[stoch.Count() - 2].PercentJ);

            var obvValues = obv.Select(obv => obv.Obv).ToList();
            var ObvLinearAngle = LinearAngle(obvValues, obvCount);
            Log.Information("ObvLinearAngle = " + ObvLinearAngle);

            var OscillatorlinearAngle = StochLinearDegreeAverageAngle(stoch, sctochCount, Stoch.Oscillator);
            Log.Information("OscillatorlinearAngle = " + OscillatorlinearAngle);
            //var SignalDegreeAverageAngle = StochLinearDegreeAverageAngle(stoch, sctochCount, Stoch.Signal);
            //var PercentJDegreeAverageAngle = StochLinearDegreeAverageAngle(stoch, sctochCount, Stoch.PercentJ);
            var TrandCloseLinearAngle = TrandLinearAngle(candleList, CloseCount);
            Log.Information("TrandCloseLinearAngle = " + TrandCloseLinearAngle);
            var TrandVolumeLinearAngle = TrandLinearAngle(candleList, volumeCount, CandleValue.Volume);
            Log.Information("TrandVolumeLinearAngle = " + TrandVolumeLinearAngle);
            //var OscillatorMaxStochDegree = MaxStochDegree(stoch, analisysCount, Stoch.Oscillator);

            var OscillatorLinearAngleT = StochLinearDegreeAverageAngle(stoch, 1, Stoch.Oscillator);
            //var TrandCloseLinearAngleT = TrandLinearAngle(candleList, 3);
            //var TrandVolumeLinearAngleT = TrandLinearAngle(candleList, 3, CandleValue.Volume);

            //Log.Information("SignalDegreeAverageAngle = " + SignalDegreeAverageAngle);
            //Log.Information("PercentJDegreeAverageAngle = " + PercentJDegreeAverageAngle);
            //Log.Information("OscillatorMaxStochDegree = " + OscillatorMaxStochDegree);
            if (
                OscillatorlinearAngle > 10
                &&
                ObvLinearAngle > 10
                &&
                TrandCloseLinearAngle < -10
                &&
                stoch.Last().Oscillator < 50
                //&&
                //TrandVolumeLinearAngle > 30
                &&
                OscillatorLinearAngleT > 0
                //&&
                //TrandCloseLinearAngleT < 0
                //&&
                //TrandVolumeLinearAngleT >0
                )
            {
                Log.Information("StochDiveregentSignal = Long - true for: " + candleList.Figi);
                return true;
            }
            else
            {
                Log.Information("StochDiveregentSignal = Long - false for: " + candleList.Figi);
                return false;
            }
        }
        double TrandLinearAngle(CandlesList candleList, int anglesCount, CandleValue candleValue = CandleValue.Close)
        {
            Log.Information("Start TrandLinearAverageAngle. Figi: " + candleList.Figi);
            List<decimal> values = null;

            if (candleValue == CandleValue.Close)
            {
                values = candleList.Candles.Select(na => na.Close).ToList();
            }
            else if (candleValue == CandleValue.Open)
            {
                values = candleList.Candles.Select(na => na.Open).ToList();
            }
            else if (candleValue == CandleValue.Low)
            {
                values = candleList.Candles.Select(na => na.Low).ToList();
            }
            else if (candleValue == CandleValue.High)
            {
                values = candleList.Candles.Select(na => na.High).ToList();
            }
            else if (candleValue == CandleValue.Volume)
            {
                values = candleList.Candles.Select(na => na.Volume).ToList();
            }
            Log.Information("Stop TrandLinearAverageAngle. Figi: " + candleList.Figi);
            return LinearAngle(values, anglesCount);

        }
        decimal? MaxCloseDegree(CandlesList candleList, int count)
        {

            List<decimal?> values = candleList.Candles.Select(na => (decimal?)na.High).ToList();
            return MaxLastDegree(values, count);

        }

        decimal? MaxStochDegree(List<StochResult> AdlValue, int count, Stoch line)
        {
            if (line == Stoch.Oscillator)
            {
                List<decimal?> values = AdlValue.Select(na => (decimal?)na.Oscillator).ToList();
                return MaxLastDegree(values, count);
            }
            else if (line == Stoch.Signal)
            {

                List<decimal?> values = AdlValue.Select(na => (decimal?)na.Signal).ToList();
                return MaxLastDegree(values, count);
            }
            else
            {

                List<decimal?> values = AdlValue.Select(na => (decimal?)na.PercentJ).ToList();
                return MaxLastDegree(values, count);
            }
        }

        //internal bool StochFromLongSignal(CandlesList candleList, decimal deltaPrice)
        //{

        //}

        //enum Stoch
        //{
        //    Oscillator,
        //    Signal,
        //    PercentJ
        //}
    }
}
