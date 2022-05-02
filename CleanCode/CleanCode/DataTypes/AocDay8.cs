using System;
using System.IO;
using System.Collections.Generic;

namespace CleanCode.DataTypes
{
    static class Day08
    {
        private const string AccumulatorShortName = "acc";
        private const string JumpsShortName = "jmp";
        
        public static void ShowResult()
        {
            string input = File.ReadAllText("Input08.txt");
            string[] puzzle = input.Split('\n');

            int accumulatorValue = 0;
            
            List<int> doneSteps = new List<int>();

            for (int i = 0; i < puzzle.Length;)
            {
                if (!doneSteps.Contains(i))
                    doneSteps.Add(i);
                else
                    break;

                string[] lineOfPuzzle = puzzle[i].Split(" ");

                string numberCurrent;
                int signedNumbersCount;

                // (1)
                // prev: if (lineOfPuzzle[1].StartsWith('+'))
                // improved: created a boolean variable
                bool lineStartedWithPlus = lineOfPuzzle[1].StartsWith('+');
                if (lineStartedWithPlus)
                {
                    signedNumbersCount = 1;
                    numberCurrent = lineOfPuzzle[1].TrimStart('+');
                }
                else
                {
                    signedNumbersCount = -1;
                    numberCurrent = lineOfPuzzle[1].TrimStart('-');
                }

                Int32.TryParse(numberCurrent, out var numberBeforeAction);

                switch (lineOfPuzzle[0])
                {
                    // (2)
                    // prev: case "acc":
                    // improved: created a constant instead of string
                    case AccumulatorShortName:
                    {
                        accumulatorValue += numberBeforeAction * signedNumbersCount;
                        i++;
                        break;
                    }
                    
                    // (3)
                    // prev: case "jmp":
                    // improved: created a constant instead of string
                    case JumpsShortName:
                    {
                        i += numberBeforeAction * signedNumbersCount;
                        break;
                    }
                    
                    default:
                    {
                        i++;
                        break;
                    }
                }
            }

            Console.WriteLine("Day 08: " + accumulatorValue);
        }
    }
}