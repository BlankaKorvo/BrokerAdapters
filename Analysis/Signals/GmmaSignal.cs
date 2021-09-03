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

namespace TradingAlgorithms.IndicatorSignals
{
    public partial class Signal : IndicatorSignalsHelper
    {
        int emaShort1Period = 3;
        int emaShort2Period = 5;
        int emaShort3Period = 8;
        int emaShort4Period = 10;
        int emaShort5Period = 12;
        int emaShort6Period = 15;

        int emaLong1Period = 30;
        int emaLong2Period = 35;
        int emaLong3Period = 40;
        int emaLong4Period = 45;
        int emaLong5Period = 50;
        int emaLong6Period = 60;
        public bool GmmaLongSignal(CandlesList candleList, decimal price)
        {
            Log.Information("Start Sma LongSignal. Figi: " + candleList.Figi);

            List<EmaResult> emaShort1 = Mapper.EmaData(candleList, price, emaShort1Period);
            List<EmaResult> emaShort2 = Mapper.EmaData(candleList, price, emaShort2Period);
            List<EmaResult> emaShort3 = Mapper.EmaData(candleList, price, emaShort3Period);
            List<EmaResult> emaShort4 = Mapper.EmaData(candleList, price, emaShort4Period);
            List<EmaResult> emaShort5 = Mapper.EmaData(candleList, price, emaShort5Period);
            List<EmaResult> emaShort6 = Mapper.EmaData(candleList, price, emaShort6Period);

            List<EmaResult> emaLong1 = Mapper.EmaData(candleList, price, emaLong1Period);
            //List<EmaResult> emaLong2 = Mapper.EmaData(candleList, price, emaLong2Period);
            //List<EmaResult> emaLong3 = Mapper.EmaData(candleList, price, emaLong3Period);
            //List<EmaResult> emaLong4 = Mapper.EmaData(candleList, price, emaLong4Period);
            //List<EmaResult> emaLong5 = Mapper.EmaData(candleList, price, emaLong5Period);
            List<EmaResult> emaLong6 = Mapper.EmaData(candleList, price, emaLong6Period);

            double emaShort1LinearAngle = LinearAngle(emaShort1.Select(x => x.Ema).ToList(), 1);
            //double emaShort2LinearAngle = LinearAngle(emaShort2.Select(x => x.Ema).ToList(), 1);
            //double emaShort3LinearAngle = LinearAngle(emaShort3.Select(x => x.Ema).ToList(), 1);
            //double emaShort4LinearAngle = LinearAngle(emaShort4.Select(x => x.Ema).ToList(), 1);
            //double emaShort5LinearAngle = LinearAngle(emaShort5.Select(x => x.Ema).ToList(), 1);
            double emaShort6LinearAngle = LinearAngle(emaShort6.Select(x => x.Ema).ToList(), 1);

            double emaLong1LinearAngle = LinearAngle(emaLong1.Select(x => x.Ema).ToList(), 1);
            //double emaLongSecondLinearAngle = LinearAngle(emaLong2.Select(x => x.Ema).ToList(), 1);
            //double emaLongThirdLinearAngle = LinearAngle(emaLong3.Select(x => x.Ema).ToList(), 1);
            //double emaLongFourthLinearAngle = LinearAngle(emaLong4.Select(x => x.Ema).ToList(), 1);
            //double emaLongFifthLinearAngle = LinearAngle(emaLong5.Select(x => x.Ema).ToList(), 1);
            //double emaLongSixthLinearAngle = LinearAngle(emaLong6.Select(x => x.Ema).ToList(), 1);

            if (
                emaShort6.LastOrDefault().Ema > emaLong1.LastOrDefault().Ema
                &&
                emaShort6.LastOrDefault().Ema > emaLong6.LastOrDefault().Ema
                &&
                emaShort1.LastOrDefault().Ema > emaShort2.LastOrDefault().Ema
                &&
                emaShort2.LastOrDefault().Ema > emaShort3.LastOrDefault().Ema
                &&
                emaShort3.LastOrDefault().Ema > emaShort4.LastOrDefault().Ema
                &&
                emaShort4.LastOrDefault().Ema > emaShort5.LastOrDefault().Ema
                &&
                emaShort5.LastOrDefault().Ema > emaShort6.LastOrDefault().Ema
                &&
                emaShort1LinearAngle > 0
                &&
                emaShort6LinearAngle > 5
               )
            {
                return true; 
            }
            else
            {
                return false; 
            }
        }

