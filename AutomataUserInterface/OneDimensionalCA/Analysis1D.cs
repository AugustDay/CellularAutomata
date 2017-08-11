using AutomataUserInterface.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    public class Analysis1D
    {
        /// <summary> Cell Value Frequency Total: the count of occurences of each cell state throughout all generations.</summary>
        public int[] CellValueFreqTotal;

        /// <summary> Cell Value Frequency Per Generation: the count of occurences of each cell state for each generation.</summary>
        public int[][] CellValueFreqTotalPerGen;

        /// <summary> Neighborhood Lookup Frequency Total: the count of occurences of each nbhd value throughout all generations.</summary>
        public int[] NeighborhoodFreqTotal;

        /// <summary> Neighborhood Lookup Frequency Per Generation: the count of occurences of each nbhd value for each generation.</summary>
        public int[][] NeighboorhoodFreqPerGen;

        public Analysis1D(List<int[]> theGenerations, List<int[]> theNeighborhoodGenerations, Rules1D theRules)
        {
            //count cell state occurences
            CellValueFreqTotal = new int[theRules.PossibleStates];
            CellValueFreqTotalPerGen = new int[theGenerations.Count][];
            CountCellValueFrequency(theGenerations, theRules.PossibleStates);

            //count neighborhood occurences
            NeighborhoodFreqTotal = new int[theRules.PossibleNeighborhoodLookupValues];
            NeighboorhoodFreqPerGen = new int[theNeighborhoodGenerations.Count][];
            CountNeighborhoodLookupFrequency(theNeighborhoodGenerations, theRules.PossibleNeighborhoodLookupValues);

            //if generations.count > 1, check for dead universe
            //if universe not dead, check for loops
            //look for other interesting things
        }


        private void CountCellValueFrequency(List<int[]> theGenerations, int thePossibleStates)
        {
            int index = 0;
            foreach (int[] generation in theGenerations)
            {
                CellValueFreqTotalPerGen[index] = new int[thePossibleStates];
                foreach (int cell in generation)
                {
                    CellValueFreqTotal[cell]++;
                    CellValueFreqTotalPerGen[index][cell]++;
                }
                index++;
            }
        }

        private void CountNeighborhoodLookupFrequency(List<int[]> theNeighborhoodLookupGenerations, int thePossibleNeighborhoodStates)
        {
            int index = 0;
            foreach (int[] generation in theNeighborhoodLookupGenerations)
            {
                NeighboorhoodFreqPerGen[index] = new int[thePossibleNeighborhoodStates];
                foreach (int cell in generation)
                {
                    NeighborhoodFreqTotal[cell]++;
                    NeighboorhoodFreqPerGen[index][cell]++;
                }
                index++;
            }
        }

        /* determine if the CA became dead, or entered a looping pattern during its run.
         * Calculate entropy??
         * calculate percentage of cells that change state between generations (for each generation)
         * 
         * 
         */

        public string GetAnalysis()
        {
            StringBuilder sb = new StringBuilder();
            //cell value freq


            sb.Append("Cell Value Total Frequencies:\n");
            for (int i = 0; i < CellValueFreqTotal.Length; i++)
            {
                sb.Append(i + ": " + CellValueFreqTotal[i] + "\n");
            }

            sb.Append("\nCell Value Frequencies Per Generation:\n");
            for (int c = 0; c < CellValueFreqTotal.Length; c++)
            {
                sb.Append("Value '" + c + "':\n");
                for (int i = 0; i < CellValueFreqTotalPerGen.Length; i++)
                {
                    sb.Append(CellValueFreqTotalPerGen[i][c] + "\n");
                }
            }

            sb.Append("\nNeighborhood Value Total Frequencies:\n");
            for (int i = 0; i < NeighborhoodFreqTotal.Length; i++)
            {
                sb.Append(i + ": " + NeighborhoodFreqTotal[i] + "\n");
            }

            sb.Append("\nNeighborhood Value Frequencies Per Generation:\n");
            for (int c = 0; c < NeighborhoodFreqTotal.Length; c++)
            {
                sb.Append("Value '" + c + "':\n");
                for (int i = 0; i < NeighboorhoodFreqPerGen.Length; i++)
                {
                    sb.Append(NeighboorhoodFreqPerGen[i][c] + "\n");
                }
            }

            return sb.ToString();
        }
    }
}
