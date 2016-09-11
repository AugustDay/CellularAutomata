using CellularAutomata;
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
            int[] coordinatesExpected = new int[] { };

            Rules1D rulesToTest = new Rules1D(numberExpected, coordinatesExpected, statesExpected);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Rules1D_BigIntConstructor_ThrowsExceptionOnInvalidRule_ToBigForStates()
        {
            BigInteger numberExpected;
            BigInteger.TryParse("257", out numberExpected);
            int statesExpected = 2;
            int[] coordinatesExpected = new int[] { -1, 0, 1 };

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

        [TestMethod]
        public void Rules1D_CalculateRuleArrayFromBigInteger_30_ReturnsTrueWhenArrayGeneratedSuccessfully()
        {
            //Make a bool in the Rules1D class which says if the array is being stored in big or little endian. Then, in this test function that can be used. 
            int[] sequenceExpected = new int[] { 0, 1, 1, 1, 1, 0, 0, 0 }; //notice, it's currently BigEndian!
            BigInteger numberInput;
            BigInteger.TryParse("30", out numberInput);
            int statesInput = 2;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            Rules1D rulesToTest = new Rules1D(numberInput, coordinatesInput, statesInput);

            Assert.IsTrue(rulesToTest.RuleArray.SequenceEqual(sequenceExpected),
                        "Rule Array was not correct (did you switch to LittleEndian?)");
        }

        [TestMethod]
        public void Rules1D_CalculateRuleArrayFromBigInteger_30_ReturnsTrueWhenDoesNotGenerateLittleEndian()
        {
            int[] sequenceExpected = new int[] { 0, 0, 0, 1, 1, 1, 1, 0 }; //should NOT be this
            BigInteger numberInput;
            BigInteger.TryParse("30", out numberInput);
            int statesInput = 2;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            Rules1D rulesToTest = new Rules1D(numberInput, coordinatesInput, statesInput);

            Assert.IsFalse(rulesToTest.RuleArray.SequenceEqual(sequenceExpected),
                        "Rule Array was in little endian");
        }

        [TestMethod]
        public void Rules1D_CalculateRuleArrayFromBigInteger_1537550572281_ReturnsTrueWhenArrayGeneratedSuccessfully()
        {
            //Make a bool in the Rules1D class which says if the array is being stored in big or little endian. Then, in this test function that can be used. 
            //int[] sequenceExpected = new int[] { 0, 1, 2, 1, 0, 2, 2, 2, 2, 2, 0, 0, 1, 1, 2, 1, 2, 2, 2, 1, 2, 0, 0, 1, 2, 2, 0 };
            int[] sequenceExpected = new int[] { 0, 2, 2, 1, 0, 0, 2, 1, 2, 2, 2, 1, 2, 1, 1, 0, 0, 2, 2, 2, 2, 2, 0, 1, 2, 1, 0 };
            //notice, it's currently BigEndian!
            BigInteger numberInput;  
            BigInteger.TryParse("1537550572281", out numberInput);
            int statesInput = 3;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            Rules1D rulesToTest = new Rules1D(numberInput, coordinatesInput, statesInput);

            Assert.IsTrue(rulesToTest.RuleArray.SequenceEqual(sequenceExpected),
                        "Rule Array was not correct (did you switch to LittleEndian?), was: " + Tools.DisplayArray(rulesToTest.RuleArray));
        }

        [TestMethod]
        public void Rules1D_CalculateRuleArrayFromBigInteger_4StateRule_ReturnsTrueWhenArrayGeneratedSuccessfully()
        {
            //Make a bool in the Rules1D class which says if the array is being stored in big or little endian. Then, in this test function that can be used. 
            //300377737362090600663731055950616632373_10
            int[] sequenceExpected = new int[] { 1, 1, 3, 0, 0, 3, 1, 2, 0, 3, 2, 1, 0, 3, 3, 2, 2, 0, 1,
        0, 0, 2, 3, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 0, 0, 0, 2, 0, 1, 2, 2, 0, 3, 3, 3, 2, 1, 3,
        0, 3, 1, 3, 1, 1, 2, 2, 2, 2, 3, 3, 1, 0, 2, 3 }; //notice, it's currently BigEndian!
            BigInteger numberInput;
            BigInteger.TryParse("300377737362090600663731055950616632373", out numberInput);
            int statesInput = 4;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            Rules1D rulesToTest = new Rules1D(numberInput, coordinatesInput, statesInput);

            Assert.IsTrue(rulesToTest.RuleArray.SequenceEqual(sequenceExpected),
                        "Rule Array was not correct (did you switch to LittleEndian?)");
        }

        [TestMethod]
        public void Rules1D_LargestPossibleRuleNumber_ReturnsTheCorrectNumberFor2StateRule()
        {
            int statesInput = 2;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            int expectedNumber = 256; //2^(2^3)
            Rules1D rulesToTest = new Rules1D(coordinatesInput, statesInput);

            Assert.AreEqual(expectedNumber, rulesToTest.LargestPossibleRuleNumber(statesInput, coordinatesInput.Length));
        }

        [TestMethod]
        public void Rules1D_LargestPossibleRuleNumber_ReturnsTheCorrectNumberFor3StateRule()
        {
            int statesInput = 3;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            BigInteger expectedNumber;
            BigInteger.TryParse("7625597484987", out expectedNumber);
            Rules1D rulesToTest = new Rules1D(coordinatesInput, statesInput);

            Assert.AreEqual(expectedNumber, rulesToTest.LargestPossibleRuleNumber(statesInput, coordinatesInput.Length));
        }

        [TestMethod]
        public void Rules1D_LargestPossibleRuleNumber_ReturnsTheCorrectNumberFor7StateRule()
        {
            int statesInput = 7;
            int[] coordinatesInput = new int[] { -1, 0, 1 };
            BigInteger expectedNumber;
            BigInteger.TryParse("73897156067403508131992561964010797488623510811411465963096756553187673674242" + 
                "192208052907140882508921992930897316344188467202522254786303935865880767877211225678168284320" + 
                "446607858490555306354934819445999851584475980430310449242090867019155825594779128355380774995" + 
                "049124533408341373887572343", out expectedNumber);
            Rules1D rulesToTest = new Rules1D(coordinatesInput, statesInput);

            Assert.AreEqual(expectedNumber, rulesToTest.LargestPossibleRuleNumber(statesInput, coordinatesInput.Length));
        }

        [TestMethod]
        public void Rules1D_Construct8StateRuleWithoutGivenNumber_FailIfExceptionThrown()
        {
            int statesInput = 8;
            int[] coordinatesInput = new int[] { -1, 0, 1 };

            try
            {
                Rules1D ruleToTest = new Rules1D(coordinatesInput, statesInput);
            }
            catch (Exception e)
            {                
                Assert.Fail("Exception thrown, stack trace: " + e.StackTrace);
            }
        }

        [TestMethod]
        public void Rules1D_Construct7StateRuleWithoutGivenNumber_FailIfExceptionThrown()
        {
            int statesInput = 7;
            int[] coordinatesInput = new int[] { -1, 0, 1 };

            try
            {
                Rules1D ruleToTest = new Rules1D(coordinatesInput, statesInput);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception thrown, stack trace: " + e.StackTrace);
            }
        }

        [TestMethod]
        public void Rules1D_Construct6StateRuleWithoutGivenNumber_FailIfExceptionThrown()
        {
            int statesInput = 6;
            int[] coordinatesInput = new int[] { -1, 0, 1 };

            try
            {
                Rules1D ruleToTest = new Rules1D(coordinatesInput, statesInput);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception thrown, stack trace: " + e.StackTrace);
            }
        }



    }
}
