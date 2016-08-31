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

        private static readonly int[] DEFAULT_RULE_ARRAY = { 0, 1, 1, 1, 1, 0, 0, 0 }; //defaults to Rule 30

        private static readonly int[] DEFAULT_NEIGHBORHOOD_ORIENTATION = { -1, 0, 1 }; //left, center, right

        private static readonly int DEFAULT_NEIGHBORHOOD_SIZE = 3;

        private static readonly int DEFAULT_POSSIBLE_STATES = 3;

        public int[] RuleArray;

        public int[] NeighborhoodOrientation;

        public int NeighborhoodSize;

        public int PossibleStates;

        public Rules1D(int[] theRules, int theNeighborhoodSize, int thePossibleStates)
        {
            RuleArray = new int[theRules.Length];
            int i = 0;
            foreach(int n in theRules.Reverse())
            {
                RuleArray[i] = n;
                i++;
            }
            NeighborhoodSize = theNeighborhoodSize;
            PossibleStates = thePossibleStates;
        }

        public Rules1D()
        {
            NeighborhoodOrientation = DEFAULT_NEIGHBORHOOD_ORIENTATION;
            NeighborhoodSize = DEFAULT_NEIGHBORHOOD_SIZE;
            PossibleStates = DEFAULT_POSSIBLE_STATES;

            //RuleArray = DEFAULT_RULE_ARRAY;
            setRandomRule();
        }

        public void setNewAutomataRule(int[] theRule, int theNeighborhoodSize, int thePossibleStates)
        {
            //TODO error & invalid input checking
            RuleArray = theRule;
            NeighborhoodSize = theNeighborhoodSize;
            PossibleStates = thePossibleStates;
        }

        public void setRandomRule(int thePossibleStates)
        {
            PossibleStates = thePossibleStates;
            setRandomRule();
        }

        public void setRandomRule()
        {            
            //TODO will need to change depending on rule type (totalistic rules different permutations)
            long numberOfCombinations = (long)Math.Pow(PossibleStates, NeighborhoodSize);
            RuleArray = new int[numberOfCombinations];
            Tools.Rand.Next(PossibleStates);
            for (int i = 0; i < numberOfCombinations; i++)
            {
                RuleArray[i] = Tools.Rand.Next(PossibleStates);
            }
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
            s += " }\nNumber: " + GetRuleNumber();
            return s;
        }

        

        public string GetRuleNumber()
        {
            BigInteger number = new BigInteger(0);
            long power = 0;
            //TODO switch to ulong/BigNum for rule number calculation! Possible permutations too large to hold in int!
            foreach (int b in RuleArray)
            {
                if (b > 0)
                {
                    number += b * Convert.ToInt32(Math.Pow(2, power)); //TODO 4-state rules can cause overflow from power being too large
                    //TODO large numbers can be too big for filename path; maybe switch to hex base to condense?
                }
                power++;
            }
            return number.ToString();
        }

        public string toString()
        {
            return "1D-Rule " + GetRuleNumber();
        }
    }
}
