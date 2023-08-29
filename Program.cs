using System;
using System.Diagnostics;
using System.Threading.Tasks;

class MatrixMultiplication
{
    static int[,] MultiplyMatricesSerial(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += matrixA[i, k] * matrixB[k, j];
                }
                result[i, j] = sum;
            }
        }

        return result;
    }

    static int[,] MultiplyMatricesParallel(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int colsB = matrixB.GetLength(1);

        int[,] result = new int[rowsA, colsB];

        Parallel.For(0, rowsA, i =>
        {
            for (int j = 0; j < colsB; j++)
            {
                int sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += matrixA[i, k] * matrixB[k, j];
                }
                result[i, j] = sum;
            }
        });

        return result;
    }

    static void Main()
    {
        int rowsA = 1000;
        int colsA = 1000;
        int colsB = 1000;

        int[,] matrixA = GenerateRandomMatrix(rowsA, colsA);
        int[,] matrixB = GenerateRandomMatrix(colsA, colsB);

        Stopwatch stopwatch = new Stopwatch();

        // Serial version
        stopwatch.Start();
        int[,] serialResult = MultiplyMatricesSerial(matrixA, matrixB);
        stopwatch.Stop();
        TimeSpan serialTime = stopwatch.Elapsed;
        Console.WriteLine("Serial version time: " + serialTime);

        // Parallel version
        stopwatch.Reset();
        stopwatch.Start();
        int[,] parallelResult = MultiplyMatricesParallel(matrixA, matrixB);
        stopwatch.Stop();
        TimeSpan parallelTime = stopwatch.Elapsed;
        Console.WriteLine("Parallel version time: " + parallelTime);

        // Compare results
        bool resultsMatch = CompareMatrices(serialResult, parallelResult);
        Console.WriteLine("Results match: " + resultsMatch);

        // Calculate speedup and efficiency
        double speedup = serialTime.TotalMilliseconds / parallelTime.TotalMilliseconds;
        double efficiency = speedup / Environment.ProcessorCount;
        Console.WriteLine("Speedup: " + speedup);
        Console.WriteLine("Efficiency: " + efficiency);
    }

    static int[,] GenerateRandomMatrix(int rows, int cols)
    {
        Random random = new Random();
        int[,] matrix = new int[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = random.Next(1, 10);
            }
        }
        return matrix;
    }

    static bool CompareMatrices(int[,] matrixA, int[,] matrixB)
    {
        int rowsA = matrixA.GetLength(0);
        int colsA = matrixA.GetLength(1);
        int rowsB = matrixB.GetLength(0);
        int colsB = matrixB.GetLength(1);

        if (rowsA != rowsB || colsA != colsB)
        {
            return false;
        }

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsA; j++)
            {
                if (matrixA[i, j] != matrixB[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }
}



