using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model.RomanNumerals
{
    public static class Roman
    {
        public static List<string> romanNumerals = new List<string>() { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        public static List<int> numerals = new List<int>() { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

        public static string ToRoman(int number)
        {
            var romanNumeral = string.Empty;
            while (number > 0)
            {
                // find biggest numeral that is less than equal to number
                var index = numerals.FindIndex(x => x <= number);
                // subtract it's value from your number
                number -= numerals[index];
                // tack it onto the end of your roman numeral
                romanNumeral += romanNumerals[index];
            }
            return romanNumeral;
        }

        public static int FromRoman(string roman)
        {
            roman = roman.ToLower();
            string literals = "mdclxvi";
            int value = 0, index = 0;
            foreach (char literal in literals)
            {
                value = romanValue(literals.Length - literals.IndexOf(literal) - 1);
                index = roman.IndexOf(literal);
                if (index > -1)
                    return FromRoman(roman.Substring(index + 1)) + (index > 0 ? value - FromRoman(roman.Substring(0, index)) : value);
            }
            return 0;
        }

        private static int romanValue(int index)
        {
            int basefactor = ((index % 2) * 4 + 1); // either 1 or 5...
                                                    // ...multiplied with the exponentation of 10, if the literal is `x` or higher
            return index > 1 ? (int)(basefactor * System.Math.Pow(10.0, index / 2)) : basefactor;
        }
    }
}
