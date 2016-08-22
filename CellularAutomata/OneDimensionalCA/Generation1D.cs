using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    class Generation1D
    {
        public int LeftEdge { get; set; }

        public List<Cell1D> Cells { get; }

        public Generation1D(List<Cell1D> cells)
        {
            Cells = cells;
            LeftEdge = Cells.First().Coordinates;
        }

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
