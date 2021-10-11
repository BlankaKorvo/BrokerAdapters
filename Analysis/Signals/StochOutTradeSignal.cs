using MarketDataModules;
using MarketDataModules.Models;
using MarketDataModules.Models.Candles;
using MarketDataModules.Models.Orderbooks;
using Serilog;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinkoffData;
using TradingAlgorithms.IndicatorSignals.Helpers;

namespace TradingAlgorithms.IndicatorSignals
{
    public partial class Signal : IndicatorSignalsHelper
    {

        internal TradeTarget StochOutTradeSignal(CandlesList candleList, Orderbook orderbook)
        {

            decimal deltaPrice = (orderbook.Asks.FirstOrDefault().Price + orderbook.Bids.FirstOrDefault().Price) / 2;
            Log.Information("Start StochOutTradeSignal. Figi: " + candleList.Figi);
            List<StochResult> stoch = Mapper.StochData(candleList, deltaPrice, stochLookbackPeriod, stochSignalPeriod, stochSmoothPeriod);
            Log.Information("Oscillator (%K) = " + stoch.Last().Oscillator);
            Log.Information("Signal (%D) = " + stoch.Last().Signal);
            Log.Information("PercentJ = " + stoch.Last().PercentJ);

            Log.Information("PreLast Oscillator (%K) = " + stoch[^2].Oscillator);
            Log.Information("PreLast Signal (%D) = " + stoch[^2].Signal);
            Log.Information("PreLast PercentJ = " + stoch[^2].PercentJ);

            var OscillatorDegreeAverageAngle = StochDegreeAverageAngle(stoch, 1, Stoch.Oscillator);
            var SignalDegreeAverageAngle = StochDegreeAverageAngle(stoch, 1, Stoch.Signal);
            var PercentJDegreeAverageAngle = StochDegreeAverageAngle(stoch, 1, Stoch.PercentJ);

            Log.Information("OscillatorDegreeAverageAngle = " + OscillatorDegreeAverageAngle);
            Log.Information("SignalDegreeAverageAngle = " + SignalDegreeAverageAngle);
            Log.Information("PercentJDegreeAverageAngle = " + PercentJDegreeAverageAngle);


            if(stoch.Last().Oscillator < 20)
            {

                Log.Information("Stoch = FromLong - true for: " + candleList.Figi);
                return TradeTarget.fromLong;
            }
            else if(stoch.Last().Oscillator > 80)
            {
                Log.Information("Stoch = FromShort - false for: " + candleList.Figi);
                return TradeTarget.fromShort;
            }
            return TradeTarget.notTrading;
        }
        

    }

        enum Stoch
        {
            Oscillator,
            Signal,
            PercentJ
        }
    
}
