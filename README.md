# Zeroshtein

Simple and fast implementation of the [Levenshtein distance](https://en.wikipedia.org/wiki/Levenshtein_distance) algorithm.
And nothing more.
It utilizes `Span<T>` over `stackallock` to avoid allocations and improve performance.
And `ArrayPool<T>` in case of exceed `MaxStackAllocSize` static property value.

It has a lot of optimizations and tricks to improve performance:
* **Do not use** `unsafe` code.
* Reducing array indexings.
* Allocating only array for current line calculation and reusing it for next line calculation.
* Zero index is not part of the line and stored in a separate variable.
* etc.

## Benchmarks

And in result it extremely fast on Windows and MacOS.
It even faster than [Fastenshtein](https://github.com/DanHarltey/Fastenshtein) library (a little bit :smirk:).

### Windows 11, x86

|                Method |      Mean |     Error |    StdDev | Ratio |    Gen0 | Allocated |
|---------------------- |----------:|----------:|----------:|------:|--------:|----------:|
|          Fastenshtein |  7.613 us | 0.0295 us | 0.0276 us |  1.00 |  0.9308 |    5896 B |
| FastenshteinZeroAlloc |  6.845 us | 0.0136 us | 0.0120 us |  0.90 |       - |         - |
|            Zeroshtein |  6.958 us | 0.0093 us | 0.0083 us |  0.91 |       - |         - |
|    ZeroshteinInMemory | 12.122 us | 0.0263 us | 0.0246 us |  1.59 |       - |         - |
|      StringSimilarity | 17.948 us | 0.0691 us | 0.0647 us |  2.36 |  1.7395 |   11064 B |
|              NinjaNye | 19.700 us | 0.0586 us | 0.0490 us |  2.59 |  7.1411 |   44880 B |
|          FuzzyStrings | 77.905 us | 0.2432 us | 0.2031 us | 10.23 | 26.8555 |  169040 B |
|         Quickenshtein | 23.650 us | 0.0324 us | 0.0271 us |  3.11 |       - |         - |

Benchmark was performed in next environment:

```text
BenchmarkDotNet=v0.13.5, OS=Windows 11 (10.0.22000.1574/21H2/SunValley)
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.102
  [Host]     : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
  DefaultJob : .NET 7.0.2 (7.0.222.60605), X64 RyuJIT AVX2
```

### MacOS 13.2, arm64

|                Method |      Mean |     Error |    StdDev | Ratio |    Gen0 | Allocated |
|---------------------- |----------:|----------:|----------:|------:|--------:|----------:|
|          Fastenshtein |  5.690 us | 0.0240 us | 0.0225 us |  1.00 |  0.9384 |    5896 B |
| FastenshteinZeroAlloc |  5.237 us | 0.0142 us | 0.0132 us |  0.92 |       - |         - |
|            Zeroshtein |  5.469 us | 0.0105 us | 0.0093 us |  0.96 |       - |         - |
|    ZeroshteinInMemory |  6.906 us | 0.0188 us | 0.0176 us |  1.21 |       - |         - |
|      StringSimilarity |  8.884 us | 0.0173 us | 0.0162 us |  1.56 |  1.7548 |   11064 B |
|              NinjaNye | 11.147 us | 0.0263 us | 0.0219 us |  1.96 |  7.1411 |   44880 B |
|          FuzzyStrings | 46.450 us | 0.1166 us | 0.0974 us |  8.17 | 26.9165 |  169040 B |
|         Quickenshtein |  5.692 us | 0.0115 us | 0.0108 us |  1.00 |       - |         - |

Benchmark was performed in next environment:

```text
BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.2.1 (22D68) [Darwin 22.3.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 7.0.1 (7.0.122.56804), Arm64 RyuJIT AdvSIMD
```

## Usage

```csharp
var distance = Zeroshtein.Levenshtein.Distance("horce", "rose");
```

By default `Zeroshtein` uses `stackallock` to allocate calculation array in stack only.
And if it exceeds `MaxStackAllocSize` static property value it uses `ArrayPool<T>` to allocate array in memory.
Default value of `MaxStackAllocSize` is `1024` bytes.
But can be changed to any value, it is a static property.

```csharp
Zeroshtein.Levenshtein.MaxStackAllocSize = 4096;
```

It should be set with caution.
Big value can cause `StackOverflowException` in case of long strings.
This exception can't be caught and handled.
It will cause application crash.
