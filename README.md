# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are two innovations in this code relative to the classic String.Format method:

* The API is divided into parts. One part parses the composite format string, while
the other part is responsible for the actual formatting work. You typically parse
your format strings as part of initializing static StringFormatter instances,
and then during execution your code can use the static StringFormatter instances
to actually format arguments.

* The StringFormatter.Format API avoids boxing for up to 3 formatting arguments.
In that case, the API does only a single allocation for the final string and typically
has no other intermediate allocations.

This code is designed to eventually be put into the .NET libraries. As such,
I liberally borrowed private code from the .NET libraries, you'll find those 
files in the FastFormatting/Borrowed folder.

Here's output from the benchmark:

```
|              Method |     Mean |    Error |   StdDev |     Gen 0 |     Gen 1 |    Gen 2 | Allocated |
|-------------------- |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
|    TestStringFormat | 52.13 ms | 1.016 ms | 1.611 ms | 3100.0000 | 1300.0000 | 400.0000 |  16.02 MB |
| TestStringFormatter | 25.95 ms | 0.470 ms | 0.894 ms | 2187.5000 |  906.2500 | 281.2500 |  11.44 MB |
|   TestInterpolation | 51.68 ms | 0.921 ms | 1.350 ms | 3100.0000 | 1300.0000 | 400.0000 |  16.02 MB |
```

# Example

```csharp
    var sf = new StringFormatter("Hello {0}");
    var str = sf.Format("World");

    Console.WriteLine(str);     // prints Hello World
```
# Implementation Todos

* Support formatting to a span, for completely no-alloc usage
* Make it so a FormatterSegment carries both a literal preamble and an argument format, to reduce the # of segments we keep
* Add API docs
