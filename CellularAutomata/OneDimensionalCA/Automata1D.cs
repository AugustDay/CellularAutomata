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

        public Imager1D myImager;

        public Rules1D myRules;

        private int farthestLeft;

        List<Cell1D> myOrigin;

        public int mySizeOfBoard;

        public int[] myNBH;

        public Automata1D()
        {
            myRules = new Rules1D();
            myImager = new Imager1D(myRules);
            cellularAutomata = new List<Generation1D>();
            myOrigin = new List<Cell1D>();
            mySizeOfBoard = 400;
            setOriginSingleCell();
            //setOriginRandomCells();
            farthestLeft = 0;
            
            myNBH = new int[myRules.myNeighborhoodSize];
        }

        public void setOriginSingleCell() //with 75 dead cells on either side
        {
            myOrigin.Clear();
            int i;
            for(i = -(mySizeOfBoard / 2); i < 0; i++ )
            {
                myOrigin.Add(new Cell1D(i, 0));
            }
            myOrigin.Add(new Cell1D(0, 1));
            i++;
            for(; i <= (mySizeOfBoard / 2); i++)
            {
                myOrigin.Add(new Cell1D(i, 0));
            }
        }

        public void setOriginRandomCells()
        {
            myOrigin.Clear();
            Random r = new Random();
            for(int i = 0; i < mySizeOfBoard; i++)
            {
                myOrigin.Add(new Cell1D(i, r.Next(myRules.myPossibleStates)));
            }
        }

        public void Go()
        {
            Initialize();
            proceed();
            myImager.GenerateAndSaveImage(cellularAutomata, Math.Abs(farthestLeft));
        }

        /// <summary>
        /// Prepare automata with a single live cell in the middle of the universe.
        /// </summary>
        public void Initialize()
        {
            cellularAutomata.Clear();
            cellularAutomata.Add(new Generation1D(myOrigin));
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
                farthestLeft = Math.Min(farthestLeft, next.LeftEdge);

            }
        }

        public List<Cell1D> NewGeneration(Generation1D theGen)
        {
            LinkedList<Cell1D> newCells = new LinkedList<Cell1D>();
            for (int i = 0; i < theGen.Cells.Count; i++)
            {
                Cell1D c = new Cell1D(theGen.Cells[i].Coordinates);
                getNeighboorhood(i, theGen);
                c.State = myRules.rule(myNBH); //TODO make GetNeighborhood thing! (in Rule?)
                newCells.AddLast(c);
            }

            //leftPadding(newCells);
            //rightPadding(newCells);
            return newCells.ToList();
        }

        public void getNeighboorhood(int i, Generation1D theGen)
        {
            for (int n = 0; n < myRules.myNeighborhoodSize; n++) 
            {
                int location = myRules.myNeighboorhoodOrientation[n] + i;
                if(location > 0 && location < theGen.Cells.Count) //is not out of bounds
                {
                    myNBH[n] = theGen.Cells[location].State;
                } else
                {
                    myNBH[n] = 0; //dead out of bounds
                }
            }
        }

        private void leftPadding(LinkedList<Cell1D> list)
        {
            int padding = 3;
            foreach (Cell1D c in list)
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
                Cell1D empty = new Cell1D(list.First().Coordinates - 1);
                empty.State = 0;
                list.AddFirst(empty);
            }
        }

        private void rightPadding(LinkedList<Cell1D> list)
        {
            int padding = 3;
            foreach (Cell1D c in list.Reverse())
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
                Cell1D empty = new Cell1D(list.Last().Coordinates + 1);
                empty.State = 0;
                list.AddLast(empty);
            }
        }
    }
}
