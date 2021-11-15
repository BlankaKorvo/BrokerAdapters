using Analysis.Signals.Helpers;
using System;
using System.Collections.Generic;
using Xunit;

namespace Analysis.Test
{
    public class IndicatorSignalsHelperTests : IndicatorSignalsHelper
    {


        [Fact]
        public void LinearAngleTestPositiv()
        {
            List<decimal> testValues = new List<decimal> { 1, 2, 3};
            Assert.Equal(45m, LinearAngle(testValues));
        }

        [Fact]
        public void LinearAngleTestPositiv1()
        {
            List<decimal> testValues = new List<decimal> { 5, 4, 3 };
            Assert.Equal(LinearAngle(testValues), -45m);
        }

        [Fact]
        public void LinearAngleTestPositiv3()
        {
            List<decimal?> testValues = new List<decimal?> { 9999, 2, 3, 4 };
            Assert.Equal(45m, LinearAngle(testValues, 3));
        }

        [Fact]
        public void LinearAngleTestPositiv4()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, 5, 4, 3 };
            Assert.Equal(LinearAngle(testValues, 3), -45m);
        }

        [Fact]
        public void LinearAngleTestPositiv5()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, 1, 0, -1 };
            Assert.Equal(LinearAngle(testValues, 3), -45m);
        }

        [Fact]
        public void LinearAngleTestPositiv6()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, -1, 0, 1 };
            Assert.Equal(45, LinearAngle(testValues, 3));
        }

        [Fact]
        public void LinearAngleTestNegativ1()
        {
            List<decimal?> testValues = new List<decimal?> { 1000, -1, 0, 1 };
            ArgumentException exception = Assert.Throws<ArgumentException>((() => LinearAngle(testValues, 1)));
            Assert.Equal("AnglesCount <2", exception.Message);
        }
        [Fact]
        public void LinearAngleTestNegativ2()
        {
            List<decimal?> testValues = new List<decimal?> { 1000 };
            ArgumentException exception = Assert.Throws<ArgumentException>((() => LinearAngle(testValues, 2)));
            Assert.Equal("Values.Count <2", exception.Message);
        }
        [Fact]
        public void LinearAngleTestNegativ3()
        {
            List<decimal?> testValues = new List<decimal?> { 54, 1000, null, 0, 1 };
            ArgumentException exception = Assert.Throws<ArgumentException>((() => LinearAngle(testValues, 3)));
            Assert.Equal("Detected nulls in the list. Position: 2", exception.Message);
        }


    }
}
