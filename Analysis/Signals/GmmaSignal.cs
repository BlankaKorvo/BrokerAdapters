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

        int emaMaxPeriod = 200;
        public TradeTarget GmmaSignal(CandlesList candleList, decimal bestAsk, decimal bestBid)
        {
            Log.Information("Start GmmaSignal. Figi: " + candleList.Figi);
            Log.Information("CondleList count = " + candleList.Candles.Count);
            Log.Information("CondleList last date = " + candleList.Candles.LastOrDefault().Time.ToString());
            Log.Information("bestAsk = " + bestAsk);
            Log.Information("bestBid = " + bestBid);

            decimal price = (bestAsk + bestBid) / 2;

            Log.Information("price = " + price);


            //Ema Close
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

            //List<EmaResult> emaMax = Mapper.EmaData(candleList, price, emaMaxPeriod, CandleStruct.Close);

            //Ema Low
            List<EmaResult> emaShort1Low = Mapper.EmaData(candleList, price, emaShort1Period, CandleStruct.Low, true);
            List<EmaResult> emaShort2Low = Mapper.EmaData(candleList, price, emaShort2Period, CandleStruct.Low, true);
            List<EmaResult> emaShort3Low = Mapper.EmaData(candleList, price, emaShort3Period, CandleStruct.Low, true);
            List<EmaResult> emaShort4Low = Mapper.EmaData(candleList, price, emaShort4Period, CandleStruct.Low, true);
            List<EmaResult> emaShort5Low = Mapper.EmaData(candleList, price, emaShort5Period, CandleStruct.Low, true);
            List<EmaResult> emaShort6Low = Mapper.EmaData(candleList, price, emaShort6Period, CandleStruct.Low, true);

            List<EmaResult> emaLong1Low = Mapper.EmaData(candleList, price, emaLong1Period, CandleStruct.Low, true);
            List<EmaResult> emaLong2Low = Mapper.EmaData(candleList, price, emaLong2Period, CandleStruct.Low, true);
            List<EmaResult> emaLong3Low = Mapper.EmaData(candleList, price, emaLong3Period, CandleStruct.Low, true);
            List<EmaResult> emaLong4Low = Mapper.EmaData(candleList, price, emaLong4Period, CandleStruct.Low, true);
            List<EmaResult> emaLong5Low = Mapper.EmaData(candleList, price, emaLong5Period, CandleStruct.Low, true);
            List<EmaResult> emaLong6Low = Mapper.EmaData(candleList, price, emaLong6Period, CandleStruct.Low, true);

            //Ema High
            List<EmaResult> emaShort1High = Mapper.EmaData(candleList, price, emaShort1Period, CandleStruct.High, true);
            List<EmaResult> emaShort2High = Mapper.EmaData(candleList, price, emaShort2Period, CandleStruct.High, true);
            List<EmaResult> emaShort3High = Mapper.EmaData(candleList, price, emaShort3Period, CandleStruct.High, true);
            List<EmaResult> emaShort4High = Mapper.EmaData(candleList, price, emaShort4Period, CandleStruct.High, true);
            List<EmaResult> emaShort5High = Mapper.EmaData(candleList, price, emaShort5Period, CandleStruct.High, true);
            List<EmaResult> emaShort6High = Mapper.EmaData(candleList, price, emaShort6Period, CandleStruct.High, true);

            List<EmaResult> emaLong1High = Mapper.EmaData(candleList, price, emaLong1Period, CandleStruct.High, true);
            List<EmaResult> emaLong2High = Mapper.EmaData(candleList, price, emaLong2Period, CandleStruct.High, true);
            List<EmaResult> emaLong3High = Mapper.EmaData(candleList, price, emaLong3Period, CandleStruct.High, true);
            List<EmaResult> emaLong4High = Mapper.EmaData(candleList, price, emaLong4Period, CandleStruct.High, true);
            List<EmaResult> emaLong5High = Mapper.EmaData(candleList, price, emaLong5Period, CandleStruct.High, true);
            List<EmaResult> emaLong6High = Mapper.EmaData(candleList, price, emaLong6Period, CandleStruct.High, true);

            ////Angle Count
            //double emaShort1Angle = AngleCalc((decimal)emaShort1[^2].Ema, (decimal)emaShort1[^1].Ema);
            //double emaShort2Angle = AngleCalc((decimal)emaShort2[^2].Ema, (decimal)emaShort2[^1].Ema);
            //double emaShort3Angle = AngleCalc((decimal)emaShort3[^2].Ema, (decimal)emaShort3[^1].Ema);
            //double emaShort4Angle = AngleCalc((decimal)emaShort4[^2].Ema, (decimal)emaShort4[^1].Ema);
            //double emaShort5Angle = AngleCalc((decimal)emaShort5[^2].Ema, (decimal)emaShort5[^1].Ema);
            //double emaShort6Angle = AngleCalc((decimal)emaShort6[^2].Ema, (decimal)emaShort6[^1].Ema);

            //double emaLong1Angle = AngleCalc((decimal)emaLong1[^2].Ema, (decimal)emaLong1[^1].Ema);
            //double emaLong2Angle = AngleCalc((decimal)emaLong2[^2].Ema, (decimal)emaLong2[^1].Ema);
            //double emaLong3Angle = AngleCalc((decimal)emaLong3[^2].Ema, (decimal)emaLong3[^1].Ema);
            //double emaLong4Angle = AngleCalc((decimal)emaLong4[^2].Ema, (decimal)emaLong4[^1].Ema);
            //double emaLong5Angle = AngleCalc((decimal)emaLong5[^2].Ema, (decimal)emaLong5[^1].Ema);
            //double emaLong6Angle = AngleCalc((decimal)emaLong6[^2].Ema, (decimal)emaLong6[^1].Ema);

            //LinearAngle Close
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


            //LinearAngle(2) Close
            double emaShort1LinearAngleInc = LinearAngle(emaShort1.Select(x => x.Ema).ToList(), 2);
            double emaShort2LinearAngleInc = LinearAngle(emaShort2.Select(x => x.Ema).ToList(), 2);
            double emaShort3LinearAngleInc = LinearAngle(emaShort3.Select(x => x.Ema).ToList(), 2);
            double emaShort4LinearAngleInc = LinearAngle(emaShort4.Select(x => x.Ema).ToList(), 2);
            double emaShort5LinearAngleInc = LinearAngle(emaShort5.Select(x => x.Ema).ToList(), 2);
            double emaShort6LinearAngleInc = LinearAngle(emaShort6.Select(x => x.Ema).ToList(), 2);

            double emaLong1LinearAngleInc = LinearAngle(emaLong1.Select(x => x.Ema).ToList(), 2);
            double emaLong2LinearAngleInc = LinearAngle(emaLong2.Select(x => x.Ema).ToList(), 2);
            double emaLong3LinearAngleInc = LinearAngle(emaLong3.Select(x => x.Ema).ToList(), 2);
            double emaLong4LinearAngleInc = LinearAngle(emaLong4.Select(x => x.Ema).ToList(), 2);
            double emaLong5LinearAngleInc = LinearAngle(emaLong5.Select(x => x.Ema).ToList(), 2);
            double emaLong6LinearAngleInc = LinearAngle(emaLong6.Select(x => x.Ema).ToList(), 2);

            //LinearAngle High
            double emaShort1HighLinearAngle = LinearAngle(emaShort1High.Select(x => x.Ema).ToList(), 1);
            double emaShort2HighLinearAngle = LinearAngle(emaShort2High.Select(x => x.Ema).ToList(), 1);
            double emaShort3HighLinearAngle = LinearAngle(emaShort3High.Select(x => x.Ema).ToList(), 1);
            double emaShort4HighLinearAngle = LinearAngle(emaShort4High.Select(x => x.Ema).ToList(), 1);
            double emaShort5HighLinearAngle = LinearAngle(emaShort5High.Select(x => x.Ema).ToList(), 1);
            double emaShort6HighLinearAngle = LinearAngle(emaShort6High.Select(x => x.Ema).ToList(), 1);

            //LinearAngle Low
            double emaShort1LowLinearAngle = LinearAngle(emaShort1Low.Select(x => x.Ema).ToList(), 1);
            double emaShort2LowLinearAngle = LinearAngle(emaShort2Low.Select(x => x.Ema).ToList(), 1);
            double emaShort3LowLinearAngle = LinearAngle(emaShort3Low.Select(x => x.Ema).ToList(), 1);
            double emaShort4LowLinearAngle = LinearAngle(emaShort4Low.Select(x => x.Ema).ToList(), 1);
            double emaShort5LowLinearAngle = LinearAngle(emaShort5Low.Select(x => x.Ema).ToList(), 1);
            double emaShort6LowLinearAngle = LinearAngle(emaShort6Low.Select(x => x.Ema).ToList(), 1);

            decimal incPrice = IncreasePrice(price, emaLong1, emaLong2, emaLong3, emaLong4, emaLong5, emaLong6);

            Log.Information("incPriceLong: " + incPrice);

            //Ema Close Last
            Log.Information("emaShort1 = " + emaShort1.LastOrDefault().Ema + " emaLong1 = " + emaLong1.LastOrDefault().Ema + " date:" + emaLong1.LastOrDefault().Date.ToString());
            Log.Information("emaShort2 = " + emaShort2.LastOrDefault().Ema + " emaLong2 = " + emaLong2.LastOrDefault().Ema + " date:" + emaLong2.LastOrDefault().Date.ToString());
            Log.Information("emaShort3 = " + emaShort3.LastOrDefault().Ema + " emaLong3 = " + emaLong3.LastOrDefault().Ema + " date:" + emaLong3.LastOrDefault().Date.ToString());
            Log.Information("emaShort4 = " + emaShort4.LastOrDefault().Ema + " emaLong4 = " + emaLong4.LastOrDefault().Ema + " date:" + emaLong4.LastOrDefault().Date.ToString());
            Log.Information("emaShort5 = " + emaShort5.LastOrDefault().Ema + " emaLong5 = " + emaLong5.LastOrDefault().Ema + " date:" + emaLong5.LastOrDefault().Date.ToString());
            Log.Information("emaShort6 = " + emaShort6.LastOrDefault().Ema + " emaLong6 = " + emaLong6.LastOrDefault().Ema + " date:" + emaLong6.LastOrDefault().Date.ToString());

            //Log.Information("emaMax = " + emaMax.LastOrDefault().Ema + " date:" + emaMax.LastOrDefault().Date.ToString());

            //Ema High Last
            Log.Information("emaShort1High = " + emaShort1High.LastOrDefault().Ema + " emaLong1High = " + emaLong1High.LastOrDefault().Ema + " date:" + emaLong1High.LastOrDefault().Date.ToString());
            Log.Information("emaShort2High = " + emaShort2High.LastOrDefault().Ema + " emaLong2High = " + emaLong2High.LastOrDefault().Ema + " date:" + emaLong2High.LastOrDefault().Date.ToString());
            Log.Information("emaShort3High = " + emaShort3High.LastOrDefault().Ema + " emaLong3High = " + emaLong3High.LastOrDefault().Ema + " date:" + emaLong3High.LastOrDefault().Date.ToString());
            Log.Information("emaShort4High = " + emaShort4High.LastOrDefault().Ema + " emaLong4High = " + emaLong4High.LastOrDefault().Ema + " date:" + emaLong4High.LastOrDefault().Date.ToString());
            Log.Information("emaShort5High = " + emaShort5High.LastOrDefault().Ema + " emaLong5High = " + emaLong5High.LastOrDefault().Ema + " date:" + emaLong5High.LastOrDefault().Date.ToString());
            Log.Information("emaShort6High = " + emaShort6High.LastOrDefault().Ema + " emaLong6High = " + emaLong6High.LastOrDefault().Ema + " date:" + emaLong6High.LastOrDefault().Date.ToString());

            //Ema close ^2 && ^3
            Log.Information("emaShort1[^2].Ema = " + emaShort1[^2].Ema + " date:" + emaShort1[^2].Date.ToString() + " emaShort1[^3].Ema = " + emaShort1[^3].Ema + " date:" + emaShort1[^3].Date.ToString());
            Log.Information("emaShort2[^2].Ema = " + emaShort2[^2].Ema + " date:" + emaShort2[^2].Date.ToString() + " emaShort2[^3].Ema = " + emaShort2[^3].Ema + " date:" + emaShort2[^3].Date.ToString());
            Log.Information("emaShort3[^2].Ema = " + emaShort3[^2].Ema + " date:" + emaShort3[^2].Date.ToString() + " emaShort3[^3].Ema = " + emaShort3[^3].Ema + " date:" + emaShort3[^3].Date.ToString());
            Log.Information("emaShort4[^2].Ema = " + emaShort4[^2].Ema + " date:" + emaShort4[^2].Date.ToString() + " emaShort4[^3].Ema = " + emaShort4[^3].Ema + " date:" + emaShort4[^3].Date.ToString());
            Log.Information("emaShort5[^2].Ema = " + emaShort5[^2].Ema + " date:" + emaShort5[^2].Date.ToString() + " emaShort5[^3].Ema = " + emaShort5[^3].Ema + " date:" + emaShort5[^3].Date.ToString());
            Log.Information("emaShort6[^2].Ema = " + emaShort6[^2].Ema + " date:" + emaShort6[^2].Date.ToString() + " emaShort6[^3].Ema = " + emaShort6[^3].Ema + " date:" + emaShort6[^3].Date.ToString());

            Log.Information("emaLong1[^2].Ema = " + emaLong1[^2].Ema + " date:" + emaLong1[^2].Date.ToString() + " emaLong1[^3].Ema = " + emaLong1[^3].Ema + " date:" + emaLong1[^3].Date.ToString());
            Log.Information("emaLong2[^2].Ema = " + emaLong2[^2].Ema + " date:" + emaLong2[^2].Date.ToString() + " emaLong2[^3].Ema = " + emaLong2[^3].Ema + " date:" + emaLong2[^3].Date.ToString());
            Log.Information("emaLong3[^2].Ema = " + emaLong3[^2].Ema + " date:" + emaLong3[^2].Date.ToString() + " emaLong3[^3].Ema = " + emaLong3[^3].Ema + " date:" + emaLong3[^3].Date.ToString());
            Log.Information("emaLong4[^2].Ema = " + emaLong4[^2].Ema + " date:" + emaLong4[^2].Date.ToString() + " emaLong4[^3].Ema = " + emaLong4[^3].Ema + " date:" + emaLong4[^3].Date.ToString());
            Log.Information("emaLong5[^2].Ema = " + emaLong5[^2].Ema + " date:" + emaLong5[^2].Date.ToString() + " emaLong5[^3].Ema = " + emaLong5[^3].Ema + " date:" + emaLong5[^3].Date.ToString());
            Log.Information("emaLong6[^2].Ema = " + emaLong6[^2].Ema + " date:" + emaLong6[^2].Date.ToString() + " emaLong6[^3].Ema = " + emaLong6[^3].Ema + " date:" + emaLong6[^3].Date.ToString());

            //Ema Linear Angles Close
            Log.Information("emaShort1LinearAngle(1) = " + emaShort1LinearAngle);
            Log.Information("emaShort2LinearAngle(1) = " + emaShort2LinearAngle);
            Log.Information("emaShort3LinearAngle(1) = " + emaShort3LinearAngle);
            Log.Information("emaShort4LinearAngle(1) = " + emaShort4LinearAngle);
            Log.Information("emaShort5LinearAngle(1) = " + emaShort5LinearAngle);
            Log.Information("emaShort6LinearAngle(1) = " + emaShort6LinearAngle);

            //Ema Linear Angles(2) Close
            Log.Information("emaShort1LinearAngle(2) = " + emaShort1LinearAngleInc);
            Log.Information("emaShort2LinearAngle(2) = " + emaShort2LinearAngleInc);
            Log.Information("emaShort3LinearAngle(2) = " + emaShort3LinearAngleInc);
            Log.Information("emaShort4LinearAngle(2) = " + emaShort4LinearAngleInc);
            Log.Information("emaShort5LinearAngle(2) = " + emaShort5LinearAngleInc);
            Log.Information("emaShort6LinearAngle(2) = " + emaShort6LinearAngleInc);

            //Ema Linear Angles High
            Log.Information("emaShort1HighLinearAngle(1) = " + emaShort1HighLinearAngle);
            Log.Information("emaShort2HighLinearAngle(1) = " + emaShort2HighLinearAngle);
            Log.Information("emaShort3HighLinearAngle(1) = " + emaShort3HighLinearAngle);
            Log.Information("emaShort4HighLinearAngle(1) = " + emaShort4HighLinearAngle);
            Log.Information("emaShort5HighLinearAngle(1) = " + emaShort5HighLinearAngle);
            Log.Information("emaShort6HighLinearAngle(1) = " + emaShort6HighLinearAngle);

            //Ema Linear Angles Low
            Log.Information("emaShort1LowLinearAngle(1) = " + emaShort1LowLinearAngle);
            Log.Information("emaShort2LowLinearAngle(1) = " + emaShort2LowLinearAngle);
            Log.Information("emaShort3LowLinearAngle(1) = " + emaShort3LowLinearAngle);
            Log.Information("emaShort4LowLinearAngle(1) = " + emaShort4LowLinearAngle);
            Log.Information("emaShort5LowLinearAngle(1) = " + emaShort5LowLinearAngle);
            Log.Information("emaShort6LowLinearAngle(1) = " + emaShort6LowLinearAngle);

            //Ema Angles 
            //Log.Information("emaShort1Angle(1) = " + emaShort1Angle);
            //Log.Information("emaShort2Angle(1) = " + emaShort2Angle);
            //Log.Information("emaShort3Angle(1) = " + emaShort3Angle);
            //Log.Information("emaShort4Angle(1) = " + emaShort4Angle);
            //Log.Information("emaShort5Angle(1) = " + emaShort5Angle);
            //Log.Information("emaShort6Angle(1) = " + emaShort6Angle);



            if //to Long
                (// Преверка на зигзаг
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

                &&// Проверка на пересечение длинных EMA
                emaShort6.LastOrDefault().Ema > emaLong1.LastOrDefault().Ema
                &&
                emaShort6.LastOrDefault().Ema > emaLong6.LastOrDefault().Ema


                &&//Все линии по порядку
                emaShort1.LastOrDefault().Ema > emaShort2.LastOrDefault().Ema
                &&
                emaShort2.LastOrDefault().Ema > emaShort3.LastOrDefault().Ema
                &&
                emaShort3.LastOrDefault().Ema > emaShort4.LastOrDefault().Ema
                &&
                emaShort4.LastOrDefault().Ema > emaShort5.LastOrDefault().Ema
                &&
                emaShort5.LastOrDefault().Ema > emaShort6.LastOrDefault().Ema



                &&// Проверка углов по Close
                emaShort1LinearAngle > 0
                &&
                emaShort6LinearAngle > 0

                &&// Проверка углов по High
                emaShort1HighLinearAngle > 2

                //&&// Проверка на резкий скачек цены
                //incPrice < 0.5m

               )
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeTarget.toLong");
                Log.Information("bestAsk = " + bestAsk);
                Log.Information("bestBid = " + bestBid);
                return TradeTarget.toLong;
            }
            else if // to short
                (
                // Преверка на зигзаг
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


                &&// Проверка на пересечение длинных EMA
                emaShort6.LastOrDefault().Ema < emaLong1.LastOrDefault().Ema
                &&
                emaShort6.LastOrDefault().Ema < emaLong6.LastOrDefault().Ema

                && //Все линии по порядку
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


                && // Проверка углов по Close
                emaShort1LinearAngle < 0
                &&
                emaShort6LinearAngle < 0

                &&// Проверка углов по Low
                emaShort1LowLinearAngle < 0

                //&&// Проверка на резкий скачек цены
                //incPrice > -0.5m
                )
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeTarget.toShort");
                Log.Information("bestAsk = " + bestAsk);
                Log.Information("bestBid = " + bestBid);
                return TradeTarget.toShort;
            }
            else if // from Long
                (
                    (
                    PositionEntryRangeForLong(emaShort6, emaLong1, emaLong2, emaLong3, emaLong4, emaLong5, emaLong6, 15) == true
                        &&
                    price < emaLong1.LastOrDefault().Ema
                                    ||
                    price < emaLong2.LastOrDefault().Ema
                                    ||
                    price < emaLong3.LastOrDefault().Ema
                                    ||
                    price < emaLong4.LastOrDefault().Ema
                                    ||
                    price < emaLong5.LastOrDefault().Ema
                                    ||
                    price < emaLong6.LastOrDefault().Ema
                    )
                ||
                    (
                    PositionEntryRangeForLong(emaShort6, emaLong1, emaLong2, emaLong3, emaLong4, emaLong5, emaLong6, 15) == false
                    &&
                    price < emaLong1.LastOrDefault().Ema
                                    &&
                    price < emaLong2.LastOrDefault().Ema
                                    &&
                    price < emaLong3.LastOrDefault().Ema
                                    &&
                    price < emaLong4.LastOrDefault().Ema
                                    &&
                    price < emaLong5.LastOrDefault().Ema
                                    &&
                    price < emaLong6.LastOrDefault().Ema
                    )
                 ||
                    (
                    emaShort1HighLinearAngle < -50
                    )
                )
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeTarget.fromLong");
                Log.Information("bestAsk = " + bestAsk);
                Log.Information("bestBid = " + bestBid);
                return TradeTarget.fromLong;
            }

            else if // from Short
                                (
                    (
                    PositionEntryRangeForShort(emaShort6, emaLong1, emaLong2, emaLong3, emaLong4, emaLong5, emaLong6, 15) == true
                        &&
                    price > emaLong1.LastOrDefault().Ema
                                    ||
                    price > emaLong2.LastOrDefault().Ema
                                    ||
                    price > emaLong3.LastOrDefault().Ema
                                    ||
                    price > emaLong4.LastOrDefault().Ema
                                    ||
                    price > emaLong5.LastOrDefault().Ema
                                    ||
                    price > emaLong6.LastOrDefault().Ema
                    )
                ||
                    (
                    PositionEntryRangeForShort(emaShort6, emaLong1, emaLong2, emaLong3, emaLong4, emaLong5, emaLong6, 15) == false
                    &&
                    price > emaLong1.LastOrDefault().Ema
                                    &&
                    price > emaLong2.LastOrDefault().Ema
                                    &&
                    price > emaLong3.LastOrDefault().Ema
                                    &&
                    price > emaLong4.LastOrDefault().Ema
                                    &&
                    price > emaLong5.LastOrDefault().Ema
                                    &&
                    price > emaLong6.LastOrDefault().Ema
                    )
                 ||
                    (
                    emaShort1LowLinearAngle > 50
                    )
                )
            //(
            //price > emaLong1.LastOrDefault().Ema
            //                &&
            //price > emaLong1.LastOrDefault().Ema
            //                &&
            //price > emaLong1.LastOrDefault().Ema
            //                &&
            //price > emaLong1.LastOrDefault().Ema
            //                &&
            //price > emaLong1.LastOrDefault().Ema
            //                &&
            //price > emaLong1.LastOrDefault().Ema


            //)
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeTarget.fromShort"); 
                Log.Information("bestAsk = " + bestAsk);
                Log.Information("bestBid = " + bestBid);
                return TradeTarget.fromShort;
            }
            else
            {
                Log.Information("Stop GmmaSignal. Figi: " + candleList.Figi + " TradeTarget.notTrading");
                Log.Information("bestAsk = " + bestAsk);
                Log.Information("bestBid = " + bestBid);
                return TradeTarget.notTrading;
            }
        }

        private decimal IncreasePrice(decimal price, List<EmaResult> emaLong1, List<EmaResult> emaLong2, List<EmaResult> emaLong3, List<EmaResult> emaLong4, List<EmaResult> emaLong5, List<EmaResult> emaLong6)
        {
            decimal? deltaLongEma = (emaLong1.LastOrDefault().Ema + emaLong2.LastOrDefault().Ema + emaLong3.LastOrDefault().Ema + emaLong4.LastOrDefault().Ema + emaLong5.LastOrDefault().Ema + emaLong6.LastOrDefault().Ema) / 6;
            decimal incPrice = (price * 100) / (decimal)deltaLongEma - 100;
            return incPrice;
        }

        private bool PositionEntryRangeForLong(List<EmaResult> emaShort6, List<EmaResult> emaLong1, List<EmaResult> emaLong2, List<EmaResult> emaLong3, List<EmaResult> emaLong4, List<EmaResult> emaLong5, List<EmaResult> emaLong6, int count)
        {
            Log.Information("Start PositionEntryRangeForLong");
            for (int i = 1; i <= count; i++)
            {
                if
                    (
                    emaShort6[^i].Ema < emaLong1[^i].Ema
                    ||
                    emaShort6[^i].Ema < emaLong2[^i].Ema
                    ||
                    emaShort6[^i].Ema < emaLong3[^i].Ema
                    ||
                    emaShort6[^i].Ema < emaLong4[^i].Ema
                    ||
                    emaShort6[^i].Ema < emaLong5[^i].Ema
                    ||
                    emaShort6[^i].Ema < emaLong6[^i].Ema
                    )
                {
                    Log.Information("emaShort6(" + i + ") = " + emaShort6[^i].Ema + " emaLong1(" + i + ") = " + emaLong1[^i].Ema + " emaLong2(" + i + ") = " + emaLong2[^i].Ema + " emaLong3(" + i + ") = " + emaLong3[^i].Ema + " emaLong4(" + i + ") = " + emaLong4[^i].Ema + " emaLong5(" + i + ") = " + emaLong5[^i].Ema + " emaLong6(" + i + ") = " + emaLong6[^i].Ema);
                    Log.Information("Stop PositionEntryRangeForLong");
                    return true;
                }
            }
            Log.Information("Stop PositionEntryRangeForLong");
            return false;
        }

        private bool PositionEntryRangeForShort(List<EmaResult> emaShort6, List<EmaResult> emaLong1, List<EmaResult> emaLong2, List<EmaResult> emaLong3, List<EmaResult> emaLong4, List<EmaResult> emaLong5, List<EmaResult> emaLong6, int count)
        {
            Log.Information("Stop PositionEntryRangeForShort");
            for (int i = 1; i <= count; i++)
            {
                if
                    (
                    emaShort6[^i].Ema > emaLong1[^i].Ema
                    ||
                    emaShort6[^i].Ema > emaLong2[^i].Ema
                    ||
                    emaShort6[^i].Ema > emaLong3[^i].Ema
                    ||
                    emaShort6[^i].Ema > emaLong4[^i].Ema
                    ||
                    emaShort6[^i].Ema > emaLong5[^i].Ema
                    ||
                    emaShort6[^i].Ema > emaLong6[^i].Ema
                    )
                {
                    Log.Information("emaShort6(" + i + ") = " + emaShort6[^i].Ema + " emaLong1(" + i + ") = " + emaLong1[^i].Ema + " emaLong2(" + i + ") = " + emaLong2[^i].Ema + " emaLong3(" + i + ") = " + emaLong3[^i].Ema + " emaLong4(" + i + ") = " + emaLong4[^i].Ema + " emaLong5(" + i + ") = " + emaLong5[^i].Ema + " emaLong6(" + i + ") = " + emaLong6[^i].Ema);
                    Log.Information("Stop PositionEntryRangeForShort");
                    return true;
                }
            }
            Log.Information("Stop PositionEntryRangeForShort");
            return false;
        }
    }
}
