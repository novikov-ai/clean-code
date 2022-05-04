using System;
using System.IO;
using System.Collections.Generic;

namespace CleanCode.VariableValues
{
    static class AocDay5
    {
        public static void ShowResult()
        {
            string input = File.ReadAllText("input.txt");
            string[] puzzle = input.Split('\n');

            int row = -1; // values of rows: 0-127
            int column = -1; // values of columns: 0-7
            int seadId = -1; // someone seatID

            int maxSeatId = -1;
            
            // (2)
            // int mySeatId = -1;

            List<int> allSeatIds = new List<int>();

            foreach (string item in puzzle)
            {
                int[] array = new int[128];
                for (int i = 0; i < array.Length; i++)
                    array[i] = i;

                int[] arrayNew = array;

                for (int i = 0; i < 10; i++) // looking for a row
                {
                    if (i == 7)
                    {
                        row = arrayNew[0];

                        array = new int[8];
                        for (int j = 0; j < array.Length; j++)
                            array[j] = j;

                        arrayNew = array;
                    }

                    int[] arrayLeft = new int[arrayNew.Length / 2];
                    int[] arrayRight = new int[arrayNew.Length / 2];

                    if (item[i] == 'F' || item[i] == 'L')
                    {
                        Array.ConstrainedCopy(arrayNew, 0, arrayLeft, 0, arrayLeft.Length);
                        arrayNew = arrayLeft;

                    }
                    else // == 'B' || 'R'
                    {
                        Array.ConstrainedCopy(arrayNew, arrayNew.Length / 2, arrayRight, 0, arrayRight.Length);
                        arrayNew = arrayRight;
                    }
                }

                column = arrayNew[0];
                seadId = row * 8 + column;
                allSeatIds.Add(seadId);

                if (seadId > maxSeatId)
                    maxSeatId = seadId;
            }

            allSeatIds.Sort();
            List<int> AllNums = new List<int>();
            for (int i = allSeatIds[0]; i <= maxSeatId; i++)
                AllNums.Add(i);

            // (2)
            // improved: moved variable initializing directly before cycle
            int mySeatId = -1;
            foreach (int item in AllNums)
            {
                if (!allSeatIds.Contains(item)) // my seat is missing in the list
                    mySeatId = item;
            }

            Console.WriteLine("Day 05: {0}", mySeatId);
        }
    }
}