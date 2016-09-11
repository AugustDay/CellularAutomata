using CellularAutomata.OneDimensionalCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Numerics;

namespace CellularAutomata
{
    public enum ImagerGridSettings1D
    {
        NoGrid,
        Grid,
        GridOnLive,
    }

    class Program
    {
        static void Main(string[] args)
        {
            string testInput = "k=3 n={-1,0,1} r=1537550572281_10 b=30";
            Simulator1D cellualrAutomata30 = Tools.MakeAutomataFromCode(testInput);
            cellualrAutomata30.Go(15);

            foreach (string s in cellualrAutomata30.Imager.displayCA((cellualrAutomata30.CellularAutomata)))
            {
                Console.Write("theList.Add(new int[] { ");
                foreach(char c in s.ToCharArray())
                {
                    Console.Write(c + ", ");
                }
                Console.WriteLine("});");
            }
            Console.WriteLine("\n\n\n");
            

            Console.Title = "Cellular Automata Simulator";
            ConsoleInterface userInterface = new ConsoleInterface();
            userInterface.Run();

            Console.WriteLine("Program complete. Press any key to close window.");
            Console.ReadKey();
        }

        static void EveryRule(Simulator1D theAutomata)
        {
           //create a simple Arbitrary Number class with an array the size of the rule you want. 
           //keep adding 1 to it, with proper code handling carries into higher digit places.
           //when the last digit overflows, you've done every permutation of that array.
        }
    }
}
