using System;
using System.Collections.Generic;
using System.Linq;

namespace CellularAutomata.OneDimensionalCA
{
    public class Simulator1D
    {
        public enum EdgeSettings
        {
            HardEdges,
            WraparoundEdges,
        }

        public static EdgeSettings DEFAULT_EDGE_SETTING = EdgeSettings.WraparoundEdges;

        public static int DEFAULT_SIZE_OF_BOARD = 200;

        public static int[] DEFAULT_STARTING_CELLS = new int[] { 1 };

        public int DEFAULT_NUMBER_OF_STEPS = 200;

        public int[] StartingCells { get; private set; }

        ///<summary>The starting orientation of cells in a new simulation.</summary>
        private int[] Origin;

        ///<summary>The state of the cells around a given cell.</summary>
        private int[] LocalSituation;

        private Dictionary<int, int> WraparoundIndexes;

        ///<summary>Each generation of this Automata, [0] being origin.</summary>
        public List<int[]> Generations { get; private set; }

        /// <summary> The numerical values of the neighborhood orientation which produced each cell in Generations. All zero on origin.</summary>
        public List<int[]> NeighborhoodGenerations { get; private set; }

        ///<summary>The imager object that the Automata can use to output results.</summary>
        public Imager1D Imager { get; private set; }

        ///<summary>Rules for the Automata.</summary>
        public Rules1D Rules { get; private set; }

        public Analysis1D Analysis { get; private set; }

        ///<summary>The number of cells on a row.</summary>
        public int SizeOfBoard { get; private set; }

        ///<summary>Total number of generations that the Automata has been simulated.</summary>
        public int StepNumber { get { return Generations.Count; } private set { } }

        public EdgeSettings CurrentEdgeSetting { get; private set; }

        public bool DisplayNeighborhoodLookup { get; set; }

        //TODO create function to retrieve current rule name (number string)
        //TODO create API class above simulator with public methods and private simulator/rules/imager, so that gui can't access them. Use Interface.

        ///<summary>No-argument constructor makes an Simulator with default parameters.</summary>
        public Simulator1D()
        {
            Rules = new Rules1D();
            Imager = new Imager1D();            
            Generations = new List<int[]>();
            NeighborhoodGenerations = new List<int[]>();
            ConstructorHelper(DEFAULT_SIZE_OF_BOARD, DEFAULT_STARTING_CELLS, EdgeSettings.WraparoundEdges);
        }

        ///<summary>Constructs a simulator with the given objects.</summary>
        public Simulator1D(Rules1D theRules, Imager1D theImager, int theSizeOfBoard, int[] theStartingCells, EdgeSettings theSetting)
        {
            if (theRules == null ||
               theImager == null)
            {
                throw new ArgumentNullException("The Rule or Imager param was null");
            }
            Rules = theRules;
            Imager = theImager;
            Generations = new List<int[]>();
            NeighborhoodGenerations = new List<int[]>();
            ConstructorHelper(theSizeOfBoard, theStartingCells, theSetting);
        }

        ///<summary>Initializes the components of the simulator.</summary>
        private void ConstructorHelper(int theSizeOfBoard, int[] theStartingCells, EdgeSettings theSetting)
        {            
            if (theSizeOfBoard < 1)
            {
                throw new ArgumentException("Board cannot be of size less than 1!");
            }
            LocalSituation = new int[Rules.NeighborhoodSize];
            SizeOfBoard = theSizeOfBoard;
            Origin = new int[SizeOfBoard];
            SetOriginStartingCells(theStartingCells); /* TODO should be changeable from code in Tools. random or single should be part of info.txt!
                                                         Could fix by making SetOriginStartingCells return be static in tools and return the array here. */

            CurrentEdgeSetting = theSetting;
            if (CurrentEdgeSetting == EdgeSettings.WraparoundEdges)
            {
                CalculateWraparoundIndex();
            }
            Initialize();            
        }

