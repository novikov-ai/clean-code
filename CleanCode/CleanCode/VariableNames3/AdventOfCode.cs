using System;
using System.IO;
using System.Collections.Generic;

namespace CleanCode.VariableNames3
{
    static class AdventOfCode
    {
        public static void ShowResult()
        {
            string inputText = File.ReadAllText("Input09.txt");
            string[] puzzle = inputText.Split('\n');

            int preamble = 25;
            
            List<int> allNums = new List<int>();

            int invalidNum = -1;
            
            // 7.2 (1) ok - processingCompleted
            bool processingCompleted = false;

            // 7.3 (1) i - pieceOfPuzzle
            for (int pieceOfPuzzle = 0; pieceOfPuzzle < puzzle.Length; pieceOfPuzzle++)
            {
                if (invalidNum != -1)
                    break;

                int num;
                Int32.TryParse(puzzle[pieceOfPuzzle], out num);
                allNums.Add(num);

                if (pieceOfPuzzle > preamble - 1)
                {
                    // 7.4 (1) (firstPiece, lastPiece)
                    var firstPiece = pieceOfPuzzle - preamble;
                    var lastPiece = pieceOfPuzzle - 1;

                    for (int i = firstPiece; i <= lastPiece; i++)
                    {
                        for (int j = firstPiece; j <= lastPiece; j++)
                        {
                            if (i == j)
                                continue;

                            if (allNums[i] + allNums[j] == allNums[pieceOfPuzzle])
                                processingCompleted = true;
                        }
                    }

                    if (!processingCompleted)
                    {
                        invalidNum = allNums[pieceOfPuzzle];
                        break;
                    }
                    processingCompleted = false;
                }
            }

            List<int> contiguousSet = new List<int>();

            // 7.5 (6) sum - sumOfPuzzleNumbers
            int sumOfPuzzleNumbers = 0;
            int setLength = 0;

            // 7.4 (2) (minNumber, maxNumber)
            int minNumber = -1;
            int maxNumber = -1;

            int startFrom = 0;

            for (int pieceOfPuzzle = 0; pieceOfPuzzle < puzzle.Length;)
            {
                // 7.5 (7) num - number
                Int32.TryParse(puzzle[pieceOfPuzzle], out var number);
                contiguousSet.Add(number);
                sumOfPuzzleNumbers += number;

                if (sumOfPuzzleNumbers > invalidNum)
                {
                    contiguousSet.Clear();
                    sumOfPuzzleNumbers = 0;

                    startFrom++;
                    pieceOfPuzzle = startFrom;
                }
                else
                {
                    if (sumOfPuzzleNumbers == invalidNum && contiguousSet.Count > setLength)
                    {
                        contiguousSet.Sort();

                        minNumber = contiguousSet[0];
                        maxNumber = contiguousSet[contiguousSet.Count - 1];

                        setLength = contiguousSet.Count;
                        sumOfPuzzleNumbers = 0;

                        contiguousSet.Clear();

                        startFrom++;
                        pieceOfPuzzle = startFrom;
                    }
                    else
                        pieceOfPuzzle++;
                }
            }

            // 7.5 [variable result is useless]
            // int result = min + max;
            // Console.WriteLine("Day 09: " + result);
            
            Console.WriteLine("Day 09: " + minNumber + maxNumber);
        }
    }
}