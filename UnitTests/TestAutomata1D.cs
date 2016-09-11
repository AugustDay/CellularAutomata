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
    public class TestSimulation1D
    {
        [TestMethod]
        public void Simulation1D_SimulationIsAccurateForRule30()
        {
            List<int[]> correctSimulationList = new List<int[]>();
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 0, 0, });
            correctSimulationList.Add(new int[] { 0, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, });
            correctSimulationList.Add(new int[] { 1, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, });
            string testInput = "k=2 n={-1,0,1} r=30_10 b=30";
            Simulator1D cellularAutomata30 = Tools.MakeAutomataFromCode(testInput);
            cellularAutomata30.Go(15);

            int counter = 0;
            foreach(int c in cellularAutomata30.CellularAutomata[14].Cells)
            {
                Assert.AreEqual(correctSimulationList[14][counter], c, "The expected value of cell  " + counter + " on row 15 was not equal.");
                counter++;
            }
        }

        [TestMethod]
        public void Simulation1D_SimulationIsAccurateForRule1537550572281()
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
            string testInput = "k=3 n={-1,0,1} r=1537550572281_10 b=30";
            Simulator1D cellularAutomata30 = Tools.MakeAutomataFromCode(testInput);
            cellularAutomata30.Go(15);

            int counter = 0;
            foreach (int c in cellularAutomata30.CellularAutomata[14].Cells)
            {
                Assert.AreEqual(correctSimulationList[14][counter], c, "The expected value of cell  " + counter + " on row 15 was not equal.");
                counter++;
            }
        }
    }
}