        private void CalculateWraparoundIndex()
        {
            WraparoundIndexes = new Dictionary<int, int>();
            foreach (int coord in Rules.NeighborhoodCoordinates)
            {
                if (Math.Abs(coord) > SizeOfBoard)
                {
                    throw new ArgumentException("All neighborhood coordinates must be within the size of the board.");
                    //otherwise, cells would be counted multiple times in such a neighborhood (and require an extra loop to calculate).
                    //TODO wraparound with large neighborhoods might be interesting, perhaps add such functionality later? 
                }
                if (coord < 0)
                {
                    WraparoundIndexes.Add(coord, SizeOfBoard + coord);
                }
                else if (coord > 0)
                {
                    WraparoundIndexes.Add(coord + SizeOfBoard - 1, -1 + coord);
                }
            }
        }

        ///<summary>Sets Origin with a cell in the middle with state=1.</summary>
        public void setOriginSingleCell()
        {
            SetOriginStartingCells(new int[] { 1 });
        }

        ///<summary>Sets Origin with a single live cell in the middle with the given state.</summary>
        public void setOriginSingleCell(int theState) //with 75 dead cells on either side
        {
            SetOriginStartingCells(new int[] { theState });
        }

        ///<summary>Sets Origin with a single live cell in the middle with the given state.</summary>
        public void SetOriginStartingCells(int[] theCells) //with 75 dead cells on either side
        {
            StartingCells = theCells;
            Origin = new int[SizeOfBoard];
            int i;
            for (i = 0; i < (SizeOfBoard / 2) - (theCells.Length / 2); i++)
            {
                Origin[i] = 0;
            }
            foreach (int c in theCells)
            {
                if (c >= Rules.PossibleStates)
                { //TODO this validation is happening twice in the case of String input. Make private inner function?
                    //Right now Starting Cells might not necessarily equal what the origin really is (set before validation).
                    Origin[i] = Rules.PossibleStates - 1;
                }
                else if (c < 0)
                {
                    Origin[i] = 0;
                }
                else
                {
                    Origin[i] = c;
                }
                i++;
            }
            for (; i < SizeOfBoard; i++)
            {
                Origin[i] = 0;
            }
            Initialize();
        }

        ///<summary>Sets Origin with a single live cell in the middle with the given state.</summary>
        public void SetOriginStringInput(string theInput) //with 75 dead cells on either side
        {
            //have a static string be the default, which persists when making a new automata.
            //convert to an int[] array, parse the string into this array only the first time. 
            int[] specificOrigin = ParseOriginInput(theInput);
            if (specificOrigin.Length > SizeOfBoard)
            { //if user gives an origin that's larger than the current size, just increase the dimensions to match. 
                SizeOfBoard = specificOrigin.Length;
            }
            SetOriginStartingCells(specificOrigin);
        }

        private int[] ParseOriginInput(string theInput)
        {
            int[] specificOrigin = new int[theInput.Length];
            int cell;
            bool succeed;
            int counter = 0;
            foreach (char c in theInput)
            {
                succeed = Int32.TryParse("" + c /* TODO is there a faster way to do that? */, out cell);
                if (succeed)
                {
                    if (cell >= Rules.PossibleStates)
                    {
                        cell = Rules.PossibleStates - 1;
                    }
                }
                else
                {
                    cell = 0;
                } //TODO need an event system for logging incorrect input without the need for printing from backend. 

                specificOrigin[counter] = cell;
                counter++;
            }

            return specificOrigin;
        }
        

        ///<summary>Sets the Origin with cells of random states.</summary>
        public void setOriginRandomCells()
        {
            Origin = new int[SizeOfBoard];
            for (int i = 0; i < SizeOfBoard; i++)
            {
                Origin[i] = Tools.Rand.Next(Rules.PossibleStates);
            }
            Initialize();
        }

        ///<summary>Initializes the Automata, then simulates for the default number of steps.</summary>
        public void Go()
        {
            Go(DEFAULT_NUMBER_OF_STEPS); //TODO rename Go to Start to reduce ambiguity.
        }

