using CellularAutomata.OneDimensionalCA;
using System;
using System.Numerics;
using AutomataUserInterface;
using System.Windows.Media;

namespace CellularAutomata
{
    public static class Tools
    {
        public static Random Rand = new Random(); //no longer readonly, for testing purposes.

        public static Brush PositiveColor = Brushes.Blue;

        public static Brush NeutralColor = Brushes.Black;

        public static Brush ErrorColor = Brushes.Red;

        public static Brush GreenColor = Brushes.Green;

        public static MainWindow OutputWindow;

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <author>Pavel Vladov</author> //Thank you!
        /// http://www.pvladov.com/2012/05/decimal-to-arbitrary-numeral-system.html
        /// <param name="theDecimal">The number to convert.</param>
        /// <param name="theRadix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <returns></returns>
        public static string DecimalToStringBase(long theDecimal, int theRadix)
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
            long currentNumber = Math.Abs(theDecimal);
            char[] charArray = new char[BITS_IN_LONG];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % theRadix);
                charArray[index--] = DIGITS[remainder];
                currentNumber = currentNumber / theRadix;
            }

            string result = new String(charArray, index + 1, BITS_IN_LONG - index - 1);
            if (theDecimal < 0)
            {
                result = "-" + result;
            }

            return result;
        }

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// </summary>
        /// <author>Pavel Vladov</author>
        /// http://www.pvladov.com/2012/07/arbitrary-to-decimal-numeral-system.html
        /// <param name="theNumberString">The arbitrary numeral system number to convert.</param>
        /// <param name="theRadix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <returns></returns>
        public static long ArbitraryToDecimalSystem(string theNumberString, int theRadix)
        {
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (theRadix < 2 || theRadix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                    Digits.Length.ToString());

            if (String.IsNullOrEmpty(theNumberString))
                return 0;

            // Make sure the arbitrary numeral system number is in upper case
            theNumberString = theNumberString.ToUpperInvariant();

            long result = 0;
            long multiplier = 1;
            for (int i = theNumberString.Length - 1; i >= 0; i--)
            {
                char c = theNumberString[i];
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
                multiplier *= theRadix;
            }

            return result;
        }

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]). 
        /// This version uses BigInteger (and is meant for very large values).
        /// </summary>
        /// <param name="theDecimal">The number to convert.</param>
        /// <param name="theRadix">The radix of the destination numeral system (in the range [2, 36]).</param>
        public static string LargeDecimalToStringBase(BigInteger theDecimal, int theRadix)
        {
            if (theDecimal.IsZero)
            {
                return "0";
            }
            string result;
            if (theDecimal > long.MaxValue)
            {
                const int BITS = 200;
                const string DIGITS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                if (theRadix < 2 || theRadix > DIGITS.Length)
                {
                    throw new ArgumentException("The radix must be >= 2 and <= " + DIGITS.Length.ToString());
                }

                int index = BITS - 1;
                BigInteger currentNumber = theDecimal;
                char[] charArray = new char[BITS];

                while (currentNumber != 0)
                {
                    int remainder = (int)(currentNumber % theRadix);
                    charArray[index--] = DIGITS[remainder];
                    currentNumber = currentNumber / theRadix;
                }

                result = new String(charArray, index + 1, BITS - index - 1);
            }
            else
            {
                result = DecimalToStringBase((long)theDecimal, theRadix);
            }
            return result;
        }

        /// <summary>
        /// Converts the given number from the numeral system with the specified
        /// radix (in the range [2, 36]) to decimal numeral system.
        /// This version uses BigInteger (and is meant for very large values).
        /// </summary>
        /// <param name="theNumberString">The arbitrary numeral system number to convert.</param>
        /// <param name="theRadix">The radix of the numeral system the given number
        /// is in (in the range [2, 36]).</param>
        /// <returns></returns>
        public static BigInteger LargeArbitraryToDecimalSystem(string theNumberString, int theRadix)
        {
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (theRadix < 2 || theRadix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " +
                    Digits.Length.ToString());

            if (String.IsNullOrEmpty(theNumberString))
                return 0;

            if (theRadix == 10) //already in base 10!
            {
                BigInteger quickResult;
                BigInteger.TryParse(theNumberString, out quickResult);
                return quickResult;
            }

            // Make sure the arbitrary numeral system number is in upper case
            theNumberString = theNumberString.ToUpperInvariant();

            BigInteger result = 0;
            BigInteger multiplier = 1;
            for (int i = theNumberString.Length - 1; i >= 0; i--)
            {
                char c = theNumberString[i];
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
                multiplier *= theRadix;
            }
            
            return result;
        }

        public static Simulator1D MakeAutomataFromCode(string theSpecification)
        {
            //string sampleSpec = "k=3 n={-1,0,1} r=1234567_10 b=400";
            if(theSpecification == null || theSpecification.Length == 0)
            {
                return new Simulator1D();
            }
            string[] testString = theSpecification.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int k = Rules1D.DEFAULT_POSSIBLE_STATES;
            int b = Simulator1D.DEFAULT_SIZE_OF_BOARD;
            int[] ruleArray = Rules1D.DEFAULT_RULE_ARRAY;
            BigInteger big = new BigInteger(-1);
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
                        try
                        {
                            string[] numberCode = s.Substring(2).Split('_'); //[0] is array, [1] is base
                            int radix;
                            int.TryParse(numberCode[1], out radix);
                            big = LargeArbitraryToDecimalSystem(numberCode[0], radix);
                        } catch (IndexOutOfRangeException)
                        {
                            DisplayMessage("Failed to parse rule number: \"" + s + "\"\n", ErrorColor);
                        }
                        break;
                    case 'b':
                        int.TryParse(s.Substring(2), out b);
                        break;
                    default:
                        DisplayMessage("Failed to parse parameter: \"" + s + "\"\n", ErrorColor);
                        break;
                }
            }
            //just pass in rule number in base 10 form, no conversion in here
            //int[] nArray = null;
            //if(nArray == null)
            //{
            //}

            Simulator1D theAutomata;
            try
            {
                Rules1D daRules;
                if (big.Sign < 0)
                {
                    daRules = new Rules1D(neighborhoodCoordinates, k);
                } else
                {
                    daRules = new Rules1D(big, neighborhoodCoordinates, k);
                }
                Imager1D daImager = new Imager1D(daRules);
                theAutomata = new Simulator1D(daRules, daImager, b);
            }
            catch (ArgumentException)
            {
                DisplayMessage("Error: something went wrong with constructing the new Automata.\n", ErrorColor);
                theAutomata = null;
            }

            return theAutomata;
        }

        public static void DisplayMessageLine(string theMessage)
        {
            OutputWindow.addSomeColoredText(theMessage + "\n", NeutralColor);
        }

        public static void DisplayMessageLine(string theMessage, Brush theColor)
        {
            OutputWindow.addSomeColoredText(theMessage + "\n", theColor);
        }

        public static void DisplayMessage(string theMessage)
        {
            OutputWindow.addSomeColoredText(theMessage, NeutralColor);
        }

        public static void DisplayMessage(string theMessage, Brush theColor)
        {
            OutputWindow.addSomeColoredText(theMessage, theColor);
        }

        public static string ArrayToString(int[] theArray)
        {
            string result = "{";
            for(int i = 0; i < theArray.Length - 1; i++)
            {
                result += theArray[i] + ",";
            }
            result += theArray[theArray.Length - 1] + "}";
            return result;
        }

        public static string ArrayToStringWithSpaces(int[] theArray)
        {
            string result = "{ ";
            for (int i = 0; i < theArray.Length - 1; i++)
            {
                result += theArray[i] + ", ";
            }
            result += theArray[theArray.Length - 1] + " }";
            return result;
        }
    }
}
