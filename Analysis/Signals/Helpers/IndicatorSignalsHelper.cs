using MarketDataModules;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqStatistics;
using MarketDataModules.Models.Candles;

namespace TradingAlgorithms.IndicatorSignals.Helpers
{
    public class IndicatorSignalsHelper
    {

        public List<Tuple<decimal, decimal>> ToTuples(List<decimal> values)
        {
            Log.Information("Start ToTuples");
            Log.Information("values.Count = " + values.Count);
            List<Tuple<decimal, decimal>> tupless = new List<Tuple<decimal, decimal>> { };
            int ind = 0;
            foreach (var item in values)
            {
                ind++;
                Tuple<decimal, decimal> tuple1 = new Tuple<decimal, decimal>(ind, item);
                tupless.Add(tuple1);
            }
            Log.Information("tupless.Count = " + tupless.Count);
            Log.Information("Stop ToTuples");
            return tupless;
        }

        public double LinearAngle(List<decimal> values)
        {
            Log.Information("Start LinearAngle");
            List<Tuple<decimal, decimal>> tuples = ToTuples(values);
            decimal M = (decimal)tuples.LeastSquares().M;
            decimal B = (decimal)tuples.LeastSquares().B;
            decimal y1 = M * 1 + B;
            decimal y2 = M * 2 + B;
            Log.Information("M: " + M);
            Log.Information("B: " + B);
            Log.Information("y1: " + y1);
            Log.Information("y2: " + y2);

            var result = AngleCalc(y1, y2);
            Log.Information("result: " + result);
            Log.Information("Stop LinearDeltaDegreeAngle");
            return result;
        }

        public double LinearAngle(List<decimal> values, int anglesCount)
        {
            Log.Information("Start LinearAngle");
            Log.Information("values.Count: " + values.Count);
            Log.Information("anglesCount: " + anglesCount);
            List<decimal> calculatedValues = values.Skip(values.Count - (anglesCount + 1)).ToList();
            foreach (var item in calculatedValues)
            { Log.Information(item.ToString()); }

            List<Tuple<decimal, decimal>> tuples = ToTuples(calculatedValues);
            decimal M = (decimal)tuples.LeastSquares().M;
            decimal B = (decimal)tuples.LeastSquares().B;
            decimal y1 = M * 1 + B;
            decimal y2 = M * 2 + B;
            var result = AngleCalc(y1, y2);
            Log.Information("M: " + M);
            Log.Information("B: " + B);
            Log.Information("y1: " + y1);
            Log.Information("y2: " + y2);
            Log.Information("result: " + result);
            Log.Information("Stop LinearDeltaDegreeAngle");
            return result;
        }

        public double LinearAngle(List<decimal?> values, int anglesCount)
        {
            Log.Information("Start LinearAngle");
            Log.Information("values.Count: " + values.Count);
            Log.Information("anglesCount: " + anglesCount);

            //Проверка на то, что списка значений в конце изначального списка не содержит Null
            if (values[^anglesCount] == null)
            {
                throw new Exception("anglesCount is more, then notnulable List values count");
            }
            else
            {
                List<decimal?> calculatedValuesN = values.Skip(values.Count - (anglesCount + 1)).ToList();
                //Приводим список значений списка к типу decimal
                List<decimal> calculatedValues = calculatedValuesN.Select(x => (decimal)x).ToList();
                foreach (var item in calculatedValues)
                { Log.Information(item.ToString()); }

                List<Tuple<decimal, decimal>> tuples = ToTuples(calculatedValues);
                decimal M = (decimal)tuples.LeastSquares().M;
                decimal B = (decimal)tuples.LeastSquares().B;
                decimal y1 = M * 1 + B;
                decimal y2 = M * 2 + B;
                var result = AngleCalc(y1, y2);
                Log.Information("M: " + M);
                Log.Information("B: " + B);
                Log.Information("y1: " + y1);
                Log.Information("y2: " + y2);
                Log.Information("result: " + result);
                Log.Information("Stop LinearDeltaDegreeAngle");
                return result;
            }
        }



