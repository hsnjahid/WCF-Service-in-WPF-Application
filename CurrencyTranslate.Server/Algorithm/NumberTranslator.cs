using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CurrencyTranslater.Server.Algorithm
{
    /// <summary>
    /// This class translates number to words
    /// </summary>
    internal sealed class NumberTranslator
    {
        #region Fields

        // dictinary items represents ones into verbal words
        private static Dictionary<int, string> _onesMap = new Dictionary<int, string>()
        {
            { 0, "zero" },
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
            { 4, "four" },
            { 5, "five" },
            { 6, "six" },
            { 7, "seven" },
            { 8, "eight" },
            { 9, "nine" },
        };

        // dictinary items represents tens into verbal words
        private static Dictionary<int, string> _tensMap = new Dictionary<int, string>()
        {
            { 10, "ten" },
            { 11, "eleven" },
            { 12, "twelve" }, 
            { 13, "thirteen" }, 
            { 14, "fourteen" },
            { 15, "fifteen" },
            { 16, "sixteen" },
            { 17, "seventeen" },
            { 18, "eighteen" },
            { 19, "nineteen" },
            { 20, "twenty" },
            { 30, "thirty" },
            { 40, "forty" },
            { 50, "fifty" },
            { 60, "sixty" },
            { 70, "seventy" },
            { 80, "eighty" },
            { 90, "ninety" },
        };

        // dictinary items represents hundreds into verbal words
        private static Dictionary<int, string> _hundredsMap = new Dictionary<int, string>()
        {
            { 100, "hundred" },
            { 1000, "thousand" },
            { 1000000, "million" },
        };

        private const int _million = 1000000;  // defines million
        private const int _thousand = 1000; // defines thousand
        private const int _hundred = 100; // defines hundred
        private const int _ten = 10; // defines ten

        #endregion

        #region Static Methods

        /// <summary>
        /// Translates a given integer number into verbal words as string.
        /// </summary>
        public static string Translate(int number)
        {
            // handle zero 
            if (number == 0)
            {
                return _onesMap[number];
            }

            // handle minus
            if (number < 0)
            {
                return string.Format("minus {0}", Translate(Math.Abs(number)));
            }

            string words = "";
            int reminder;

            // handles million
            int result = Math.DivRem(number, _million, out reminder);

            if (result > 0)
            {
                // translates to words
                words += string.Format("{0} {1} ", Translate(result), _hundredsMap[_million]);
                number = reminder;
            }

            // handles thousand
            result = Math.DivRem(number, _thousand, out reminder);

            if (result > 0)
            {
                // translates to words
                words += string.Format("{0} {1} ", Translate(result), _hundredsMap[_thousand]);
                number = reminder;
            }

            // handles hundred
            result = Math.DivRem(number, _hundred, out reminder);

            if (result > 0)
            {
                // translates to words
                words += string.Format("{0} {1} ", Translate(result), _hundredsMap[_hundred]);
                number = reminder;
            }

            if (number > 0)
            {
                // handles ones
                if (number < 10)
                {
                    Debug.Assert(_onesMap.ContainsKey(number));
                    words += _onesMap[number];
                }
                // handles if number is less than 20
                else if (number < 20)
                {
                    Debug.Assert(_tensMap.ContainsKey(number));
                    words += _tensMap[number];
                }
                // handles if number is greater than or equal 20
                else
                {
                    result = Math.DivRem(number, _ten, out reminder);

                    Debug.Assert(_tensMap.ContainsKey(result * _ten));
                    words += _tensMap[result * _ten];

                    if (reminder > 0)
                    {
                        Debug.Assert(_onesMap.ContainsKey(reminder));
                        words += string.Format("-{0}", _onesMap[reminder]);
                    }
                }
            }

            return words;
        }

        #endregion
    }
}
