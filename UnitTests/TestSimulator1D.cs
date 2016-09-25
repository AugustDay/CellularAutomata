using CellularAutomata;
using CellularAutomata.OneDimensionalCA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests
{
    [TestClass]
    public class TestSimulator1D
    {
        [TestMethod]
        public void Simulation1D_SimulationIsAccurateForRule30_WraparoundEdges()
        {
            int[][] correctSimulationList = new int[][] {
                new int[] { 0, 0, 0, 1, 0, 0, 0 },
                new int[] { 0, 0, 1, 1, 1, 0, 0 },
                new int[] { 0, 1, 1, 0, 0, 1, 0 },
                new int[] { 1, 1, 0, 1, 1, 1, 1 },
                new int[] { 0, 0, 0, 1, 0, 0, 0 }, //repeats!
                new int[] { 0, 0, 1, 1, 1, 0, 0 },
                new int[] { 0, 1, 1, 0, 0, 1, 0 },
                new int[] { 1, 1, 0, 1, 1, 1, 1 },
                new int[] { 0, 0, 0, 1, 0, 0, 0 }};

            string testInput = "k=2 n={-1,0,1} r=30_10 b=7";
            Simulator1D cellularAutomata30 = Tools.MakeAutomataFromCode(testInput);
            cellularAutomata30.GenerateImageAfterSimulating = false;

            cellularAutomata30.Go(8);

            int counter = 0;
            foreach (int c in cellularAutomata30.Generations[4])
            {
                Assert.AreEqual(correctSimulationList[4][counter], c, "The expected value of cell  " + counter + " on row 5 was " + 
                    c + ", expected: " + correctSimulationList[4][counter] + ".");
                counter++;
            }

            counter = 0;
            foreach (int c in cellularAutomata30.Generations[7])
            {
                Assert.AreEqual(correctSimulationList[7][counter], c, "The expected value of cell  " + counter + " on row 5 was " + 
                    c + ", expected: " + correctSimulationList[7][counter]);
                counter++;
            }
        }

        [TestMethod]
        public void Simulation1D_SimulationIsAccurateForRule30_HardEdges()
        {
            int[][] correctSimulationList = new int[][] {
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, },
                new int[] { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, },
                new int[] { 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, },
                new int[] { 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, },
                new int[] { 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, }};

            string testInput = "k=2 n={-1,0,1} r=30_10 b=30 h";
            Simulator1D cellularAutomata30 = Tools.MakeAutomataFromCode(testInput);
            cellularAutomata30.GenerateImageAfterSimulating = false;

            cellularAutomata30.Go(15);

            int counter = 0;
            foreach(int c in cellularAutomata30.Generations[14])
            {
                Assert.AreEqual(correctSimulationList[14][counter], c, "The expected value of cell  " + counter + " on row 15 was not equal.");
                counter++;
            }
        }

        [TestMethod]
        public void Simulation1D_SimulationIsAccurateForRule1537550572281_HardEdges()
        {
            List<int[]> correctSimulationList = new List<int[]>();
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 2, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 1, 1, 1, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 0, 0, 1, 1, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 2, 2, 2, 0, 1, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 1, 2, 0, 2, 2, 1, 1, 2, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 0, 1, 0, 2, 2, 1, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 2, 1, 0, 2, 2, 1, 1, 2, 1, 2, 2, 1, 1, 2, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 1, 2, 1, 0, 1, 0, 2, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 2, 1, 1, 0, 1, 0, 2, 2, 1, 2, 0, 0, 2, 2, 1, 1, 2, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 2, 1, 0, 2, 2, 1, 1, 2, 1, 1, 0, 2, 2, 2, 1, 0, 1, 2, 0, 2, 2, 2, 2, 2, 2, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 0, 2, 1, 2, 0, 1, 2, 2, 0, 0, 2, 2, 0, 0, 0, 0, 2, 2, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 2, 1, 1, 0, 1, 0, 2, 2, 1, 2, 2, 1, 1, 0, 2, 0, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2, 2, 2, 0, });
            correctSimulationList.Add(new int[] { 2, 1, 0, 2, 2, 1, 1, 2, 1, 1, 2, 1, 0, 2, 1, 2, 2, 2, 0, 0, 0, 0, 0, 2, 2, 2, 2, 0, 0, 2, 2, });
            string testInput = "k=3 n={-1,0,1} r=1537550572281_10 b=30 h";
            Simulator1D cellularAutomata30 = Tools.MakeAutomataFromCode(testInput);
            cellularAutomata30.GenerateImageAfterSimulating = false;

            cellularAutomata30.Go(15);

            int counter = 0;
            foreach (int c in cellularAutomata30.Generations[14])
            {
                Assert.AreEqual(correctSimulationList[14][counter], c, "The expected value of cell  " + counter + " on row 15 was not equal.");
                counter++;
            }
        }
    }
}
