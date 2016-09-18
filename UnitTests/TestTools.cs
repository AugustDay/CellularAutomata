using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using CellularAutomata;
using CellularAutomata.OneDimensionalCA;

namespace UnitTests
{
    [TestClass]
    public class TestTools
    {
        [TestMethod]
        public void Tools_LargeDecimalToStringBase_TestConversionOfNumberToBase_36()
        {
            BigInteger numberToTest;
            BigInteger.TryParse("1537550572281", out numberToTest);
            string resultExpected = "JMC9WUEX";
            string resultActual = Tools.LargeDecimalToStringBase(numberToTest, 36);

            Assert.AreEqual(resultExpected, resultActual, "The conversion was not accurate.");
        }

        [TestMethod]
        public void Tools_LargeDecimalToStringBase_TestConversionOfVeryLargeNumberToBase_36()
        {
            BigInteger numberToTest;
            BigInteger.TryParse("300377737362090600663731055950616632373", out numberToTest);
            string resultExpected = "DDMJQ6ROGBS63DRF9EBKGG7XX";
            string resultActual = Tools.LargeDecimalToStringBase(numberToTest, 36);

            Assert.AreEqual(resultExpected, resultActual, "The conversion was not accurate.");
        }

        [TestMethod]
        public void Tools_LargeArbitraryBaseToDecimal_TestConversionOfBase_36_ToBigInt()
        {
            string numberToTest = "DDMJQ6ROGBS63DRF9EBKGG7XX";
            BigInteger resultExpected;
            BigInteger.TryParse("300377737362090600663731055950616632373", out resultExpected);
            BigInteger resultActual = Tools.LargeArbitraryToDecimalSystem(numberToTest, 36);

            Assert.AreEqual(resultExpected, resultActual, "The conversion was not accurate.");
        }

        [TestMethod]
        public void Tools_ConvertFromBigIntToStringBackToSameBigIntegerValue()
        {
            BigInteger numberToTest;
            BigInteger.TryParse("300377737362090600663731055950616632373", out numberToTest);
            string numToStringResult = Tools.LargeDecimalToStringBase(numberToTest, 36);
            BigInteger stringToNumResult = Tools.LargeArbitraryToDecimalSystem(numToStringResult, 36);

            Assert.AreEqual(numberToTest, stringToNumResult, "The number was not converted back cleanly.");
        }

        [TestMethod]
        public void Tools_ConvertFromStringToBigIntBackToSameString()
        {
            string numberToTest = "DDMJQ6ROGBS63DRF9EBKGG7XX";
            BigInteger stringToNumResult = Tools.LargeArbitraryToDecimalSystem(numberToTest, 36);
            string numToStringResult = Tools.LargeDecimalToStringBase(stringToNumResult, 36);

            Assert.AreEqual(numberToTest, numToStringResult, "The string was not converted back cleanly.");
        }

        [TestMethod]
        public void Tools_MakeAutomataFromCode_NoArgumentsGivenReturnsDefault()
        {
            //this won't be true once I implement "dynamic defaults" without some reworking
            DateTime now = DateTime.Now; //uses test-specific seed values to make SetRandomRule() consistent.
            Tools.Rand = new Random((int)now.Ticks);
            Simulator1D resultExpected = new Simulator1D();
            Tools.Rand = new Random((int)now.Ticks);
            Simulator1D resultActual = Tools.MakeAutomataFromCode(""); 
            Assert.AreEqual(resultExpected, resultActual, "The Automata generated were not equal.");
        }

        [TestMethod]
        public void Tools_MakeAutomataFromCode_AllArgumentsGivenCorrectOutput()
        {
            string testInput = "k=3 n={-1,0,1} r=1537550572281_10 b=400";
            BigInteger numberExpected;
            BigInteger.TryParse("1537550572281", out numberExpected);
            int sizeExpected = 400;
            int statesExpected = 3;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesExpected = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
            Imager1D imagerExpected = new Imager1D(rulesExpected);
            Simulator1D resultExpected = new Simulator1D(rulesExpected, imagerExpected, sizeExpected, Simulator1D.EdgeSettings.HardEdges);

            Simulator1D resultActual = Tools.MakeAutomataFromCode(testInput);
            Assert.AreEqual(resultExpected, resultActual, "The Automata generated were not equal.");
        }

        [TestMethod]
        public void Tools_MakeAutomataFromCode_AllArgsAccurateAndBOARDSIZECorrectDefaultOutput()
        {
            string testInput = "k=3 n={-1,0,1} r=1537550572281_10";
            BigInteger numberExpected;
            BigInteger.TryParse("1537550572281", out numberExpected);
            int sizeExpected = Simulator1D.DEFAULT_SIZE_OF_BOARD;
            int statesExpected = 3;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesExpected = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
            Imager1D imagerExpected = new Imager1D(rulesExpected);
            Simulator1D resultExpected = new Simulator1D(rulesExpected, imagerExpected, sizeExpected, Simulator1D.EdgeSettings.HardEdges);

            Simulator1D resultActual = Tools.MakeAutomataFromCode(testInput);
            Assert.AreEqual(resultExpected, resultActual, "The Automata generated were not equal.");
        }

        [TestMethod]
        public void Tools_MakeAutomataFromCode_AllArgsAccurateAndNEIGHBORHOODCorrectDefaultOutput()
        {
            string testInput = "k=3 b=300 r=1537550572281_10";
            BigInteger numberExpected;
            BigInteger.TryParse("1537550572281", out numberExpected);
            int sizeExpected = 300;
            int statesExpected = 3;
            int[] coordinatesExpected = Rules1D.DEFAULT_NEIGHBORHOOD_ORIENTATION;

            Rules1D rulesExpected = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
            Imager1D imagerExpected = new Imager1D(rulesExpected);
            Simulator1D resultExpected = new Simulator1D(rulesExpected, imagerExpected, sizeExpected, Simulator1D.EdgeSettings.HardEdges);

            Simulator1D resultActual = Tools.MakeAutomataFromCode(testInput);
            Assert.AreEqual(resultExpected, resultActual, "The Automata generated were not equal.");
        }

        [TestMethod]
        public void Tools_MakeAutomataFromCode_AllArgsAccurateAndSTATESCorrectDefaultOutput()
        {
            string testInput = "b=300 n={-1,0,1} r=1537550572281_10";
            BigInteger numberExpected;
            BigInteger.TryParse("1537550572281", out numberExpected);
            int sizeExpected = 300;
            int statesExpected = Rules1D.DEFAULT_POSSIBLE_STATES;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesExpected = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
            Imager1D imagerExpected = new Imager1D(rulesExpected);
            Simulator1D resultExpected = new Simulator1D(rulesExpected, imagerExpected, sizeExpected, Simulator1D.EdgeSettings.HardEdges);

            Simulator1D resultActual = Tools.MakeAutomataFromCode(testInput);
            Assert.AreEqual(resultExpected, resultActual, "The Automata generated were not equal.");
        }
    }
}
