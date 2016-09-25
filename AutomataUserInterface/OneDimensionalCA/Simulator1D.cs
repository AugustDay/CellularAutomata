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

        public int DEFAULT_NUMBER_OF_STEPS = 200;

        ///<summary>The starting orientation of cells in a new simulation.</summary>
        private int[] Origin;

        ///<summary>The state of the cells around a given cell.</summary>
        private int[] LocalSituation;

        private Dictionary<int, int> WraparoundIndexes;

        ///<summary>Each generation of this Automata, [0] being origin.</summary>
        public List<int[]> Generations { get; private set; }

        ///<summary>The imager object that the Automata can use to output results.</summary>
        public Imager1D Imager { get; private set; }

        ///<summary>Rules for the Automata.</summary>
        public Rules1D Rules { get; private set; }

        ///<summary>The number of cells on a row.</summary>
        public int SizeOfBoard { get; private set; }

        ///<summary>Total number of generations that the Automata has been simulated.</summary>
        public int StepNumber { get { return Generations.Count; } private set { } }

        public EdgeSettings CurrentEdgeSetting { get; private set; }

        public bool GenerateImageAfterSimulating = true;

        public bool AutomaticSaving = false;        

        ///<summary>No-argument constructor makes an Simulator with default parameters.</summary>
        public Simulator1D() 
        {
            Rules = new Rules1D();
            Imager = new Imager1D(Rules);
            Generations = new List<int[]>();
            ConstructorHelper(DEFAULT_SIZE_OF_BOARD, EdgeSettings.WraparoundEdges);
        }

        ///<summary>Constructs a simulator with the given objects.</summary>
        public Simulator1D(Rules1D theRules, Imager1D theImager, int theSizeOfBoard, EdgeSettings theSetting)
        {
            if(theRules == null || 
               theImager == null)
            {
                throw new ArgumentNullException("The Rule or Imager param was null");
            }
            Rules = theRules;
            Imager = theImager;
            Generations = new List<int[]>();
            ConstructorHelper(theSizeOfBoard, theSetting);
        }

        ///<summary>Initializes the components of the simulator.</summary>
        private void ConstructorHelper(int theSizeOfBoard, EdgeSettings theSetting)
        {            
            if(theSizeOfBoard < 1)
            {
                throw new ArgumentException("Board cannot be of size less than 1!");
            }
            LocalSituation = new int[Rules.NeighborhoodSize];
            SizeOfBoard = theSizeOfBoard;
            Origin = new int[SizeOfBoard];
            CurrentEdgeSetting = theSetting;
            if(CurrentEdgeSetting == EdgeSettings.WraparoundEdges)
            {
                CalculateWraparoundIndex();
            }
            setOriginSingleCell(); //TODO should be changeable from code in Tools. random or single should be part of info.txt!
            Initialize();
            Imager.GenerateImage(Generations);
        }

        private void CalculateWraparoundIndex()
        {
            WraparoundIndexes = new Dictionary<int, int>();
            foreach(int coord in Rules.NeighborhoodCoordinates)
            {
                if(Math.Abs(coord) > SizeOfBoard)
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
            setOriginSingleCell(1);
        }

        ///<summary>Sets Origin with a single live cell in the middle with the given state.</summary>
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

        ///<summary>Sets the Origin with cells of random states.</summary>
        public void setOriginRandomCells()
        {
            Origin = new int[SizeOfBoard];
            for(int i = 0; i < SizeOfBoard; i++)
            {
                Origin[i] = Tools.Rand.Next(Rules.PossibleStates);
            }
            Initialize();
        }

        ///<summary>Initializes the Automata, then simulates for the default number of steps.</summary>
        public void Go()
        {
            Go(DEFAULT_NUMBER_OF_STEPS);            
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
            if(GenerateImageAfterSimulating)
            {
                Imager.GenerateImage(Generations);
            }
            if (AutomaticSaving)
            {
                OutputAutomata();
            }
        }

        ///<summary>Produces and returns a new generation based on the state of the given generation.</summary>
        public int[] NewGeneration(int[] theGen)
        {
            int[] newCells = new int[SizeOfBoard];
            for (int i = 0; i < theGen.Length; i++)
            {
                getNeighboorhood(i, theGen);
                newCells[i] = Rules.rule(LocalSituation);
            }

            return newCells;
        }

        ///<summary>Finds the cells which occupy the given cell's neighborhood.</summary>
        public void getNeighboorhood(int theIndex, int[] theGen)
        {
            for (int n = 0; n < Rules.NeighborhoodSize; n++) 
            {
                int location = Rules.NeighborhoodCoordinates[n] + theIndex;
                if(location >= 0 && location < SizeOfBoard) //is not out of bounds
                { 
                    LocalSituation[n] = theGen[location];
                } else
                {
                    switch(CurrentEdgeSetting)
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

        ///<summary>Saves the current state of the Automata.</summary>
        public void OutputAutomata()
        {
            Imager.SaveToFile();
        }

        public override bool Equals(Object theOther)
        {
            // Check for null values and compare run-time types.
            if (theOther == null || GetType() != theOther.GetType())
                return false;

            Simulator1D otherAutomata = (Simulator1D)theOther;
            bool gensAreEqual = true;
            for(int i = 0; i < Generations.Count; i++)
            {
                if(!Generations[i].SequenceEqual(otherAutomata.Generations[i]))
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
