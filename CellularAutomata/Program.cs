using CellularAutomata.OneDimensionalCA;
using System;

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
            ConsoleInterface userInterface = new ConsoleInterface();
            userInterface.Run();
            //test   
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
