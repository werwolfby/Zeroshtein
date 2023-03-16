using System.Buffers;

namespace Zeroshtein;

public static class Levenshtein
{
    public static int MaxStackAllocSize { get; set; } = 1024;

    public static int Distance(string a, string b)
    {
        if (a.Length == 0)
        {
            return b.Length;
        }

        if (b.Length == 0)
        {
            return a.Length;
        }

        if (b.Length > a.Length)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        int[]? rentArray = null;

        var heapSpan = b.Length > MaxStackAllocSize;
        if (heapSpan)
        {
            rentArray = ArrayPool<int>.Shared.Rent(b.Length);
        }

        var d = heapSpan
            ? rentArray.AsSpan(0, b.Length)
            : stackalloc int[b.Length];

        for (var j = 0; j < d.Length;)
        {
            d[j] = ++j;
        }

        for (var i = 0; i < a.Length; i++)
        {
            var cost = i;
            var left = i + 1;
            for (var j = 0; j < b.Length; j++)
            {
                var up = d[j];

                if (a[i] != b[j])
                {
                    if (up < cost)
                    {
                        cost = up;
                    }
                    if (left < cost)
                    {
                        cost = left;
                    }

                    cost++;
                }

                left = d[j] = cost;
                cost = up;
            }
        }

        if (heapSpan)
        {
            ArrayPool<int>.Shared.Return(rentArray!);
        }

        return d[d.Length - 1];
    }
}
