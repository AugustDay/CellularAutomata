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

        public int[] neighborhood = new int[3];

        public Automata1D()
        {
            myRules = new Rules1D();
            myImager = new Imager1D(myRules);
            cellularAutomata = new List<Generation1D>();
            farthestLeft = 0;
            
        }

        public void Go()
        {
            Initialize();
            proceed(100);
            myImager.GenerateAndSaveImage(cellularAutomata, Math.Abs(farthestLeft));
        }

        /// <summary>
        /// Prepare automata with a single live cell in the middle of the universe.
        /// </summary>
        public void Initialize()
        {
            cellularAutomata.Clear();
            List<Cell1D> origin = new List<Cell1D>();
            Cell1D c = new Cell1D(0);
            c.State = 1;
            origin.Add(new Cell1D(-2));
            origin.Add(new Cell1D(-1));
            origin.Add(c);
            origin.Add(new Cell1D(1));
            origin.Add(new Cell1D(2));
            cellularAutomata.Add(new Generation1D(origin));
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
            for (int i = 1; i < theGen.Cells.Count - 1; i++)
            {
                Cell1D c = new Cell1D(theGen.Cells[i].Coordinates);
                neighborhood[0] = theGen.Cells[i - 1].State;
                neighborhood[1] = theGen.Cells[i].State;
                neighborhood[2] = theGen.Cells[i + 1].State;
                
                c.State = myRules.rule(neighborhood); //TODO make GetNeighborhood thing! (in Rule?)
                newCells.AddLast(c);
            }

            leftPadding(newCells);
            rightPadding(newCells);
            return newCells.ToList();
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
