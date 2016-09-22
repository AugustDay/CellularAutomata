using CellularAutomata.OneDimensionalCA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace CellularAutomata
{
    public class InterfaceBackend
    {
        private const string COMMAND_NOT_RECOGNIZED =
            "Command not recognized. Type 'help' for list of commands.";

        private const string USER_INPUT_SYMBOL = "> ";

        private const string WELCOME_PROMPT = "Welcome to the Cellular Automata (CA) simulator!\n" +
            "By: Austin Ingraham\n\nPress 'help' or simply 'h' for commands."; //TODO name here!

        private const string HELP_MESSAGE =
            "h|help: lists all commands.\n" +

            "a|about {topic}: describes different topics about automata\n" +
            "\tTopics: automata, states, neighborhood, etc. etc.\n" +
            "\tExample: > about automata\n\n" +

            "n|new \"{var1} {var2} {etc.}\": creates a new CA with the given parameters.\n\t" +
            "The quotes around the parameter list is necessary!\n\t" +
            "Any missing parameters are replaced with default settings.\n\t" +
            "Valid parameters: \n\tk=### number of states that a cell can take.\n\t" +
            "n={##, ##, ##, etc.} the neighborhood coordinates, normally {-1,0,1}.\n\t" +
            "r=###_## the rule number code, in the specified base.\n\t" +
            "b=### the number of cells on each row of the board.\n\t" +
            "Example: > new \"k=3 n={-1,0,1} r=1234567_10 b=400\"\n\t\n" +

            "s|status: prints the status of the current automata.\n" +
            "g|go {###}: initializes, runs ### generations of the CA, and saves result.\n" +
            "c|continue {###}: runs another ### generations of the CA, saving result.\n\t" +
            "For Go and Continue, program will output result if autosaving is ON.\n\n" + //TODO implement autosave setting.

            "m|many {###}: inits CA origin w/ both random and single-cell origins, runs Go.\n" +
            "o|output: saves CA as a BMP image and text file with info on its structure.\n" +
            "q|quit: ends the program.\n";

        private Dictionary<string, int> Commands = new Dictionary<string, int>()
        {
            {"help", 0 },
            {"about", 1 },
            {"quit", 2 },
            {"new", 3 },
            {"undo", 4 }, //TODO implement this and add to Help text.
            {"status", 5 },
            {"go", 6 },
            {"continue", 7 },
            {"output", 8 },
            {"many", 9 },

            //short versions:
            {"h", 0 },
            {"a", 1 },
            {"q", 2 },
            {"n", 3 },
            {"u", 4 },
            {"s", 5 },
            {"g", 6 },
            {"c", 7 },
            {"o", 8 },
            {"m", 9 }
        };

        private const int MAXIMUM_HISTORY_SIZE = 5;

        private LinkedList<Simulator1D> History = new LinkedList<Simulator1D>();

        public Simulator1D CurrentAutomata;

        private Stopwatch Watch;

        public InterfaceBackend()
        {
            CurrentAutomata = new Simulator1D();
            History.AddLast(CurrentAutomata);
            Watch = new Stopwatch();
            Tools.DisplayMessage(WELCOME_PROMPT);
        }

        public void Run(string theInput)
        {
            string[] arguments;
            if (theInput.Length > 0) //checks if the user just hit enter or input something.
            {
                Watch.Restart();
                arguments = ParseInput(theInput).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                if (arguments.Length > 0 && Commands.ContainsKey(arguments[0])) //input has valid argument.
                {
                    switch (Commands[arguments[0]])
                    {
                        case 0: //help
                            Tools.DisplayMessageLine(HELP_MESSAGE);
                            break;
                        case 1: //about
                            Tools.DisplayMessageLine("'About' command not yet implemented.");
                            break;
                        //case 2: //quit 
                        //    keepGoing = false; TODO Should close the window!
                        //    break;
                        case 3: //new
                            NewCommand(arguments);
                            break;
                        case 5: //status
                            StatusCommand();
                            break;
                        case 6:
                            GoCommand(arguments);
                            break;
                        case 7:
                            ContinueCommand(arguments);
                            break;
                        case 8:
                            CurrentAutomata.OutputAutomata();
                            break;
                        case 9:
                            ManyCommand(arguments);
                            break;
                    }
                    Watch.Stop();
                    Tools.DisplayMessageLine($"Command took: {Watch.ElapsedMilliseconds} ms.");
                }
                else
                {
                    Tools.DisplayMessageLine(COMMAND_NOT_RECOGNIZED);
                }                
            }
        }

        public void ManyCommand(string[] theArguments)
        {
            bool autoSaving = CurrentAutomata.AutomaticSaving;
            CurrentAutomata.AutomaticSaving = true;
            int numberOfSteps = GetNumberOfSteps(theArguments);
            for(int i = 1; i < CurrentAutomata.Rules.PossibleStates; i++)
            {
                CurrentAutomata.setOriginSingleCell(i);
                CurrentAutomata.Proceed(numberOfSteps);
                CurrentAutomata.Imager.PrintInfoText = false;
            }
            CurrentAutomata.setOriginRandomCells();
            CurrentAutomata.Proceed(numberOfSteps);
            CurrentAutomata.Imager.PrintInfoText = true;

            CurrentAutomata.AutomaticSaving = autoSaving;
        }

        public void ContinueCommand(string[] theArguments)
        {
            if (theArguments.Length > 1)
            {
                CurrentAutomata.Proceed(GetNumberOfSteps(theArguments));
            }
            else //default
            {
                CurrentAutomata.Proceed();
            }
        }

        public void GoCommand(string[] theArguments)
        {
            if (theArguments.Length > 1)
            {
                CurrentAutomata.Go(GetNumberOfSteps(theArguments));
            }
            else //default
            {
                CurrentAutomata.Go();
            }
        }

        public void StatusCommand()
        {

            string status = "========================\nDimension: 1, Type: Elementary\n" +
                $"States: {CurrentAutomata.Rules.PossibleStates}, " +
                $"Neighborhood size: {CurrentAutomata.Rules.NeighborhoodSize}\n" +
                $"Neighborhood coordinates: {Tools.ArrayToString(CurrentAutomata.Rules.NeighborhoodCoordinates)}\n" +
                $"Rule Number: {CurrentAutomata.Rules.RuleNumber}_{CurrentAutomata.Rules.RuleBase}\n" +
                $"Current Generation: {CurrentAutomata.StepNumber}\n========================";
            Tools.DisplayMessageLine(status);
        }

        public void NewCommand(string[] theArguments)
        {
            if (theArguments.Length > 1)
            {
                if (theArguments[1].Length > 2) //quotation marks
                {
                    CurrentAutomata = Tools.MakeAutomataFromCode(
                        theArguments[1].Substring(1, theArguments[1].Length - 2), CurrentAutomata);
                }
                else
                {
                    CurrentAutomata = null;
                }
            }
            else //no args, make default
            {
                CurrentAutomata = Tools.MakeAutomataFromCode("");
            }

            if (CurrentAutomata != null)
            {
                AddToHistory();
                Tools.DisplayMessageLine("Found automata #" + CurrentAutomata.Rules.RuleNumber);
            }
            else
            {
                Tools.DisplayMessageLine("Error: failed to parse parameters.", Tools.ErrorColor);
                CurrentAutomata = History.Last();
            }
        }

        private int GetNumberOfSteps(string[] theArguments)
        {
            int numberOfSteps;
            if (theArguments.Length == 1)
            {
                numberOfSteps = CurrentAutomata.DEFAULT_NUMBER_OF_STEPS;
            }
            else if (!int.TryParse(theArguments[1], out numberOfSteps))
            {
                Tools.DisplayMessageLine("Failed to read the given number of steps. Using default instead.");
                numberOfSteps = CurrentAutomata.DEFAULT_NUMBER_OF_STEPS;
            }

            return numberOfSteps;
        }

        private void AddToHistory()
        {
            if (History.Count >= MAXIMUM_HISTORY_SIZE)
            {
                History.RemoveFirst();
                History.AddLast(CurrentAutomata);
            }
            else
            {
                History.AddLast(CurrentAutomata);
            }
        }

        public string[] ParseInput(string theInput)
        {
            string[] array =
                Regex.Split(theInput, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
            array[0] = array[0].ToLower(); //insures that the command is lowercase.
            return array;
        }

        private void PrintWelcomeCA()
        {
            List<int[]> theList = new List<int[]>();
            theList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 1, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 2, 1, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 0, 0, 0, 2, 1, 2, 1, 1, 1, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 0, 0, 2, 1, 1, 0, 0, 1, 1, 2, 0, 2, 2, 0, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 0, 2, 1, 0, 2, 2, 2, 0, 1, 0, 2, 2, 2, 2, 0, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 0, 2, 1, 2, 1, 2, 0, 2, 2, 1, 1, 2, 0, 0, 2, 2, 0, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 0, 2, 1, 1, 0, 1, 0, 2, 2, 1, 0, 1, 0, 2, 2, 2, 2, 2, 0, 0, 0 });
            theList.Add(new int[] { 0, 0, 2, 1, 0, 2, 2, 1, 1, 2, 1, 2, 2, 1, 1, 2, 0, 0, 0, 2, 2, 0, 0 });
            theList.Add(new int[] { 0, 2, 1, 2, 1, 2, 1, 0, 1, 0, 1, 2, 1, 0, 1, 0, 2, 0, 2, 2, 2, 2, 0 });
            theList.Add(new int[] { 2, 1, 1, 0, 1, 0, 2, 2, 1, 2, 0, 0, 2, 2, 1, 1, 2, 2, 2, 0, 0, 2, 2 });
            foreach (int[] row in theList)
            {
                foreach(int c in row)
                {
                    switch(c)
                    {
                        case 0:
                            Console.BackgroundColor = ConsoleColor.Black;
                            break;
                        case 1:
                            Console.BackgroundColor = ConsoleColor.Blue;
                            break;
                        case 2:
                            Console.BackgroundColor = ConsoleColor.Green;
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
        }
    }
}
