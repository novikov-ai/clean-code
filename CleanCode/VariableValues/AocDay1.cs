using System;
using System.IO;

namespace CleanCode.VariableValues
{
    static class AocDay1
    {
        public static void ShowResult()
        {
            string input = File.ReadAllText("input.txt");
            string[] allNums = input.Split('\n');
            
            string firstNum = "";
            string secondNum = "";
            string thirdNum = "";

            // (1)
            // int sum;

            foreach (string item in allNums)
            {
                if (thirdNum.Length > 0)
                    break;

                foreach (string elem in allNums)
                {
                    if (thirdNum.Length > 0)
                        break;

                    if ((Convert.ToInt32(item) + Convert.ToInt32(elem)) < 2020)
                    {
                        firstNum = item;
                        secondNum = elem;
                        
                        // (1)
                        // improved: moved variable initializing to declaration place
                        int sum = Convert.ToInt32(item) + Convert.ToInt32(elem);

                        foreach (string el in allNums)
                        {
                            if (el != firstNum && el != secondNum && (Convert.ToInt32(el) + sum) == 2020)
                            {
                                thirdNum = el;
                                break;
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Day 01: "+ Convert.ToInt32(firstNum) * Convert.ToInt32(secondNum) * Convert.ToInt32(thirdNum));
        }
    }
}