        public bool GmmaFromLongSignal(CandlesList candleList, decimal price)
        {
            Log.Information("Start Sma LongSignal. Figi: " + candleList.Figi);

            List<EmaResult> emaShort1 = Mapper.EmaData(candleList, price, emaShort1Period);
            List<EmaResult> emaShort2 = Mapper.EmaData(candleList, price, emaShort2Period);
            //List<EmaResult> emaShort3 = Mapper.EmaData(candleList, price, emaShort3Period);
            //List<EmaResult> emaShort4 = Mapper.EmaData(candleList, price, emaShort4Period);
            //List<EmaResult> emaShort5 = Mapper.EmaData(candleList, price, emaShort5Period);
            List<EmaResult> emaShort6 = Mapper.EmaData(candleList, price, emaShort6Period);

            //List<EmaResult> emaLong1 = Mapper.EmaData(candleList, price, emaLong1Period);
            //List<EmaResult> emaLong2 = Mapper.EmaData(candleList, price, emaLong2Period);
            //List<EmaResult> emaLong3 = Mapper.EmaData(candleList, price, emaLong3Period);
            //List<EmaResult> emaLong4 = Mapper.EmaData(candleList, price, emaLong4Period);
            //List<EmaResult> emaLong5 = Mapper.EmaData(candleList, price, emaLong5Period);
            //List<EmaResult> emaLong6 = Mapper.EmaData(candleList, price, emaLong6Period);

            //double emaShort1LinearAngle = LinearAngle(emaShort1.Select(x => x.Ema).ToList(), 1);
            //double emaShort2LinearAngle = LinearAngle(emaShort2.Select(x => x.Ema).ToList(), 1);
            //double emaShort3LinearAngle = LinearAngle(emaShort3.Select(x => x.Ema).ToList(), 1);
            //double emaShort4LinearAngle = LinearAngle(emaShort4.Select(x => x.Ema).ToList(), 1);
            //double emaShort5LinearAngle = LinearAngle(emaShort5.Select(x => x.Ema).ToList(), 1);
            double emaShort6LinearAngle = LinearAngle(emaShort6.Select(x => x.Ema).ToList(), 1);

            //double emaLongFirstLinearAngle = LinearAngle(emaLong1.Select(x => x.Ema).ToList(), 1);
            //double emaLongSecondLinearAngle = LinearAngle(emaLong2.Select(x => x.Ema).ToList(), 1);
            //double emaLongThirdLinearAngle = LinearAngle(emaLong3.Select(x => x.Ema).ToList(), 1);
            //double emaLongFourthLinearAngle = LinearAngle(emaLong4.Select(x => x.Ema).ToList(), 1);
            //double emaLongFifthLinearAngle = LinearAngle(emaLong5.Select(x => x.Ema).ToList(), 1);
            //double emaLongSixthLinearAngle = LinearAngle(emaLong6.Select(x => x.Ema).ToList(), 1);

            if (
                emaShort6LinearAngle < 0
                ||
                emaShort1.LastOrDefault().Ema < emaShort2.LastOrDefault().Ema
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GmmaShortSignal(CandlesList candleList, decimal price)
        {
            Log.Information("Start Sma LongSignal. Figi: " + candleList.Figi);

            List<EmaResult> emaShort1 = Mapper.EmaData(candleList, price, emaShort1Period);
            List<EmaResult> emaShort2 = Mapper.EmaData(candleList, price, emaShort2Period);
            List<EmaResult> emaShort3 = Mapper.EmaData(candleList, price, emaShort3Period);
            List<EmaResult> emaShort4 = Mapper.EmaData(candleList, price, emaShort4Period);
            List<EmaResult> emaShort5 = Mapper.EmaData(candleList, price, emaShort5Period);
            List<EmaResult> emaShort6 = Mapper.EmaData(candleList, price, emaShort6Period);

            List<EmaResult> emaLong1 = Mapper.EmaData(candleList, price, emaLong1Period);
            //List<EmaResult> emaLong2 = Mapper.EmaData(candleList, price, emaLong2Period);
            //List<EmaResult> emaLong3 = Mapper.EmaData(candleList, price, emaLong3Period);
            //List<EmaResult> emaLong4 = Mapper.EmaData(candleList, price, emaLong4Period);
            //List<EmaResult> emaLong5 = Mapper.EmaData(candleList, price, emaLong5Period);
            List<EmaResult> emaLong6 = Mapper.EmaData(candleList, price, emaLong6Period);

            double emaShort1LinearAngle = LinearAngle(emaShort1.Select(x => x.Ema).ToList(), 1);
            //double emaShort2LinearAngle = LinearAngle(emaShort2.Select(x => x.Ema).ToList(), 1);
            //double emaShort3LinearAngle = LinearAngle(emaShort3.Select(x => x.Ema).ToList(), 1);
            //double emaShort4LinearAngle = LinearAngle(emaShort4.Select(x => x.Ema).ToList(), 1);
            //double emaShort5LinearAngle = LinearAngle(emaShort5.Select(x => x.Ema).ToList(), 1);
            double emaShort6LinearAngle = LinearAngle(emaShort6.Select(x => x.Ema).ToList(), 1);

            //double emaLong1LinearAngle = LinearAngle(emaLong1.Select(x => x.Ema).ToList(), 1);
            //double emaLongSecondLinearAngle = LinearAngle(emaLong2.Select(x => x.Ema).ToList(), 1);
            //double emaLongThirdLinearAngle = LinearAngle(emaLong3.Select(x => x.Ema).ToList(), 1);
            //double emaLongFourthLinearAngle = LinearAngle(emaLong4.Select(x => x.Ema).ToList(), 1);
            //double emaLongFifthLinearAngle = LinearAngle(emaLong5.Select(x => x.Ema).ToList(), 1);
            //double emaLongSixthLinearAngle = LinearAngle(emaLong6.Select(x => x.Ema).ToList(), 1);

            if (
                emaShort6.LastOrDefault().Ema < emaLong1.LastOrDefault().Ema
                &&
                emaShort6.LastOrDefault().Ema < emaLong6.LastOrDefault().Ema
                &&
                emaShort1.LastOrDefault().Ema < emaShort2.LastOrDefault().Ema
                &&
                emaShort2.LastOrDefault().Ema < emaShort3.LastOrDefault().Ema
                &&
                emaShort3.LastOrDefault().Ema < emaShort4.LastOrDefault().Ema
                &&
                emaShort4.LastOrDefault().Ema < emaShort5.LastOrDefault().Ema
                &&
                emaShort5.LastOrDefault().Ema < emaShort6.LastOrDefault().Ema
                &&
                emaShort1LinearAngle < 0
                &&
                emaShort6LinearAngle < 5
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GmmaFromShortSignal(CandlesList candleList, decimal price)
        {
            Log.Information("Start Sma LongSignal. Figi: " + candleList.Figi);

            List<EmaResult> emaShort1 = Mapper.EmaData(candleList, price, emaShort1Period);
            List<EmaResult> emaShort2 = Mapper.EmaData(candleList, price, emaShort2Period);
            //List<EmaResult> emaShort3 = Mapper.EmaData(candleList, price, emaShort3Period);
            //List<EmaResult> emaShort4 = Mapper.EmaData(candleList, price, emaShort4Period);
            //List<EmaResult> emaShort5 = Mapper.EmaData(candleList, price, emaShort5Period);
            List<EmaResult> emaShort6 = Mapper.EmaData(candleList, price, emaShort6Period);

            //List<EmaResult> emaLong1 = Mapper.EmaData(candleList, price, emaLong1Period);
            //List<EmaResult> emaLong2 = Mapper.EmaData(candleList, price, emaLong2Period);
            //List<EmaResult> emaLong3 = Mapper.EmaData(candleList, price, emaLong3Period);
            //List<EmaResult> emaLong4 = Mapper.EmaData(candleList, price, emaLong4Period);
            //List<EmaResult> emaLong5 = Mapper.EmaData(candleList, price, emaLong5Period);
            //List<EmaResult> emaLong6 = Mapper.EmaData(candleList, price, emaLong6Period);

            //double emaShort1LinearAngle = LinearAngle(emaShort1.Select(x => x.Ema).ToList(), 1);
            //double emaShort2LinearAngle = LinearAngle(emaShort2.Select(x => x.Ema).ToList(), 1);
            //double emaShort3LinearAngle = LinearAngle(emaShort3.Select(x => x.Ema).ToList(), 1);
            //double emaShort4LinearAngle = LinearAngle(emaShort4.Select(x => x.Ema).ToList(), 1);
            //double emaShort5LinearAngle = LinearAngle(emaShort5.Select(x => x.Ema).ToList(), 1);
            double emaShort6LinearAngle = LinearAngle(emaShort6.Select(x => x.Ema).ToList(), 1);

            //double emaLongFirstLinearAngle = LinearAngle(emaLong1.Select(x => x.Ema).ToList(), 1);
            //double emaLongSecondLinearAngle = LinearAngle(emaLong2.Select(x => x.Ema).ToList(), 1);
            //double emaLongThirdLinearAngle = LinearAngle(emaLong3.Select(x => x.Ema).ToList(), 1);
            //double emaLongFourthLinearAngle = LinearAngle(emaLong4.Select(x => x.Ema).ToList(), 1);
            //double emaLongFifthLinearAngle = LinearAngle(emaLong5.Select(x => x.Ema).ToList(), 1);
            //double emaLongSixthLinearAngle = LinearAngle(emaLong6.Select(x => x.Ema).ToList(), 1);

            if (
                emaShort6LinearAngle > 0
                ||
                emaShort1.LastOrDefault().Ema > emaShort2.LastOrDefault().Ema
               )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
