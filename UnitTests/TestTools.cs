using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CellularAutomata;
using System.Numerics;

namespace UnitTests
{
    [TestClass]
    public class TestTools
    {
        [TestMethod]
        public void LargeDecimalToStringBase_TestConversionOfNumberToBase_36()
        {
            BigInteger numberToTest;
            BigInteger.TryParse("1537550572281", out numberToTest);
            string resultExpected = "JMC9WUEX";
            string resultActual = Tools.LargeDecimalToStringBase(numberToTest, 36);

            Assert.AreEqual(resultExpected, resultActual, "The conversion was not accurate.");
        }

        [TestMethod]
        public void LargeDecimalToStringBase_TestConversionOfVeryLargeNumberToBase_36()
        {
            BigInteger numberToTest;
            BigInteger.TryParse("300377737362090600663731055950616632373", out numberToTest);
            string resultExpected = "DDMJQ6ROGBS63DRF9EBKGG7XX";
            string resultActual = Tools.LargeDecimalToStringBase(numberToTest, 36);

            Assert.AreEqual(resultExpected, resultActual, "The conversion was not accurate.");
        }

        [TestMethod]
        public void LargeArbitraryBaseToDecimal_TestConversionOfBase_36_ToBigInt()
        {
            string numberToTest = "DDMJQ6ROGBS63DRF9EBKGG7XX";
            BigInteger resultExpected;
            BigInteger.TryParse("300377737362090600663731055950616632373", out resultExpected);
            BigInteger resultActual = Tools.LargeArbitraryToDecimalSystem(numberToTest, 36);

            Assert.AreEqual(resultExpected, resultActual, "The conversion was not accurate.");
        }

        [TestMethod]
        public void ConvertFromBigIntToStringBackToSameBigIntegerValue()
        {
            BigInteger numberToTest;
            BigInteger.TryParse("300377737362090600663731055950616632373", out numberToTest);
            string numToStringResult = Tools.LargeDecimalToStringBase(numberToTest, 36);
            BigInteger stringToNumResult = Tools.LargeArbitraryToDecimalSystem(numToStringResult, 36);

            Assert.AreEqual(numberToTest, stringToNumResult, "The number was not converted back cleanly.");
        }

        [TestMethod]
        public void ConvertFromStringToBigIntBackToSameString()
        {
            string numberToTest = "DDMJQ6ROGBS63DRF9EBKGG7XX";
            BigInteger stringToNumResult = Tools.LargeArbitraryToDecimalSystem(numberToTest, 36);
            string numToStringResult = Tools.LargeDecimalToStringBase(stringToNumResult, 36);

            Assert.AreEqual(numberToTest, numToStringResult, "The string was not converted back cleanly.");
        }
    }
}