        ///<summary>Initializes the Automata, then simulates for a given number of steps.</summary>
        public void Go(int theNumberOfSteps)
        {
            Initialize();
            Proceed(theNumberOfSteps);
        }

        ///<summary>Clear the Automata of any previously simulated steps, add Origin as first generation.</summary>
        public void Initialize()
        {
            Generations.Clear();
            Generations.Add(Origin);
            NeighborhoodGenerations.Clear();
            NeighborhoodGenerations.Add(new int[SizeOfBoard]);
            Analysis = new Analysis1D(Generations, NeighborhoodGenerations, Rules);
        }

        ///<summary>Iterate forward the default number of steps.</summary>
        public void Proceed()
        {
            Proceed(DEFAULT_NUMBER_OF_STEPS);
        }

        /// <summary>Iterate forward a given number of steps.</summary>
        /// <param name="theNumberOfSteps"></param>
        public void Proceed(int theNumberOfSteps)
        {
            int[] next = Generations[Generations.Count - 1];
            for (int i = 0; i < theNumberOfSteps; i++)
            {
                next = NewGeneration(next);
                Generations.Add(next);
            }
            Analysis = new Analysis1D(Generations, NeighborhoodGenerations, Rules);
        }

        ///<summary>Produces and returns a new generation based on the state of the given generation.</summary>
        public int[] NewGeneration(int[] theGen)
        {
            int[] newCells = new int[SizeOfBoard];
            int[] cellLookup = new int[SizeOfBoard];
            for (int i = 0; i < theGen.Length; i++)
            {
                //TODO alternate way that may be more efficient: neighborhood lookup is a public value in Rule that is written every time the rule() function is called.
                getNeighboorhood(i, theGen);
                int[] cellsAndLookup = Rules.rule(LocalSituation);
                newCells[i] = cellsAndLookup[0];
                cellLookup[i] = cellsAndLookup[1];
            }
            NeighborhoodGenerations.Add(cellLookup);
            return newCells;
        }

        ///<summary>Finds the cells which occupy the given cell's neighborhood.</summary>
        public void getNeighboorhood(int theIndex, int[] theGen)
        {
            for (int n = 0; n < Rules.NeighborhoodSize; n++)
            {
                int location = Rules.NeighborhoodCoordinates[n] + theIndex;
                if (location >= 0 && location < SizeOfBoard) //is not out of bounds
                {
                    LocalSituation[n] = theGen[location];
                }
                else
                {
                    switch (CurrentEdgeSetting)
                    {
                        case EdgeSettings.HardEdges:
                            LocalSituation[n] = 0; //dead out of bounds
                            break;
                        case EdgeSettings.WraparoundEdges:
                            LocalSituation[n] = theGen[WraparoundIndexes[location]];
                            break;
                    }
                }
            }
        }


        public void RefreshDisplay()
        {
            if(DisplayNeighborhoodLookup)
            {
                Imager.GenerateImage(NeighborhoodGenerations);
            } else
            {
                Imager.GenerateImage(Generations);
            }            
        }
               
        public override bool Equals(Object theOther)
        {
            // Check for null values and compare run-time types.
            if (theOther == null || GetType() != theOther.GetType())
                return false;

            Simulator1D otherAutomata = (Simulator1D)theOther;
            bool gensAreEqual = true;
            for (int i = 0; i < Generations.Count; i++)
            {
                if (!Generations[i].SequenceEqual(otherAutomata.Generations[i]))
                {
                    gensAreEqual = false;
                    break;
                }
            }
            return Imager.Equals(otherAutomata.Imager) && SizeOfBoard == otherAutomata.SizeOfBoard && gensAreEqual &&
                Origin.SequenceEqual(otherAutomata.Origin) && LocalSituation.SequenceEqual(otherAutomata.LocalSituation);
        }
    }
}
