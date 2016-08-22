using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    class Cell1D
    {
        public int Coordinates { get; }

        public int State { get; set; }

        public Cell1D(int coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
