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
            Assert.Equal(AngleCalc(1, 2, 1), x);
            Assert.Equal(AngleCalc(-1, -2), mx);
            Assert.Equal(AngleCalc(50, 52, 2), x);
            Assert.Equal(AngleCalc(50, 50), 0);
        }

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

        [Fact]
        public void ÑorrelationPearsonTest()
        {
            decimal expR = 0.5298089018901743912249653292m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (43m, 99m), (21m, 65m), (25m, 79m), (42m, 75m), (57m, 87m), (59m, 81m) };
            Assert.Equal(ÑorrelationPearson(testData), expR);
        }

        [Fact]
        public void ÑorrelationPearsonTest1()
        {
            decimal expR = 1m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (2m, 2m), (3m, 3m), (4m, 4m), (5m, 5m), (6m, 6m) };
            Assert.Equal(ÑorrelationPearson(testData), expR);
        }

        [Fact]
        public void ÑorrelationPearsonTest2()
        {
            decimal expR = 0.5m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (0m, 0m), (-1m, -1m), (0m, -1m), (1m, -1m), (1, 0) };
            Assert.Equal(ÑorrelationPearson(testData), expR);
        }
        [Fact]
        public void ÑorrelationPearsonTest3()
        {
            decimal expR = -1;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 6m), (2m, 5m), (3m, 4m), (4m, 3m), (5m, 2m), (6m, 1m) };
            Assert.Equal(ÑorrelationPearson(testData), expR);
        }

        [Fact]
        public void ÑorrelationPearsonTest4()
        {
            decimal expR = 1m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (6m, 6m), (5m, 5m), (4m, 4m), (3m, 3m), (2m, 2m), (1m, 1m) };
            Assert.Equal(ÑorrelationPearson(testData), expR);
        }
        [Fact]
        public void ÑorrelationPearsonTest5()
        {
            decimal expR = 1m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (3m, 3m), (5m, 5m), (4m, 4m), (5m, 5m), (6m, 6m) };
            Assert.Equal(ÑorrelationPearson(testData), expR);
        }
    }
}
