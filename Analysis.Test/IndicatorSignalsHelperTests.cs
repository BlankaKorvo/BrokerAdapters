using Analysis.Signals.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Analysis.Test
{
    public class IndicatorSignalsHelperTests : IndicatorSignalsHelper
    {
        [Fact]
        public void AngleCalcTest()
        {
            double x = 45;
            double mx = -45;
            Assert.Equal(AngleCalc(0, 1, 1), x);
            Assert.Equal(AngleCalc(-1, -2), mx);
            Assert.Equal(AngleCalc(50, 52, 2), x);
            Assert.Equal(AngleCalc(50, 50), 0);
        }

        [Fact]
        public void DeltaDegreeValuesTest()
        {
            List<decimal?> testValues = new List<decimal?> { 30, 6, 20, 15, 25 };
            Assert.Equal(DeltaDegreeValues(testValues), -36);
        }
    }
}
