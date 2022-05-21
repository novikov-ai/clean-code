using System;

namespace CleanCode.VariableNames3
{
    public class Matrix
    {
        private int[,] _matrix;

        public Matrix(int[,] matrix)
        {
            if (matrix is null)
                throw new ArgumentNullException("Matrix is null");

            _matrix = matrix;
        }

        public delegate void SortingStrategy(ref int rowValue, int i, int j, int[,] matrix);
        public delegate bool OrderStrategy(int rowPrevValue, int rowValue);

        public void Sort(SortingStrategy sortingStrategy, OrderStrategy orderStrategy)
        {
            int rowPrevValue = 0;
            int rowPrevNum = 0;

            // 7.1 (1) sorted - isMatrixSorted
            bool isMatrixSorted = false;
            while (!isMatrixSorted)
            {
                isMatrixSorted = true;
                for (int i = 0; i <= _matrix.GetUpperBound(0); i++)
                {
                    int rowValue = 0;

                    for (int j = 0; j <= _matrix.GetUpperBound(1); j++)
                    {
                        sortingStrategy(ref rowValue, i, j, _matrix);
                    }

                    if (i != 0 && orderStrategy(rowPrevValue, rowValue))
                    {
                        SwapRows(ref _matrix, i, rowPrevNum);
                        isMatrixSorted = false;
                    }

                    rowPrevValue = rowValue;
                    rowPrevNum = i;
                }
            }
        }

        private void SwapRows(ref int[,] matrix, int row1, int row2)
        {
            for (int i = 0; i <= matrix.GetUpperBound(1); i++)
            {
                int cache = matrix[row1, i];
                matrix[row1, i] = matrix[row2, i];
                matrix[row2, i] = cache;
            }
        }
    }
}