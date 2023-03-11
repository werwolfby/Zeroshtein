namespace Zeroshtein;

public static class Levenshtein
{
    public static int Distance(string? a, string? b)
    {
        if (a is null || a.Length == 0)
        {
            return b?.Length ?? 0;
        }

        if (b is null || b.Length == 0)
        {
            return a.Length;
        }

        var n = a.Length;
        var m = b.Length;

        if (m > n)
        {
            (a, b) = (b, a);
            (n, m) = (m, n);
        }

        var d = new int[m + 1];

        for (var j = 0; j <= m; j++)
        {
            d[j] = j;
        }

        for (var i = 1; i <= n; i++)
        {
            d[0] = i;
            var prev = i - 1;
            for (var j = 1; j <= m; j++)
            {
                var equalCost = a[i - 1] == b[j - 1] ? 0 : 1;
                (prev, d[j]) = (d[j], Min(prev + equalCost, d[j - 1] + 1, d[j] + 1));
            }
        }

        return d[m];
    }

    private static int Min(int a, int b, int c) => Math.Min(Math.Min(a, b), c);
}
