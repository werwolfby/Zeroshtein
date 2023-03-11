namespace Zeroshtein.Tests;

public class LevenshteinTests
{
    [TestCase("a", "a", 0)]
    [TestCase("a", "b", 1)]
    [TestCase("a", "ab", 1)]
    [TestCase("a", "ba", 1)]
    [TestCase("a", "abc", 2)]
    [TestCase("a", "cba", 2)]
    [TestCase("a", "abcd", 3)]
    [TestCase("a", "dcba", 3)]
    [TestCase("a", "abcde", 4)]
    [TestCase("a", "edcba", 4)]
    [TestCase("a", "abcdef", 5)]
    [TestCase("a", "fedcba", 5)]
    [TestCase("a", "abcdefg", 6)]
    [TestCase("a", "gfedcba", 6)]
    [TestCase("a", "abcdefgh", 7)]
    [TestCase("a", "hgfedcba", 7)]
    [TestCase("a", "abcdefghi", 8)]
    [TestCase("a", "ihgfedcba", 8)]
    [TestCase("a", "abcdefghij", 9)]
    [TestCase("a", "jihgfedcba", 9)]
    [TestCase("a", "abcdefghijk", 10)]
    [TestCase("a", "kjihgfedcba", 10)]
    [TestCase("a", "abcdefghijkl", 11)]
    [TestCase("a", "lkjihgfedcba", 11)]
    [TestCase("a", "abcdefghijklm", 12)]
    [TestCase("a", "mlkjihgfedcba", 12)]
    [TestCase("abc", "abc", 0)]
    [TestCase("abc", "acb", 2)]
    [TestCase("test", "tett", 1)]
    [TestCase("test", "tet", 1)]
    [TestCase("test", "testt", 1)]
    [TestCase("rose", "horse", 2)]
    public void DistanceTest(string a, string b, int expected)
    {
        Assert.That(Levenshtein.Distance(a, b), Is.EqualTo(expected));
    }
}
