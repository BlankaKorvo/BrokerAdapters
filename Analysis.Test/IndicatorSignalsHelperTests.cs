using Analysis.Signals.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Analysis.Test
{
    public class IndicatorSignalsHelperTests : IndicatorSignalsHelper
    {


        [Fact]
        public void LinearAngleTest()
        {
            List<decimal> testValues = new List<decimal> { 1, 2, 3};
            Assert.Equal(LinearAngle(testValues), 45);
        }

        [Fact]
        public void LinearAngleTest1()
        {
            List<decimal> testValues = new List<decimal> { 5, 4, 3 };
            Assert.Equal(LinearAngle(testValues), -45);
        }

        [Fact]
        public void LinearAngleTestP()
        {
            List<decimal?> testValues = new List<decimal?> { 9999, 2, 3, 4 };
            Assert.Equal(LinearAngle(testValues, 3), 45);
        }

        [Fact]
        public void LinearAngleTestP1()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, 5, 4, 3 };
            Assert.Equal(LinearAngle(testValues, 3), -45);
        }

        [Fact]
        public void LinearAngleTestP2()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, 1, 0, -1 };
            Assert.Equal(LinearAngle(testValues, 3), -45);
        }

        [Fact]
        public void LinearAngleTestP3()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, -1, 0, 1 };
            Assert.Equal(LinearAngle(testValues, 3), 45);
        }

        
    }
}
