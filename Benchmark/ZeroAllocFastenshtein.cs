using System.Buffers;

namespace Benchmark;

public static class ZeroAllocFastenshtein
{
    public static int MaxStackAllocSize { get; set; } = 1024;

    /// <summary>
    /// Code copied from https://github.com/DanHarltey/Fastenshtein/blob/master/src/Fastenshtein/StaticLevenshtein.cs
    /// to compare performance with Zerostein and check if it worth to submit a PR to Fastenshtein
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static int Distance(string value1, string value2)
    {
        if (value2.Length == 0)
        {
            return value1.Length;
        }

        int[]? rentArray = null;

        var heapSpan = value2.Length > MaxStackAllocSize;
        if (heapSpan)
        {
            rentArray = ArrayPool<int>.Shared.Rent(value2.Length);
        }
        Span<int> costs = heapSpan
            ? rentArray.AsSpan(0, value2.Length)
            : stackalloc int[value2.Length];;

        // Add indexing for insertion to first row
        for (int i = 0; i < costs.Length;)
        {
            costs[i] = ++i;
        }

        for (int i = 0; i < value1.Length; i++)
        {
            // cost of the first index
            int cost = i;
            int previousCost = i;

            // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
            char value1Char = value1[i];

            for (int j = 0; j < value2.Length; j++)
            {
                int currentCost = cost;

                // assigning this here reduces the array reads we do, improvement of the old version
                cost = costs[j];

                if (value1Char != value2[j])
                {
                    if (previousCost < currentCost)
                    {
                        currentCost = previousCost;
                    }

                    if (cost < currentCost)
                    {
                        currentCost = cost;
                    }

                    ++currentCost;
                }

                /*
                 * Improvement on the older versions.
                 * Swapping the variables here results in a performance improvement for modern intel CPU’s, but I have no idea why?
                 */
                costs[j] = currentCost;
                previousCost = currentCost;
            }
        }

        if (heapSpan)
        {
            ArrayPool<int>.Shared.Return(rentArray!);
        }

        return costs[costs.Length - 1];
    }
}
