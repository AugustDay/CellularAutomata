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

        private static readonly int[] __DEFAULT_RULE_ARRAY = { 0, 1, 1, 1, 1, 0, 0, 0 }; //defaults to Rule 30

        private static readonly int[] DEFAULT_RULE_ARRAY = { 0, 2, 2, 1, 0, 0, 2, 1, 2, 2, 2, 1, 2, 1, 1, 0, 0, 2, 2, 2, 2, 2, 0, 1, 2, 1, 0 }; //defaults to Rule 30

        private static readonly int[] DEFAULT_NEIGHBORHOOD_ORIENTATION = { -1, 0, 1 }; //left, center, right

        private static readonly int DEFAULT_NEIGHBORHOOD_SIZE = 3;

        private static readonly int DEFAULT_POSSIBLE_STATES = 3;

        public int[] RuleArray;

        public int[] NeighborhoodOrientation;

        public int NeighborhoodSize;

        public int PossibleStates;

        public string RuleNumber { get; private set; }

        public Rules1D(int[] theRules, int theNeighborhoodSize, int thePossibleStates)
        {
            RuleArray = new int[theRules.Length];
            int i = 0;
            foreach(int n in theRules.Reverse())
            {
                RuleArray[i] = n;
                i++;
            }
            //TODO needs to pass orientation instead of size
            NeighborhoodSize = theNeighborhoodSize;
            PossibleStates = thePossibleStates;
            CalculateRuleNumber();
        }

        public Rules1D()
        {
            NeighborhoodOrientation = DEFAULT_NEIGHBORHOOD_ORIENTATION;
            NeighborhoodSize = DEFAULT_NEIGHBORHOOD_SIZE;
            PossibleStates = DEFAULT_POSSIBLE_STATES;

            //RuleArray = DEFAULT_RULE_ARRAY;
            setRandomRule();
            //CalculateRuleNumber();
        }

        public void setNewAutomataRule(int[] theRule, int[] theNeighborhoodOrientation, int thePossibleStates)
        {
            //TODO error & invalid input checking
            RuleArray = theRule;
            NeighborhoodOrientation = theNeighborhoodOrientation;
            NeighborhoodSize = NeighborhoodOrientation.Length;
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
            foreach(int n in NeighborhoodOrientation)
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
            s += " }\nNumber: " + RuleNumber;
            return s;
        }

        

        private void CalculateRuleNumber()
        {
            BigInteger number = new BigInteger(0);
            double d = 5;
            long power = 0;
            //TODO switch to ulong/BigNum for rule number calculation! Possible permutations too large to hold in int!
            foreach (int b in RuleArray)
            {
                if (b > 0)
                {
                    number += new BigInteger(b * Math.Pow(PossibleStates, power)); //TODO 4-state rules can cause overflow from power being too large
                    //TODO large numbers can be too big for filename path; maybe switch to hex base to condense?
                }
                power++;
            }
            RuleNumber = number.ToString();
        }

        public string toString()
        {
            return "1D-Rule " + RuleNumber;
        }
    }
}
