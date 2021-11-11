using Analysis.Signals.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Analysis.Test
{
    public class MatanTests : Matan
    {
        [Fact]
        public void AngleCalcTest()
        {
            decimal x = 45;
            decimal mx = -45;
            Assert.Equal(AngleCalc(0, 1, 1), x);
            Assert.Equal(AngleCalc(1, 2, 1), x);
            Assert.Equal(AngleCalc(-1, -2), mx);
            Assert.Equal(AngleCalc(50, 52, 2), x);
            Assert.Equal(AngleCalc(50, 50), 0);
        }
        [Fact]
        public void СorrelationPearsonTest0()
        {
            decimal expR = 0m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (1m, 1m), (1m, 1m), (1m, 1m), (1m, 1m), (1m, 1m) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }
        [Fact]
        public void СorrelationPearsonTest()
        {
            decimal expR = 0.5298089018901743912249653292m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (43m, 99m), (21m, 65m), (25m, 79m), (42m, 75m), (57m, 87m), (59m, 81m) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }

        [Fact]
        public void СorrelationPearsonTest1()
        {
            decimal expR = 1m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (2m, 2m), (3m, 3m), (4m, 4m), (5m, 5m), (6m, 6m) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }

        [Fact]
        public void СorrelationPearsonTest2()
        {
            decimal expR = 0.5m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (0m, 0m), (-1m, -1m), (0m, -1m), (1m, -1m), (1, 0) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }
        [Fact]
        public void СorrelationPearsonTest3()
        {
            decimal expR = -1;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 6m), (2m, 5m), (3m, 4m), (4m, 3m), (5m, 2m), (6m, 1m) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }

        [Fact]
        public void СorrelationPearsonTest4()
        {
            decimal expR = 1m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (6m, 6m), (5m, 5m), (4m, 4m), (3m, 3m), (2m, 2m), (1m, 1m) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }
        [Fact]
        public void СorrelationPearsonTest5()
        {
            decimal expR = 1m;
            List<(decimal, decimal)> testData = new List<(decimal, decimal)>() { (1m, 1m), (3m, 3m), (5m, 5m), (4m, 4m), (5m, 5m), (6m, 6m) };
            Assert.Equal(СorrelationPearson(testData), expR);
        }
    }
}
