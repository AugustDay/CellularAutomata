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
            Console.Title = "Cellular Automata Simulator";
            ConsoleInterface ci = new ConsoleInterface();
            ci.Run();

            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            //Automata1D theAutomata = Tools.MakeAutomataFromCode("k=3 n={-1,0,1} r=1537550572281_10 b=400");

            //if (theAutomata != null)
            //{
            //    theAutomata.Go();
            //    theAutomata.setOriginRandomCells();
            //    theAutomata.Imager.PrintInfoText = false;
            //    theAutomata.Go();
            //}

            //sw.Stop();
            Console.WriteLine("Program complete. Press any key to close window.");
            //Console.WriteLine("Milliseconds elapsed: " + sw.ElapsedMilliseconds);
            Console.ReadKey();
        }

        static void EveryRule(Automata1D theAutomata)
        {
           //create a simple Arbitrary Number class with an array the size of the rule you want. 
           //keep adding 1 to it, with proper code handling carries into higher digit places.
           //when the last digit overflows, you've done every permutation of that array.
        }
    }
}
