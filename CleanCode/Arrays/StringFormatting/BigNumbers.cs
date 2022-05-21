using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanCode.Arrays.StringFormatting
{
    public static class BigNumbers
    {
        public static string GetSum(string num1, string num2)
        {
            if (num1 is null || num2 is null || num1.Length == 0 || num2.Length == 0)
                throw new ArgumentNullException("Input argument is null.");

            // (3)
            // prev:
            // char[] num1Reversed = CheckInputAndReverse(num1);
            // char[] num2Reversed = CheckInputAndReverse(num2);
            //
            // char[] majorNum = num1Reversed.Length > num2Reversed.Length ? num1Reversed : num2Reversed;
            // char[] minorNum = num1Reversed.Length > num2Reversed.Length ? num2Reversed : num1Reversed;
            // what: array data structure replaced by generic List<char>
            
            List<char> num1Reversed = CheckInputAndReverse(num1);
            List<char> num2Reversed = CheckInputAndReverse(num2);

            List<char> majorNum = num1Reversed.Count > num2Reversed.Count ? num1Reversed : num2Reversed;
            List<char> minorNum = num1Reversed.Count > num2Reversed.Count ? num2Reversed : num1Reversed;

            var resultReversed = new StringBuilder();
            
            int sum;
            int carry = 0;

            // (3)
            // prev:
            // for (int i = 0; i < majorNum.Length; i++)
            // {
            //     sum = Int32.Parse(Char.ToString(majorNum[i])) + carry;
            //
            //     if (i < minorNum.Length)
            //         sum += Int32.Parse(Char.ToString(minorNum[i]));
            //
            //     if (sum > 9)
            //     {
            //         carry = sum / 10;
            //         resultReversed.Append(sum % 10);
            //     }
            //     else
            //     {
            //         carry = 0;
            //         resultReversed.Append(sum);
            //     }
            // }

            foreach (var letter in majorNum)
            {
                sum = Int32.Parse(Char.ToString(letter)) + carry;

                var letterIndex = majorNum.IndexOf(letter);
                if (letterIndex < minorNum.Count)
                    sum += Int32.Parse(Char.ToString(minorNum[letterIndex]));
                
                if (sum > 9)
                {
                    carry = sum / 10;
                    resultReversed.Append(sum % 10);
                }
                else
                {
                    carry = 0;
                    resultReversed.Append(sum);
                }
            }

            if (carry > 0)
                resultReversed.Append(carry);

            var result = new StringBuilder();

            for (int i = resultReversed.Length - 1; i >= 0; i--)
                result.Append(resultReversed[i]);

            return result.ToString();
        }

        // (3)
        // prev:
        // private static char[] CheckInputAndReverse(string input)
        // {
        //     char[] numReversed = input.Reverse().ToArray();
        //     if (!numReversed.All(Char.IsNumber))
        //         throw new ArgumentException("Input argument is not valid. String must represents only a number > 0");
        //     return numReversed;
        // }
        private static List<char> CheckInputAndReverse(string input)
        {
            var numReversed = input.Reverse().ToList();
            if (!numReversed.All(Char.IsNumber))
                throw new ArgumentException("Input argument is not valid. String must represents only a number > 0");
            return numReversed;
        }
    }
}