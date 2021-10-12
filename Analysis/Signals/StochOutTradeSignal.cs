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
            Log.Information("Start StochOutTradeSignal. Figi: " + candleList.Figi);
            decimal deltaPrice = (orderbook.Asks.FirstOrDefault().Price + orderbook.Bids.FirstOrDefault().Price) / 2;
            List<StochResult> stoch = Mapper.StochData(candleList, deltaPrice, stochLookbackPeriod, stochSignalPeriod, stochSmoothPeriod);
            decimal? stochOscillator = stoch.Last().Oscillator;
            //decimal? stochSignal = stoch.Last().Signal;            

            Log.Information("Oscillator (%K) = " + stochOscillator);
            //Log.Information("Signal (%D) = " + stochSignal);
            //Log.Information("PercentJ = " + stoch.Last().PercentJ);

            //Log.Information("PreLast Oscillator (%K) = " + stoch[^2].Oscillator);
            //Log.Information("PreLast Signal (%D) = " + stoch[^2].Signal);
            //Log.Information("PreLast PercentJ = " + stoch[^2].PercentJ);

            //var OscillatorDegreeAverageAngle = StochDegreeAverageAngle(stoch, 1, Stoch.Oscillator);
            //var SignalDegreeAverageAngle = StochDegreeAverageAngle(stoch, 1, Stoch.Signal);
            //var PercentJDegreeAverageAngle = StochDegreeAverageAngle(stoch, 1, Stoch.PercentJ);

            //Log.Information("OscillatorDegreeAverageAngle = " + OscillatorDegreeAverageAngle);
            //Log.Information("SignalDegreeAverageAngle = " + SignalDegreeAverageAngle);
            //Log.Information("PercentJDegreeAverageAngle = " + PercentJDegreeAverageAngle);


            if(stochOscillator < 25)
            {

                Log.Information("Stop StochOutTradeSignal. Figi: " + candleList.Figi + " TradeTarget.fromLong");
                return TradeTarget.fromLong;
            }
            else if(stochOscillator > 75)
            {
                Log.Information("Stop StochOutTradeSignal. Figi: " + candleList.Figi + " TradeTarget.fromShort");
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
