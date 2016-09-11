using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    public class Simulator1D
    {
        public static readonly int DEFAULT_SIZE_OF_BOARD = 400;

        public int DEFAULT_NUMBER_OF_STEPS = 200;

        private int[] Origin;

        /// <summary>The state of the cells within a given Cell's neighborhood coordinates.</summary>
        private int[] LocalSituation;

        public List<Generation1D> CellularAutomata { get; }

        public Imager1D Imager { get; }

        public Rules1D Rules { get; }

        public int SizeOfBoard { get; set; }

        public int Generation { get { return CellularAutomata.Count; } }

        public Simulator1D() 
        {
            Rules = new Rules1D();
            Imager = new Imager1D(Rules);
            CellularAutomata = new List<Generation1D>();
            ConstructorHelper(DEFAULT_SIZE_OF_BOARD);
        }

        public Simulator1D(Rules1D theRules, Imager1D theImager, int theSizeOfBoard)
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
            SizeOfBoard = theSizeOfBoard;
            Origin = new int[SizeOfBoard];
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
            Origin = new int[SizeOfBoard];
            int i;
            for(i = 0; i < SizeOfBoard / 2; i++ )
            {
                Origin[i] = 0;
            }
            Origin[i] = theState;
            i++;
            for(; i < SizeOfBoard; i++)
            {
                Origin[i] = 0;
            }
            Initialize(); //TODO should these setOrigin functions re-initialize?
        }

        public void setOriginRandomCells()
        {
            Origin = new int[SizeOfBoard];
            for(int i = 0; i < SizeOfBoard; i++)
            {
                Origin[i] = Tools.Rand.Next(Rules.PossibleStates);
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
            }
        }

        public int[] NewGeneration(Generation1D theGen)
        {
            int[] newCells = new int[SizeOfBoard];
            for (int i = 0; i < theGen.Cells.Length; i++)
            {
                getNeighboorhood(i, theGen);
                newCells[i] = Rules.rule(LocalSituation);
            }

            return newCells;
        }

        public void getNeighboorhood(int theIndex, Generation1D theGen)
        {
            for (int n = 0; n < Rules.NeighborhoodSize; n++) 
            {
                int location = Rules.NeighborhoodCoordinates[n] + theIndex;
                if(location > 0 && location < theGen.Cells.Length) //is not out of bounds
                {
                    LocalSituation[n] = theGen.Cells[location];
                } else
                {
                    LocalSituation[n] = 0; //dead out of bounds
                }
            }
        }

        public void OutputAutomata()
        {
            Imager.SaveImage(CellularAutomata);
        }

        public override bool Equals(Object theOther)
        {
            // Check for null values and compare run-time types.
            if (theOther == null || GetType() != theOther.GetType())
                return false;

            Simulator1D otherAutomata = (Simulator1D)theOther;
            return /*Rules.Equals(theOther.Rules) &&*/ Imager.Equals(otherAutomata.Imager) &&
                //Imager will need to check its Rule for equality anyways, no need to directly check Rule
                SizeOfBoard == otherAutomata.SizeOfBoard &&
                Origin.SequenceEqual(otherAutomata.Origin) && LocalSituation.SequenceEqual(otherAutomata.LocalSituation) &&
                CellularAutomata.SequenceEqual(otherAutomata.CellularAutomata); //should it compare the generations?
        }
    }
}
