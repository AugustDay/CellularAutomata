using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    class Automata1D
    {
        public List<Generation1D> cellularAutomata;

        public Imager1D Imager;

        public Rules1D Rules;

        private int FarthestLeft;

        List<Cell1D> Origin;

        public int SizeOfBoard;

        public int[] NBH;

        public Automata1D()
        {
            Rules = new Rules1D();
            Imager = new Imager1D(Rules);
            cellularAutomata = new List<Generation1D>();
            Origin = new List<Cell1D>();
            SizeOfBoard = 400;
            setOriginSingleCell();
            //setOriginRandomCells();
            FarthestLeft = 0;
            
            NBH = new int[Rules.NeighborhoodSize];
        }

        public void setOriginSingleCell() //with 75 dead cells on either side
        {
            Origin.Clear();
            int i;
            for(i = -(SizeOfBoard / 2); i < 0; i++ )
            {
                Origin.Add(new Cell1D(i, 0));
            }
            Origin.Add(new Cell1D(0, 1/*Rules.PossibleStates - 1*/));
            i++;
            for(; i <= (SizeOfBoard / 2); i++)
            {
                Origin.Add(new Cell1D(i, 0));
            }
        }

        public void setOriginRandomCells()
        {
            Origin.Clear();
            for(int i = 0; i < SizeOfBoard; i++)
            {
                Origin.Add(new Cell1D(i, Tools.Rand.Next(Rules.PossibleStates)));
            }
        }

        public void Go()
        {
            Initialize();
            proceed();
            Imager.SaveImage(cellularAutomata, Math.Abs(FarthestLeft));
        }

        /// <summary>
        /// Prepare automata with a single live cell in the middle of the universe.
        /// </summary>
        public void Initialize()
        {
            cellularAutomata.Clear();
            cellularAutomata.Add(new Generation1D(Origin));
            FarthestLeft = 0;
        }

        /// <summary>
        /// Iterate forward 200 steps.
        /// </summary>
        public void proceed()
        {
            proceed(200);
        }

        /// <summary>
        /// Iterate forward a given number of steps. 
        /// </summary>
        /// <param name="theNumberOfSteps"></param>
        public void proceed(int theNumberOfSteps)
        {
            Generation1D next = cellularAutomata[cellularAutomata.Count - 1];
            for (int i = 0; i < theNumberOfSteps; i++)
            {
                next = new Generation1D(NewGeneration(next));
                cellularAutomata.Add(next);
                FarthestLeft = Math.Min(FarthestLeft, next.LeftEdge);

            }
        }

        public List<Cell1D> NewGeneration(Generation1D theGen)
        {
            LinkedList<Cell1D> newCells = new LinkedList<Cell1D>();
            for (int i = 0; i < theGen.Cells.Count; i++)
            {
                Cell1D c = new Cell1D(theGen.Cells[i].Coordinate);
                getNeighboorhood(i, theGen);
                c.State = Rules.rule(NBH); //TODO make GetNeighborhood thing! (in Rule?)
                newCells.AddLast(c);
            }

            //leftPadding(newCells);
            //rightPadding(newCells);
            return newCells.ToList();
        }

        public void getNeighboorhood(int theIndex, Generation1D theGen)
        {
            for (int n = 0; n < Rules.NeighborhoodSize; n++) 
            {
                int location = Rules.NeighborhoodOrientation[n] + theIndex;
                if(location > 0 && location < theGen.Cells.Count) //is not out of bounds
                {
                    NBH[n] = theGen.Cells[location].State;
                } else
                {
                    NBH[n] = 0; //dead out of bounds
                }
            }
        }

        private void leftPadding(LinkedList<Cell1D> theList)
        {
            int padding = 3;
            foreach (Cell1D c in theList)
            {
                if (c.State > 0)
                {
                    break;
                }
                else //c is dead
                {
                    padding--;
                }
            }

            for (; padding > 0; padding--)
            {
                Cell1D empty = new Cell1D(theList.First().Coordinate - 1);
                empty.State = 0;
                theList.AddFirst(empty);
            }
        }

        private void rightPadding(LinkedList<Cell1D> theList)
        {
            int padding = 3;
            foreach (Cell1D c in theList.Reverse())
            {
                if (c.State > 0)
                {
                    break;
                }
                else //c is dead
                {
                    padding--;
                }
            }

            for (; padding > 0; padding--)
            {
                Cell1D empty = new Cell1D(theList.Last().Coordinate + 1);
                empty.State = 0;
                theList.AddLast(empty);
            }
        }
    }
}
