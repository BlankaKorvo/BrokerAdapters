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
        public TradeOperation GmmaSignal(CandlesList candleList, decimal bestAsk, decimal bestBid)
        {
            Log.Information("Start GmmaSignal. Figi: " + candleList.Figi);
            Log.Information("CondleList count = " + candleList.Candles.Count);
            Log.Information("CondleList last date = " + candleList.Candles.LastOrDefault().Time.ToString());
            Log.Information("bestAsk = " + bestAsk);
            Log.Information("bestBid = " + bestBid);

            decimal price = (bestAsk + bestBid) / 2;

            Log.Information("price = " + price);

            List<EmaResult> emaShort1 = Mapper.EmaData(candleList, price, emaShort1Period, CandleStruct.Close);
            List<EmaResult> emaShort2 = Mapper.EmaData(candleList, price, emaShort2Period, CandleStruct.Close);
            List<EmaResult> emaShort3 = Mapper.EmaData(candleList, price, emaShort3Period, CandleStruct.Close);
            List<EmaResult> emaShort4 = Mapper.EmaData(candleList, price, emaShort4Period, CandleStruct.Close);
            List<EmaResult> emaShort5 = Mapper.EmaData(candleList, price, emaShort5Period, CandleStruct.Close);
            List<EmaResult> emaShort6 = Mapper.EmaData(candleList, price, emaShort6Period, CandleStruct.Close);

            List<EmaResult> emaLong1 = Mapper.EmaData(candleList, price, emaLong1Period, CandleStruct.Close);
            List<EmaResult> emaLong2 = Mapper.EmaData(candleList, price, emaLong2Period, CandleStruct.Close);
            List<EmaResult> emaLong3 = Mapper.EmaData(candleList, price, emaLong3Period, CandleStruct.Close);
            List<EmaResult> emaLong4 = Mapper.EmaData(candleList, price, emaLong4Period, CandleStruct.Close);
            List<EmaResult> emaLong5 = Mapper.EmaData(candleList, price, emaLong5Period, CandleStruct.Close);
            List<EmaResult> emaLong6 = Mapper.EmaData(candleList, price, emaLong6Period, CandleStruct.Close);

            List<EmaResult> emaShortLow1 = Mapper.EmaData(candleList, price, emaShort1Period, CandleStruct.Low);
            List<EmaResult> emaShortLow2 = Mapper.EmaData(candleList, price, emaShort2Period, CandleStruct.Low);
            List<EmaResult> emaShortLow3 = Mapper.EmaData(candleList, price, emaShort3Period, CandleStruct.Low);
            List<EmaResult> emaShortLow4 = Mapper.EmaData(candleList, price, emaShort4Period, CandleStruct.Low);
            List<EmaResult> emaShortLow5 = Mapper.EmaData(candleList, price, emaShort5Period, CandleStruct.Low);
            List<EmaResult> emaShortLow6 = Mapper.EmaData(candleList, price, emaShort6Period, CandleStruct.Low);

            List<EmaResult> emaLongLow1 = Mapper.EmaData(candleList, price, emaLong1Period, CandleStruct.Low);
            List<EmaResult> emaLongLow2 = Mapper.EmaData(candleList, price, emaLong2Period, CandleStruct.Low);
            List<EmaResult> emaLongLow3 = Mapper.EmaData(candleList, price, emaLong3Period, CandleStruct.Low);
            List<EmaResult> emaLongLow4 = Mapper.EmaData(candleList, price, emaLong4Period, CandleStruct.Low);
            List<EmaResult> emaLongLow5 = Mapper.EmaData(candleList, price, emaLong5Period, CandleStruct.Low);
            List<EmaResult> emaLongLow6 = Mapper.EmaData(candleList, price, emaLong6Period, CandleStruct.Low);

            List<EmaResult> emaShortHigh1 = Mapper.EmaData(candleList, price, emaShort1Period, CandleStruct.High);
            List<EmaResult> emaShortHigh2 = Mapper.EmaData(candleList, price, emaShort2Period, CandleStruct.High);
            List<EmaResult> emaShortHigh3 = Mapper.EmaData(candleList, price, emaShort3Period, CandleStruct.High);
            List<EmaResult> emaShortHigh4 = Mapper.EmaData(candleList, price, emaShort4Period, CandleStruct.High);
            List<EmaResult> emaShortHigh5 = Mapper.EmaData(candleList, price, emaShort5Period, CandleStruct.High);
            List<EmaResult> emaShortHigh6 = Mapper.EmaData(candleList, price, emaShort6Period, CandleStruct.High);

            List<EmaResult> emaLongHigh1 = Mapper.EmaData(candleList, price, emaLong1Period, CandleStruct.High);
            List<EmaResult> emaLongHigh2 = Mapper.EmaData(candleList, price, emaLong2Period, CandleStruct.High);
            List<EmaResult> emaLongHigh3 = Mapper.EmaData(candleList, price, emaLong3Period, CandleStruct.High);
            List<EmaResult> emaLongHigh4 = Mapper.EmaData(candleList, price, emaLong4Period, CandleStruct.High);
            List<EmaResult> emaLongHigh5 = Mapper.EmaData(candleList, price, emaLong5Period, CandleStruct.High);
            List<EmaResult> emaLongHigh6 = Mapper.EmaData(candleList, price, emaLong6Period, CandleStruct.High);


            double emaShort1LinearAngle = LinearAngle(emaShort1.Select(x => x.Ema).ToList(), 1);
            double emaShort2LinearAngle = LinearAngle(emaShort2.Select(x => x.Ema).ToList(), 1);
            double emaShort3LinearAngle = LinearAngle(emaShort3.Select(x => x.Ema).ToList(), 1);
            double emaShort4LinearAngle = LinearAngle(emaShort4.Select(x => x.Ema).ToList(), 1);
            double emaShort5LinearAngle = LinearAngle(emaShort5.Select(x => x.Ema).ToList(), 1);
            double emaShort6LinearAngle = LinearAngle(emaShort6.Select(x => x.Ema).ToList(), 1);

            double emaLong1LinearAngle = LinearAngle(emaLong1.Select(x => x.Ema).ToList(), 1);
            double emaLong2LinearAngle = LinearAngle(emaLong2.Select(x => x.Ema).ToList(), 1);
            double emaLong3LinearAngle = LinearAngle(emaLong3.Select(x => x.Ema).ToList(), 1);
            double emaLong4LinearAngle = LinearAngle(emaLong4.Select(x => x.Ema).ToList(), 1);
            double emaLong5LinearAngle = LinearAngle(emaLong5.Select(x => x.Ema).ToList(), 1);
            double emaLong6LinearAngle = LinearAngle(emaLong6.Select(x => x.Ema).ToList(), 1);

            Log.Information("emaShort1 = " + emaShort1.LastOrDefault().Ema + " emaLong1 = " + emaLong1.LastOrDefault().Ema);
            Log.Information("emaShort2 = " + emaShort2.LastOrDefault().Ema + " emaLong2 = " + emaLong2.LastOrDefault().Ema);
            Log.Information("emaShort3 = " + emaShort3.LastOrDefault().Ema + " emaLong3 = " + emaLong3.LastOrDefault().Ema);
            Log.Information("emaShort4 = " + emaShort4.LastOrDefault().Ema + " emaLong4 = " + emaLong4.LastOrDefault().Ema);
            Log.Information("emaShort5 = " + emaShort5.LastOrDefault().Ema + " emaLong5 = " + emaLong5.LastOrDefault().Ema);
            Log.Information("emaShort6 = " + emaShort6.LastOrDefault().Ema + " emaLong6 = " + emaLong6.LastOrDefault().Ema);

            Log.Information("emaLong1 = " + emaLong1.LastOrDefault().Ema + " date:" + emaLong1.LastOrDefault().Date.ToString());
            Log.Information("emaLong2 = " + emaLong2.LastOrDefault().Ema + " date:" + emaLong2.LastOrDefault().Date.ToString());
            Log.Information("emaLong3 = " + emaLong3.LastOrDefault().Ema + " date:" + emaLong3.LastOrDefault().Date.ToString());
            Log.Information("emaLong4 = " + emaLong4.LastOrDefault().Ema + " date:" + emaLong4.LastOrDefault().Date.ToString());
            Log.Information("emaLong5 = " + emaLong5.LastOrDefault().Ema + " date:" + emaLong5.LastOrDefault().Date.ToString());
            Log.Information("emaLong6 = " + emaLong6.LastOrDefault().Ema + " date:" + emaLong6.LastOrDefault().Date.ToString());

            Log.Information("emaShort1[^2].Ema = " + emaShort1[^2].Ema + " emaShort1[^3].Ema = " + emaShort1[^3].Ema);
            Log.Information("emaShort2[^2].Ema = " + emaShort2[^2].Ema + " emaShort2[^3].Ema = " + emaShort2[^3].Ema);
            Log.Information("emaShort3[^2].Ema = " + emaShort3[^2].Ema + " emaShort3[^3].Ema = " + emaShort3[^3].Ema);
            Log.Information("emaShort4[^2].Ema = " + emaShort4[^2].Ema + " emaShort4[^3].Ema = " + emaShort4[^3].Ema);
            Log.Information("emaShort5[^2].Ema = " + emaShort5[^2].Ema + " emaShort5[^3].Ema = " + emaShort5[^3].Ema);
            Log.Information("emaShort6[^2].Ema = " + emaShort6[^2].Ema + " emaShort6[^3].Ema = " + emaShort6[^3].Ema);

            Log.Information("emaShort1LinearAngle(1) = " + emaShort1LinearAngle);
            Log.Information("emaShort2LinearAngle(1) = " + emaShort2LinearAngle);
            Log.Information("emaShort3LinearAngle(1) = " + emaShort3LinearAngle);
            Log.Information("emaShort4LinearAngle(1) = " + emaShort4LinearAngle);
            Log.Information("emaShort5LinearAngle(1) = " + emaShort5LinearAngle);
            Log.Information("emaShort6LinearAngle(1) = " + emaShort6LinearAngle);

            if //to Long
                (
                emaShort1[^2].Ema > emaShort1[^3].Ema
                &&
                emaShort2[^2].Ema > emaShort2[^3].Ema
                &&
                emaShort3[^2].Ema > emaShort3[^3].Ema
                &&
                emaShort4[^2].Ema > emaShort4[^3].Ema
                &&
                emaShort5[^2].Ema > emaShort5[^3].Ema
                &&
                emaShort6[^2].Ema > emaShort6[^3].Ema

                &&
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
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeOperation.toLong");
                return TradeOperation.toLong;
            }
            else if // to short
                (
                emaShort1[^2].Ema < emaShort1[^3].Ema
                &&
                emaShort2[^2].Ema < emaShort2[^3].Ema
                &&
                emaShort3[^2].Ema < emaShort3[^3].Ema
                &&
                emaShort4[^2].Ema < emaShort4[^3].Ema
                &&
                emaShort5[^2].Ema < emaShort5[^3].Ema
                &&
                emaShort6[^2].Ema < emaShort6[^3].Ema


                &&
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
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeOperation.toShort");
                return TradeOperation.toShort;
            }
            else if // from Long
                (
                emaShort6LinearAngle < 0
                ||
                emaShort1.LastOrDefault().Ema < emaShort2.LastOrDefault().Ema
                )
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeOperation.fromLong");
                return TradeOperation.fromLong;
            }

            else if // from Short
                (
                emaShort6LinearAngle > 0
                ||
                emaShort1.LastOrDefault().Ema > emaShort2.LastOrDefault().Ema
                )
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeOperation.fromShort");
                return TradeOperation.fromShort;
            }
            else
            {
                throw new Exception("GmmaSignal Error");
                //return TradeOperation.notTrading;
            }
        }
    }
}
