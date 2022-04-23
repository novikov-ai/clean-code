using System;
using System.IO;

namespace CleanCode.VariableNames4
{
    static class AocDay1
    {
        public static void ShowResult()
        {
            // (1)
            // input - rawInput
            string rawInput = File.ReadAllText("input.txt");
            
            // (2)
            // allNums - allNumbers
            string[] allNumbers = rawInput.Split('\n');

            // (3)
            // firstNum - numberFirst
            string numberFirst = "";
            
            // (4)
            // secondNum - numberSecond
            string numberSecond = "";
            
            // (5)
            // thirdNum - numberThird
            string numberThird = "";
            
            // (6)
            // item - number
            foreach (string number in allNumbers)
            {
                if (numberThird.Length > 0)
                    break;

                // (7)
                // elem - numberNext
                foreach (string numberNext in allNumbers)
                {
                    if (numberThird.Length > 0)
                        break;

                    if (Convert.ToInt32(number) + Convert.ToInt32(numberNext) < 2020)
                    {
                        numberFirst = number;
                        numberSecond = numberNext;
                        
                        // (8)
                        // sum - numberAndNumberNextSum
                        var numberAndNumberNextSum = Convert.ToInt32(number) + Convert.ToInt32(numberNext);

                        // (9)
                        // el - numberNextNext
                        foreach (string numberNextNext in allNumbers)
                        {
                            if (numberNextNext != numberFirst && numberNextNext != numberSecond && Convert.ToInt32(numberNextNext) + numberAndNumberNextSum == 2020)
                            {
                                numberThird = numberNextNext;
                                break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Day 01: "+ Convert.ToInt32(numberFirst) * Convert.ToInt32(numberSecond) * Convert.ToInt32(numberThird));
        }
    }
}