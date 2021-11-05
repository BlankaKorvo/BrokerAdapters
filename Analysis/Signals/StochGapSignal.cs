using MarketDataModules;
using MarketDataModules.Models.Candles;
using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analysis.Signals.Helpers;

namespace Analysis.Signals
{
    public partial class Signal : IndicatorSignalsHelper
    {
        int StochGapLookbackPeriod = 14;
        int StochGapPeriod = 3;
        int StochGapMoothPeriod = 1;

        internal bool StochGapSignalLongSignal(CandlesList candleList, decimal deltaPrice)
        {
            Log.Information("Start StochGapSignalLongSignal. Figi: " + candleList.Figi);
            List<StochResult> stoch = Mapper.StochData(candleList, deltaPrice, StochGapLookbackPeriod, StochGapPeriod, StochGapMoothPeriod);
            decimal? lastStochOscillator = stoch.Last().Oscillator;
            decimal? preLastStochOscillator = stoch[stoch.Count() - 2].Oscillator;

            Log.Information("Oscillator (%K) = " + lastStochOscillator);
            //Log.Information("Signal (%D) = " + stoch.Last().Signal);
            //Log.Information("PercentJ = " + stoch.Last().PercentJ);

            Log.Information("PreLast Oscillator (%K) = " + preLastStochOscillator);
            //Log.Information("PreLast Signal (%D) = " + stoch[stoch.Count() - 2].Signal);
            //Log.Information("PreLast PercentJ = " + stoch[stoch.Count() - 2].PercentJ);


            //Log.Information("OscillatorDegreeAverageAngle = " + OscillatorDegreeAverageAngle);
            //Log.Information("SignalDegreeAverageAngle = " + SignalDegreeAverageAngle);
            //Log.Information("PercentJDegreeAverageAngle = " + PercentJDegreeAverageAngle);


            if (
                lastStochOscillator > 80
                &&
                preLastStochOscillator < 20
                )
            {

                Log.Information("Stoch = Long - true for: " + candleList.Figi);
                return true;
            }
            else
            {
                Log.Information("Stoch = Long - false for: " + candleList.Figi);
                return false;
            }
        }


        //internal bool StochFromLongSignal(CandlesList candleList, decimal deltaPrice)
        //{

        //}


    }
}
