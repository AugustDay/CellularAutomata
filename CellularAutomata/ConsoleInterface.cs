using CellularAutomata.OneDimensionalCA;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CellularAutomata
{
    class ConsoleInterface
    {
        private const string COMMAND_NOT_RECOGNIZED =
            "Command not recognized. Type 'help' for list of commands.";

        private const string USER_INPUT_SYMBOL = "> ";

        private const string WELCOME_PROMPT = "Welcome to the Cellular Automata (CA) simulator!\n";

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
            "o|output: saves CA as a BMP image and text file with info on its structure.\n" +
            "q|quit: ends the program."; 

        private Dictionary<string, int> Commands = new Dictionary<string, int>()
        {
            {"help", 0},
            {"about", 1 },
            {"quit", 2 },
            {"new", 3 },
            {"undo", 4 }, //TODO implement this and add to Help text.
            {"status", 5},
            {"go", 6},
            {"continue", 7},
            {"output", 8},

            //short versions:
            {"h", 0},
            {"a", 1 },
            {"q", 2 },
            {"n", 3 },
            {"u", 4 },
            {"s", 5},
            {"g", 6},
            {"c", 7},
            {"o", 8},
        };

        private const int MAXIMUM_HISTORY_SIZE = 5;

        private LinkedList<Automata1D> History = new LinkedList<Automata1D>();

        private Automata1D CurrentAutomata;

        public ConsoleInterface()
        {
            CurrentAutomata = new Automata1D();
            History.AddLast(CurrentAutomata);
        }

        public void Run()
        {
            bool keepGoing = true;
            string prompt = WELCOME_PROMPT + USER_INPUT_SYMBOL;
            string input = "";
            string[] arguments;
            Stopwatch sw = new Stopwatch();
            while (keepGoing)
            {
                Console.Write(prompt);
                prompt = USER_INPUT_SYMBOL;
                input = Console.ReadLine();
                if(input.Length > 0) //checks if the user just hit enter or input something.
                {
                    sw.Restart();
                    arguments = ParseInput(input).Where(s => !string.IsNullOrEmpty(s)).ToArray();
                    if (Commands.ContainsKey(arguments[0])) //input has valid argument.
                    {
                        switch(Commands[arguments[0]])
                        {
                            case 0: //help
                                Console.WriteLine(HELP_MESSAGE);
                                break;
                            case 1: //about
                                Console.WriteLine("'About' command not yet implemented.");
                                break;
                            case 2: //quit
                                keepGoing = false;                               
                                break;
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
                                CurrentAutomata.OuputAutomata();
                                break;
                        }
                        sw.Stop();
                        Console.WriteLine($"Command took: {sw.ElapsedMilliseconds} ms.");
                    } else
                    {
                        Console.WriteLine(COMMAND_NOT_RECOGNIZED);
                    }                    
                }            
            }
        }

        public void ContinueCommand(string[] theArguments)
        {
            if (theArguments.Length > 1)
            {
                int numberOfSteps;
                if (int.TryParse(theArguments[1], out numberOfSteps))
                {
                    CurrentAutomata.Proceed(numberOfSteps);
                }
                else
                {
                    Console.WriteLine("Failed to read the given number of steps. Using default instead.");
                    CurrentAutomata.Proceed();
                }
            }
            else //default
            {
                CurrentAutomata.Proceed();
            }
            CurrentAutomata.OuputAutomata();
        }

        public void GoCommand(string[] theArguments)
        {
            if(theArguments.Length > 1)
            {
                int numberOfSteps;
                if(int.TryParse(theArguments[1], out numberOfSteps))
                {
                    CurrentAutomata.Go(numberOfSteps);
                } else
                {
                    Console.WriteLine("Failed to read the given number of steps. Using default instead.");
                    CurrentAutomata.Go();
                }
            } else //default
            {
                CurrentAutomata.Go();
            }
            CurrentAutomata.OuputAutomata();
        }

        public void StatusCommand()
        {

            string status = "========================\nDimension: 1, Type: Elementary\n" +
                $"States: {CurrentAutomata.Rules.PossibleStates}, " +
                $"Neighborhood size: {CurrentAutomata.Rules.NeighborhoodSize}\n" +
                $"Neighborhood coordinates: {Tools.DisplayArray(CurrentAutomata.Rules.NeighborhoodCoordinates)}\n" +
                $"Rule Number: {CurrentAutomata.Rules.RuleNumber}_{CurrentAutomata.Rules.RuleBase}\n" +
                $"Current Generation: {CurrentAutomata.Generation}\n========================";
            Console.WriteLine(status);
        }

        public void NewCommand(string[] theArguments)
        {
            if(theArguments.Length > 1)
            {
                if(theArguments[1].Length > 2) //quotation marks
                {
                    CurrentAutomata = Tools.MakeAutomataFromCode(theArguments[1].Substring(1, theArguments[1].Length - 2));
                } else
                {
                    CurrentAutomata = null;
                }
            } else //no args, make default
            {
                CurrentAutomata = Tools.MakeAutomataFromCode("");
            }

            if (CurrentAutomata != null)
            {
                AddToHistory();
                Console.WriteLine("Found automata #" + CurrentAutomata.Rules.RuleNumber);
            } else
            {
                Console.WriteLine("Error: failed to parse parameters.");
                CurrentAutomata = History.Last();
            }
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
    }
}
