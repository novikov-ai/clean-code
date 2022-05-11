using System;
using System.Collections.Generic;

namespace CleanCode.Arrays.ConsoleApp
{
    public static class ArrayHelper
    {
        public static int GetSumOfPositiveNumbers(int[,] array)
        {
            if (array is null)
                throw new ArgumentNullException("Input array is null");

            int sum = 0;

            foreach (int num in array)
            {
                if (num > 0)
                    sum += num;
            }

            return sum;
        }
        private static int[] Sort(int[] array, bool asc)
        {
            if (array is null)
                throw new ArgumentNullException("Input array is null.");

            bool sorted = false;
            while (!sorted)
            {
                sorted = true;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if ((asc && array[i] > array[i + 1]) || (!asc && array[i] < array[i + 1]))
                    {
                        sorted = false;
                        var cache = array[i + 1];
                        array[i + 1] = array[i];
                        array[i] = cache;
                    }
                }
            }
            return array;
        }

        private static int[] Sort(IEnumerable<int> array, bool asc)
        {
            // ...
            // business logic removed
            // ...
            
            return null;
        }

        public static int[] SortAsc(int[] array)
        {
            return Sort(array, true);
        }
        public static int[] SortDesc(int[] array)
        {
            return Sort(array, false);
        } 
        public static int[] SortAsc(IEnumerable<int> array)
        {
            return Sort(array, true);
        }
        public static int[] SortDesc(IEnumerable<int> array)
        {
            return Sort(array, false);
        }
    }
}