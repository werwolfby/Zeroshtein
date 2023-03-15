using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmark;

[MemoryDiagnoser]
public class BenchmarkLevenstein
{
    private static readonly string[] words = {
        "horse",
        "rose",
        "test",
        "tett",
        "tet",
        "testt",
        "acb",
        "abc",
        "abc",
        "mlkjihgfedcba",
        "abcdefghijklm",
    };

    [Benchmark(Baseline = true)]
    public void Fastenshtein()
    {
        for (var i = 0; i < words.Length; i++)
        {
            for (var j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Levenshtein.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void FastenshteinZeroAlloc()
    {
        for (var i = 0; i < words.Length; i++)
        {
            for (var j = 0; j < words.Length; j++)
            {
                ZeroAllocFastenshtein.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void Zeroshtein()
    {
        for (var i = 0; i < words.Length; i++)
        {
            for (var j = 0; j < words.Length; j++)
            {
                global::Zeroshtein.Levenshtein.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void ZeroshteinInMemory()
    {
        global::Zeroshtein.Levenshtein.MaxStackAllocSize = 0;
        for (var i = 0; i < words.Length; i++)
        {
            for (var j = 0; j < words.Length; j++)
            {
                global::Zeroshtein.Levenshtein.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void StringSimilarity()
    {
        // I've read the source code it is thread safe
        var lev = new global::F23.StringSimilarity.Levenshtein();

        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                // why does it return a double ??
                lev.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void NinjaNye()
    {
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::NinjaNye.SearchExtensions.Levenshtein.LevenshteinProcessor.LevenshteinDistance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void FuzzyStrings()
    {
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                DuoVia.FuzzyStrings.LevenshteinDistanceExtensions.LevenshteinDistance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void Quickenshtein()
    {
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Quickenshtein.Levenshtein.GetDistance(words[i], words[j]);
            }
        }
    }

    public static void Main()
    {
        var summary = BenchmarkRunner.Run<BenchmarkLevenstein>();
        Console.WriteLine(summary);
    }
}
