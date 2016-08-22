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

        public int[] myRule;

        public int myNeighborhoodSize = 3;

        public int myPossibleStates = 2;

        public Rules1D(int[] theRules, int theNeighborhoodSize, int thePossibleStates)
        {
            myRule = new int[theRules.Length];
            int i = 0;
            foreach(int n in theRules.Reverse())
            {
                myRule[i] = n;
                i++;
            }
            myNeighborhoodSize = theNeighborhoodSize;
            myPossibleStates = thePossibleStates;
        }

        public Rules1D()
        {
            myRule = new int[] { 0, 1, 1, 1, 1, 0, 0, 0 }; //defaults to Rule 30
            myNeighborhoodSize = 3;
            myPossibleStates = 2;
        }

        public void setNewAutomataRule(int[] theRule, int theNeighborhoodSize, int thePossibleStates)
        {
            //TODO error / invalid input checking
            myRule = theRule;
            myNeighborhoodSize = theNeighborhoodSize;
            myPossibleStates = thePossibleStates;
        }

        //TODO should a neighborhood be its own object?

        public int rule(int[] neighborhood)
        {
            int number = 0;
            int power = 0;
            foreach(int b in neighborhood.Reverse())
            {
                if (b > 0)
                {
                    number += b * Convert.ToInt32(Math.Pow(myPossibleStates, power));
                }
                power++;
            }
            
            return myRule[number]; //TODO this is a problem with the .Reverse() stuff
        }

        public string toString()
        {
            int number = 0;
            int power = 0;
            foreach(int b in myRule)
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
