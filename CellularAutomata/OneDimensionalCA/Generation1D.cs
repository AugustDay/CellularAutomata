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
        /// <summary> Coordinate of left-most Cell. </summary>
        public int LeftEdge { get; }

        /// <summary> The Cells in this Generation. </summary>
        public List<Cell1D> Cells { get; }

        /// <summary> Constructs a new Generation of the Automata. </summary>
        /// <param name="theCells"></param>
        public Generation1D(List<Cell1D> theCells)
        {
            Cells = theCells;

            //TODO might be more efficient to set LeftEdge = first LIVE cell.
            LeftEdge = Cells.First().Coordinate; 
        }

        /// <summary> Outputs a String representation of this Generation. </summary>
        /// <returns></returns>
        public string toString()
        {
            string s = "";
            foreach (Cell1D c in Cells)
            {
                s += c.State; //TODO does this work? Verify this.
            }
            return s;
        }
    }
}
