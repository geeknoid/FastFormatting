# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are three innovations in this code relative to the classic String.Format method:

* The API is divided into parts. One part parses the composite format string, while
the other part is responsible for the actual formatting work. You typically parse
your format strings as part of initializing static StringFormatter instances,
and then during execution your code can use the static StringFormatter instances
to actually format arguments.

* The StringFormatter.Format API avoids boxing for up to 3 formatting arguments.
In that case, the API does only a single allocation for the final string and typically
has no other intermediate allocations.

* The StringFormatter.TryFormat API formats into a preallocated Span<char>, enabling completely
0 allocation and 0 copy string formatting. It took 20 years, but formatting in C# is now as fast
as C's sprintf function!'

This code is designed to eventually be put into the .NET libraries. As such,
I borrowed a bit of private code from the .NET libraries, you'll find those 
files in the FastFormatting/Borrowed folder.

Here's output from the benchmark:

```
|                      Method |     Mean |    Error |   StdDev |     Gen 0 |     Gen 1 |    Gen 2 |  Allocated |
|---------------------------- |---------:|---------:|---------:|----------:|----------:|---------:|-----------:|
|     TestClassicStringFormat | 46.02 ms | 0.865 ms | 1.515 ms | 2909.0909 | 1181.8182 | 363.6364 | 16800057 B |
|           TestInterpolation | 47.91 ms | 0.943 ms | 1.523 ms | 3000.0000 | 1250.0000 | 416.6667 | 16800523 B |
|         TestStringFormatter | 25.59 ms | 0.508 ms | 0.903 ms | 2187.5000 |  906.2500 | 281.2500 | 12000025 B |
| TestStringFormatterWithSpan | 10.86 ms | 0.182 ms | 0.224 ms |         - |         - |        - |        4 B |
```

# Example

```csharp
    var sf = new StringFormatter("Hello {0}");
    var str = sf.Format("World");

    Console.WriteLine(str);     // prints Hello World
```
# Implementation Todos

* Add more unit tests
