using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    /// <summary>Object representing a single one-dimensional Cell.</summary>
    public class Cell1D
    {
        /// <summary>Location on horizontal axis in 1D Automata Generation.</summary>
        public int Coordinate { get; }

        /// <summary>State of this Cell.</summary>
        public int State { get; set; }

        /// <summary>
        /// Constructs a new Cell with the given Coordinate value,
        /// and default State of "0 = dead".
        /// </summary>
        /// <param name="theCoordinate"></param>
        public Cell1D(int theCoordinate)
        {
            Coordinate = theCoordinate;
            State = 0;
        }

        /// <summary>
        /// Constructs a new Cell with a given Coordinate and State value.
        /// </summary>
        /// <param name="theCoordinate"></param>
        /// <param name="theState"></param>
        public Cell1D(int theCoordinate, int theState)
        {
            Coordinate = theCoordinate;
            State = theState;
        }

        public override bool Equals(object theOther)
        {
            if (theOther == null || GetType() != theOther.GetType())
            {
                return false;
            }
            Cell1D otherCell = (Cell1D)theOther;
            return Coordinate == otherCell.Coordinate && State == otherCell.State;
        }
    }
}
