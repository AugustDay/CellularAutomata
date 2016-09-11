using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    /// <summary>
    /// Represents a single Generation of the Automata, 
    /// containing a list of Cells and their leftmost coordinate.
    /// </summary>
    public class Generation1D
    {
        /// <summary> The Cells in this Generation. </summary>
        public int[] Cells { get; }

        /// <summary> Constructs a new Generation of the Automata. </summary>
        /// <param name="theCells"></param>
        public Generation1D(int[] theCells)
        {
            Cells = theCells;
        }

        /// <summary> Outputs a String representation of this Generation. </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            foreach (int i in Cells)
            {
                s += i; //TODO does this work? Verify this.
            }
            return s;
        }

        public override bool Equals(object theOther)
        {
            if (theOther == null || GetType() != theOther.GetType())
            {
                return false;
            }
            return Cells.SequenceEqual(((Generation1D)theOther).Cells);
        }
    }
}
