using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CellularAutomata.OneDimensionalCA
{
    /// <summary>
    /// Object representing the current ruleset of the one-dimensional Automata.
    /// </summary>
    class Rules1D
    {
        //ruleset90 = { 0, 1, 0, 1, 1, 0, 1, 0 };
        //ruleset30 = { 0, 0, 0, 1, 1, 1, 1, 0 };
        //{0, 1, 1, 1, 1, 0, 0, 0};
        //TODO implement Observer/Observable to update, example: change in number of states = more brushes

        public static readonly int[] DEFAULT_RULE_ARRAY = { 0, 1, 1, 1, 1, 0, 0, 0 }; //defaults to Rule 30

        private static readonly int[] DEFAULT_RULE_ARRAY_3_STATES = { 0, 2, 2, 1, 0, 0, 2, 1, 2, 2, 2, 1, 2, 1, 1, 0, 0, 2, 2, 2, 2, 2, 0, 1, 2, 1, 0 }; //defaults to Rule 30

        public static readonly int[] DEFAULT_NEIGHBORHOOD_ORIENTATION = { -1, 0, 1 }; //left, center, right

        public static readonly int DEFAULT_POSSIBLE_STATES = 3;

        public int[] RuleArray;

        public int[] NeighborhoodCoordinates;

        public int NeighborhoodSize;

        public int PossibleStates; 

        public string RuleNumber { get; private set; }

        public int RuleBase { get; private set; }

        public Rules1D(BigInteger theRuleNumber, int[] theNeighborhoodCoordinates, int thePossibleStates)
        {
            if(theNeighborhoodCoordinates.Length == 0 || //no neighborhood
               thePossibleStates < 2 || //must have at least two states
               theRuleNumber > LargestPossibleRuleNumber(thePossibleStates, theNeighborhoodCoordinates.Length) || //too big
               theRuleNumber.Sign < 0) //negative rule
            {
                throw new ArgumentException();
            }
            NeighborhoodCoordinates = theNeighborhoodCoordinates;
            NeighborhoodSize = NeighborhoodCoordinates.Length;
            PossibleStates = thePossibleStates;

            CalculateRuleArrayFromBigInteger(theRuleNumber);
            RuleNumber = theRuleNumber.ToString(); //base ten right now!
            RuleBase = 10;
        }

        public Rules1D(int[] theRuleArray, int[] theNeighborhoodOrientation, int thePossibleStates)
        {
            RuleArray = new int[theRuleArray.Length];
            int i = 0;
            foreach(int n in theRuleArray.Reverse())
            {
                RuleArray[i] = n;
                i++;
            }
            //TODO needs to pass orientation instead of size
            //TODO deprecate this method, or fix it up!
            //TODO get rid of this constructor; no more passing an int array into the Rules class, must make rule from number!
            NeighborhoodCoordinates = theNeighborhoodOrientation;
            NeighborhoodSize = NeighborhoodCoordinates.Length;
            PossibleStates = thePossibleStates;
            CalculateRuleNumber();
        }

        public Rules1D()
        {
            NeighborhoodCoordinates = DEFAULT_NEIGHBORHOOD_ORIENTATION;
            NeighborhoodSize = NeighborhoodCoordinates.Length;
            PossibleStates = DEFAULT_POSSIBLE_STATES;

            //RuleArray = DEFAULT_RULE_ARRAY;
            setRandomRule();
            //CalculateRuleNumber();
        }

        public void setNewAutomataRule(int[] theRule, int[] theNeighborhoodOrientation, int thePossibleStates)
        {
            //TODO error & invalid input checking
            RuleArray = theRule;
            NeighborhoodCoordinates = theNeighborhoodOrientation;
            NeighborhoodSize = NeighborhoodCoordinates.Length;
            PossibleStates = thePossibleStates;
            CalculateRuleNumber();
        }

        public void setRandomRule(int thePossibleStates)
        {
            PossibleStates = thePossibleStates;
            setRandomRule();
        }

        public void setRandomRule() //TODO make this call setNewAutomataRule?
        {            
            //TODO will need to change depending on rule type (totalistic rules different permutations)
            long numberOfCombinations = (long)Math.Pow(PossibleStates, NeighborhoodSize);
            RuleArray = new int[numberOfCombinations];
            Tools.Rand.Next(PossibleStates);
            for (int i = 0; i < numberOfCombinations; i++)
            {
                RuleArray[i] = Tools.Rand.Next(PossibleStates);
            }
            CalculateRuleNumber();
        }

        //TODO should a neighborhood be its own object?

        public int rule(int[] theNeighborhood)
        {
            long number = 0;
            long power = 0;
            foreach(int b in theNeighborhood.Reverse())
            {
                if (b > 0)
                {
                    number += b * Convert.ToInt32(Math.Pow(PossibleStates, power));
                }
                power++;
            }
            
            return RuleArray[number]; //TODO this is a problem with the .Reverse() stuff
        }

        public string GetInfo()
        {
            string s = "1-Dimensional, Elementary\n";
            s += "Possible States: " + PossibleStates + "\n";
            s += "Neighborhood Size: " + NeighborhoodSize + ", Orientation: { ";
            foreach(int n in NeighborhoodCoordinates)
            {
                s += n + ", ";
            }
            s += "}\nRule: {"; //LITTLE ENDIAN.
            string c = "";
            for(int i = RuleArray.Length - 1; i >= 0 ; i--)
            {
                c = Tools.DecimalToStringBase(i, PossibleStates);
                while(c.Length < NeighborhoodSize)
                {
                    c = "0" + c;
                }
                s += "\n" + c + " -> " + RuleArray[i];
                c = "";
            }
            s += " }\nNumber: " + RuleNumber + "_" + RuleBase;
            return s;
        }

        private BigInteger LargestPossibleRuleNumber(int thePossibleStates, int theNeighborhoodSize)
        { //k^(k^n)
            return BigInteger.Pow(thePossibleStates, (int)Math.Pow(thePossibleStates, theNeighborhoodSize));
        }

        private void CalculateRuleArrayFromBigInteger(BigInteger theNumber)
        {

            string ruleString = Tools.LargeDecimalToStringBase(theNumber, PossibleStates);
            RuleArray = new int[(int)Math.Pow(PossibleStates, NeighborhoodSize)];
            int n = 0;
            for (int i = ruleString.Length - 1; i >= 0; i--) //TODO bigEndian!
            {
                RuleArray[i] = ruleString[n] - '0';
                n++;
            }
        }

        private void CalculateRuleNumber()
        {
            BigInteger number = new BigInteger(0);
            long power = 0;
            //TODO switch to ulong/BigNum for rule number calculation! Possible permutations too large to hold in int!
            foreach (int b in RuleArray)
            {
                if (b > 0)
                {
                    number += new BigInteger(b * Math.Pow(PossibleStates, power)); //TODO 4-state rules can cause overflow from power being too large
                    //TODO should perhaps use converter to do this? Need a method to go from arbitrary to arbitrary (rather than to 10)
                    //TODO large numbers can be too big for filename path; maybe switch to hex base to condense?
                }
                power++;
            }
            RuleNumber = number.ToString();
            RuleBase = 10;
        }

        public string toString()
        {
            return "1D-Rule " + RuleNumber;
        }
    }
}
