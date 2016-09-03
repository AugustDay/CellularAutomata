using CellularAutomata.OneDimensionalCA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDesk;
using NDesk.Options;
using ManyConsole;
using System.Numerics;

namespace CellularAutomata
{
    static class Tools
    {
        

        public static readonly Random Rand = new Random();

        public static string DecimalToStringBase(BigInteger theDecimal, uint theRadix)
        {
            string result;
            if (theDecimal > ulong.MaxValue)
            {
                result = DecimalToStringBase(theDecimal, theRadix);
                //do one thing
            } else
            {
                result = DecimalToStringBase((ulong)theDecimal, theRadix);
            }
            return result;
        }


        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <author>Pavel Vladov</author> //Thanks, StackOverflow!
        /// <param name="theDecimal">The number to convert.</param>
        /// <param name="theRadix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <returns></returns>
        private static string DecimalToStringBase(ulong theDecimal, uint theRadix)
        {
            const int BITS_IN_LONG = 64;
            const string DIGITS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (theRadix < 2 || theRadix > DIGITS.Length)
            {
                throw new ArgumentException("The radix must be >= 2 and <= " + DIGITS.Length.ToString());
            }

            if (theDecimal == 0)
            {
                return "0";
            } 

            int index = BITS_IN_LONG - 1;
            ulong currentNumber = theDecimal;
            char[] charArray = new char[BITS_IN_LONG];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % theRadix);
                charArray[index--] = DIGITS[remainder];
                currentNumber = currentNumber / theRadix;
            }

            string result = new String(charArray, index + 1, BITS_IN_LONG - index - 1);

            return result;
        }

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// </summary>
        /// <author>Pavel Vladov</author>
        /// <param name="number">The arbitrary numeral system number to convert.</param>
        /// <param name="radix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <returns></returns>
        public static double ArbitraryToDecimalSystem(string number, int radix)
        {
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                    Digits.Length.ToString());

            if (String.IsNullOrEmpty(number))
                return 0;

            // Make sure the arbitrary numeral system number is in upper case
            number = number.ToUpperInvariant();

            double result = 0;
            double multiplier = 1;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                char c = number[i];
                if (i == 0 && c == '-')
                {
                    // This is the negative sign symbol
                    result = -result;
                    break;
                }

                int digit = Digits.IndexOf(c);
                if (digit == -1)
                    throw new ArgumentException(
                        "Invalid character in the arbitrary numeral system number",
                        "number");

                result += digit * multiplier;
                multiplier *= radix;
            }

            return result;
        }

        public static Automata1D MakeAutomataFromCode(string theSpecification)
        {
            //string sampleSpec = "k=3 n={-1,0,1} r=1234567 b=400";
            string[] testString = theSpecification.Split(' ');
            int k = Rules1D.DEFAULT_POSSIBLE_STATES;
            int b = Automata1D.DEFAULT_SIZE_OF_BOARD;
            double rule = 30;
            int[] neighborhoodCoordinates = Rules1D.DEFAULT_NEIGHBORHOOD_ORIENTATION;

            foreach (string s in testString)
            {
                switch(s[0]) //k, n, r, b, etc...
                {
                    case 'k':
                        int.TryParse(s.Substring(2), out k);
                        break;
                    case 'n':
                        string[] sub = s.Substring(2).Split(new char[] { '{', ',', '}' }, StringSplitOptions.RemoveEmptyEntries);
                        neighborhoodCoordinates = new int[sub.Length];
                        for(int i = 0; i < neighborhoodCoordinates.Length; i++)
                        {
                            int.TryParse(sub[i], out neighborhoodCoordinates[i]);
                        }
                        Array.Sort(neighborhoodCoordinates);
                        break;
                    case 'r':
                        double.TryParse(s.Substring(2), out rule);
                        break;
                    case 'b':
                        int.TryParse(s.Substring(2), out b);
                        break;
                    default:
                        Console.WriteLine("Failed to parse: \"" + s);
                        break;
                }
            }

            //Automata1D theAutomata;
            //try
            //{
            //    theAutomata = new Automata1D(/* parameters from above. */);
            //} catch (ArgumentException)
            //{
            //    theAutomata = null;
            //}

            return null;
        }
    }
}