        public double DeltaDegreeAngle(List<decimal> values)
        {
            Log.Information("Start DeltaDegreeAngle");
            var countDelta = values.Count;
            double summ = 0;
            for (int i = 1; i < countDelta; i++)
            {
                summ += AngleCalc(values[i], values[i - 1]);
                //summ = AngleCalc(values, summ, i);
            }
            double averageAngles = summ / (countDelta - 1);
            Log.Information("Average Angles: " + averageAngles.ToString());
            Log.Information("Stop DeltaDegreeAngle");
            return averageAngles;
        }



        //private static double AngleCalc(List<decimal?> values, double summ, int i)
        //{
        //    double deltaLeg = Convert.ToDouble(values[i] - values[i - 1]);
        //    double legDifference = Math.Atan(deltaLeg);
        //    double angle = legDifference * (180 / Math.PI);
        //    Log.Information("Angle: " + angle.ToString());
        //    summ += angle;
        //    return summ;
        //}

        private static double AngleCalc(decimal value1, decimal value2)
        {
            Log.Information("Start AngleCalc");
            Log.Information("value1: " + value1);
            Log.Information("value2: " + value2);
            double deltaLeg = Convert.ToDouble(value2 -value1);
            double legDifference = Math.Atan(deltaLeg);
            double angle = legDifference * (180 / Math.PI);
            Log.Information("Angle: " + angle.ToString());
            Log.Information("Stop AngleCalc");

            return angle;
        }


        internal double DeltaDegreeAngle(List<decimal?> values, int anglesCount)
        {
            Log.Information("Start DeltaDegreeAngle");
            List<decimal?> calculatedValues = values.Skip(values.Count - (anglesCount + 1)).ToList();
            var countDelta = calculatedValues.Count;
            double summ = 0;
            for (int i = 1; i < countDelta; i++)
            {
                double deltaLeg = Convert.ToDouble(calculatedValues[i] - calculatedValues[i - 1]);
                double legDifference = Math.Atan(deltaLeg);
                double angle = legDifference * (180 / Math.PI);
                Log.Information("Angle: " + angle.ToString());
                summ += angle;
            }
            double averageAngles = summ / (countDelta - 1);
            Log.Information("Average Angles: " + averageAngles.ToString());
            Log.Information("Stop DeltaDegreeAngle");
            return averageAngles;
        }
        //internal double DeltaDegreeAngleEx(List<decimal?> values, int anglesCount)
        //{
        //    Log.Information("Start DeltaDegreeAngle");
        //    List<decimal?> calculatedValues = values.Skip(values.Count - (anglesCount + 1)).ToList();
        //    var countDelta = calculatedValues.Count;
        //    double summ = 0;
        //    for (int i = 1; i < countDelta; i++)
        //    {
        //        double deltaLeg = Convert.ToDouble(calculatedValues[i] - calculatedValues[i - 1]);
        //        double legDifference = Math.Atan(deltaLeg);
        //        double angle = legDifference * (180 / Math.PI) * i;
        //        Log.Information("Angle: " + angle.ToString());
        //        summ += angle;
        //    }
        //    double averageAngles = summ / (countDelta - 1);
        //    Log.Information("Average Angles: " + averageAngles.ToString());
        //    Log.Information("Stop DeltaDegreeAngle");
        //    return averageAngles;
        //}

        internal decimal? MaxLastDegree(List<decimal?> values, int Count)
        {
            Log.Information("Start MaxCloseDegree");
            List<decimal?> calculatedValues = values.Skip(values.Count - (Count + 1)).ToList();
            foreach (var item in calculatedValues)
            {
                Log.Information("calculatedValue= " + item);
            }
            decimal? maxCalculatedValues = calculatedValues.Max();
            Log.Information("maxCalculatedValues= " + maxCalculatedValues);
            Log.Information("calculatedValues.Last()= " + calculatedValues.Last());
            decimal? diff = calculatedValues.Last() - maxCalculatedValues;
            Log.Information("diff= " + diff);
            Log.Information("Stop MaxCloseDegree");
            return diff;
        }

        internal bool IsCandleGreen(CandleStructure candleStructure)
        {
            if (candleStructure.Open <= candleStructure.Close)
                return true;
            else
                return false;
        }
    }
}
