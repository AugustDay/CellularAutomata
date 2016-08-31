using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellularAutomata
{
    static class Tools
    {
        public static readonly Random Rand = new Random();

        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <author>Pavel Vladov</author> //Thanks, StackOverflow!
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
    }
}
