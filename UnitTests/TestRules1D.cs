using CellularAutomata.OneDimensionalCA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class TestRules1D
    {
        [TestMethod]
        public void Rules1D_BigIntConstructor_AccurateConstructionOfFourStateRule()
        {
            BigInteger numberExpected; 
            BigInteger.TryParse("1537550572281", out numberExpected);
            int statesExpected = 4;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);

            Assert.AreEqual(statesExpected, rulesToTest.PossibleStates, "The possible states were not equal.");
            Assert.AreEqual(coordinatesExpected, rulesToTest.NeighborhoodCoordinates, "The coordinates were not equal.");
            Assert.AreEqual(coordinatesExpected.Length, rulesToTest.NeighborhoodSize, "The neighborhood size was not equal.");
            Assert.AreEqual(numberExpected.ToString(), rulesToTest.RuleNumber, "The rule number was not equal.");
        }

        [TestMethod]
        public void Rules1D_BigIntConstructor_AccurateConstructionOfTwoStateRule()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("30", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);

            Assert.AreEqual(statesExpected, rulesToTest.PossibleStates, "The possible states were not equal.");
            Assert.AreEqual(coordinatesExpected, rulesToTest.NeighborhoodCoordinates, "The coordinates were not equal.");
            Assert.AreEqual(coordinatesExpected.Length, rulesToTest.NeighborhoodSize, "The neighborhood size was not equal.");
            Assert.AreEqual(numberExpected.ToString(), rulesToTest.RuleNumber, "The rule number was not equal.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Rules1D_BigIntConstructor_ThrowsExceptionOnInvalidStateNumber_LessThan2()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("30", out numberExpected);
            int statesExpected = 1;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Rules1D_BigIntConstructor_ThrowsExceptionOnInvalidCoordinates_EmptyArray()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("30", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] {};

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Rules1D_BigIntConstructor_ThrowsExceptionOnInvalidRule_ToBigForStates()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("257", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1};

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Rules1D_BigIntConstructor_ThrowsExceptionOnInvalidRule_LessThanZero()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("-1", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
        }

        [TestMethod]
        public void Rules1D_Equals_ReturnsTrueWhenTwoRulesAreEqual()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("30", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
            Rules1D secondRuleToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);

            Assert.AreEqual(rulesToTest, secondRuleToTest, "The rules were not considered equal.");
        }

        [TestMethod]
        public void Rules1D_Equals_ReturnsFalseWhenArgumentIsNull()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("30", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
            Rules1D secondRuleToTest = null;

            Assert.AreNotEqual(rulesToTest, secondRuleToTest, "The rules were considered equal.");
        }

        [TestMethod]
        public void Rules1D_Equals_ReturnsFalseWhenArgumentIsNotARule1D()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("30", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);

            Assert.AreNotEqual(rulesToTest, numberExpected, "The objects were considered equal.");
        }
    }
}
