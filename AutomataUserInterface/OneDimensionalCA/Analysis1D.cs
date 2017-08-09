using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    public class Analysis1D
    {
        public int[] CellValueFrequency;

        public Analysis1D()
        {
        }

        /* Analysis needed: count requency of each state, per generation.
         * count frequency of each neighborhood lookup, per generation.
         * determine if the CA became dead, or entered a looping pattern during its run.
         * Calculate entropy??
         * calculate percentage of cells that change state between generations (for each generation)
         * 
         * 
         */

        public string GetAnalysis()
        {
            return "Analysis not yet implemented!";
        }
    }
}
