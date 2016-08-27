using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata.OneDimensionalCA
{
    class Rules1D
    {
        //ruleset90 = { 0, 1, 0, 1, 1, 0, 1, 0 };
        //ruleset30 = { 0, 0, 0, 1, 1, 1, 1, 0 };
        //{0, 1, 1, 1, 1, 0, 0, 0};
        //TODO implement Observer/Observable to update, example: change in number of states = more brushes

        public int[] RuleArray;

        public int[] NeighborhoodOrientation;

        public int NeighborhoodSize = 3;

        public int PossibleStates = 2;

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
            RuleArray = new int[] { 0, 1, 1, 1, 1, 0, 0, 0 }; //defaults to Rule 30
            NeighborhoodOrientation = new int[] { -1, 0, 1 };
            NeighborhoodSize = 3;
            PossibleStates = 2;
        }

        public void setNewAutomataRule(int[] theRule, int theNeighborhoodSize, int thePossibleStates)
        {
            //TODO error / invalid input checking
            RuleArray = theRule;
            NeighborhoodSize = theNeighborhoodSize;
            PossibleStates = thePossibleStates;
        }

        //TODO should a neighborhood be its own object?

        public int rule(int[] theNeighborhood)
        {
            int number = 0;
            int power = 0;
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

        public string toString()
        {
            int number = 0;
            int power = 0;
            foreach(int b in RuleArray)
            {
                if(b > 0)
                {
                    number += b * Convert.ToInt32(Math.Pow(2, power));
                }
                power++;
            }

            return "1D-Rule " + number;
        }
    }
}
