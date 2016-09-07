using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    public class Automata1D
    {
        public static readonly int DEFAULT_SIZE_OF_BOARD = 400;

        public int DEFAULT_NUMBER_OF_STEPS = 200;

        private List<Cell1D> Origin;

        private int FarthestLeft;

        /// <summary>The state of the cells within a given Cell's neighborhood coordinates.</summary>
        private int[] LocalSituation;

        public List<Generation1D> CellularAutomata { get; }

        public Imager1D Imager { get; }

        public Rules1D Rules { get; }

        public int SizeOfBoard { get; set; }

        public int Generation { get { return CellularAutomata.Count; } }

        public Automata1D() 
        {
            Rules = new Rules1D();
            Imager = new Imager1D(Rules);
            CellularAutomata = new List<Generation1D>();
            ConstructorHelper(DEFAULT_SIZE_OF_BOARD);
        }

        public Automata1D(Rules1D theRules, Imager1D theImager, int theSizeOfBoard)
        {
            if(theRules == null || 
               theImager == null ||
               theSizeOfBoard < 1)
            {
                throw new ArgumentException();
            }
            Rules = theRules;
            Imager = theImager;
            CellularAutomata = new List<Generation1D>();
            ConstructorHelper(theSizeOfBoard);
        }

        private void ConstructorHelper(int theSizeOfBoard)
        {            
            LocalSituation = new int[Rules.NeighborhoodSize];
            Origin = new List<Cell1D>();
            SizeOfBoard = theSizeOfBoard;
            setOriginSingleCell(); //TODO should be changeable from code in Tools. random or single should be part of info.txt!
            Initialize();
        }

        public void setOriginSingleCell()
        {
            setOriginSingleCell(1);
        }

        public void setOriginSingleCell(int theState) //with 75 dead cells on either side
        {
            if(theState >= Rules.PossibleStates || theState < 0)
            {
                throw new ArgumentOutOfRangeException("theState", theState, "Need a given state that exists in this rule.");
            }
            Origin.Clear();
            int i;
            for(i = -(SizeOfBoard / 2); i < 0; i++ )
            {
                Origin.Add(new Cell1D(i, 0));
            }
            Origin.Add(new Cell1D(0, theState));
            i++;
            for(; i <= (SizeOfBoard / 2); i++)
            {
                Origin.Add(new Cell1D(i, 0));
            }
            Initialize(); //TODO should these setOrigin functions re-initialize?
        }

        public void setOriginRandomCells()
        {
            Origin.Clear();
            for(int i = 0; i < SizeOfBoard; i++)
            {
                Origin.Add(new Cell1D(i, Tools.Rand.Next(Rules.PossibleStates)));
            }
            Initialize();
        }

        public void Go()
        {
            Go(DEFAULT_NUMBER_OF_STEPS);            
        }

        public void Go(int theNumberOfSteps)
        {
            Initialize();
            Proceed(theNumberOfSteps);
        }

        /// <summary>
        /// Prepare automata with a single live cell in the middle of the universe.
        /// </summary>
        public void Initialize()
        {
            CellularAutomata.Clear();
            CellularAutomata.Add(new Generation1D(Origin));
            FarthestLeft = CellularAutomata.Last().LeftEdge;
        }

        /// <summary>
        /// Iterate forward 200 steps.
        /// </summary>
        public void Proceed()
        {
            Proceed(DEFAULT_NUMBER_OF_STEPS);
        }

        /// <summary>
        /// Iterate forward a given number of steps. 
        /// </summary>
        /// <param name="theNumberOfSteps"></param>
        public void Proceed(int theNumberOfSteps)
        {
            Generation1D next = CellularAutomata[CellularAutomata.Count - 1];
            for (int i = 0; i < theNumberOfSteps; i++)
            {
                next = new Generation1D(NewGeneration(next));
                CellularAutomata.Add(next);
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
                c.State = Rules.rule(LocalSituation); //TODO make GetNeighborhood thing! (in Rule?)
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
                int location = Rules.NeighborhoodCoordinates[n] + theIndex;
                if(location > 0 && location < theGen.Cells.Count) //is not out of bounds
                {
                    LocalSituation[n] = theGen.Cells[location].State;
                } else
                {
                    LocalSituation[n] = 0; //dead out of bounds
                }
            }
        }

        public void OutputAutomata()
        {
            Imager.SaveImage(CellularAutomata, Math.Abs(FarthestLeft));
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
