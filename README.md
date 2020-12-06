# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are two new types to support faster formatting:

* The `StringMaker` type is a supercharged version of the classic `StringBuilder` type. It
is designed for efficiently building up a string or span by appending together bits and
pieces. The implementation tries to avoid any memory allocations when it can, and if you
supply your own span to the constructor, it can operate entirely without allocating memory.
When used in this memory, totally performance is around 4x faster than using `StringBuilder`.

* The `StringFormatter` type is built on top of `StringMaker` and is designed to support the
normal .NET composite formatting model, such as you use with `String.Format`. The type splits
the normal formatting step in two in order to boost performance of the formatting process. 
The first step is parsing the composite format string into an efficient form. This step is done
by create a `StringFormatter` instance. Once you have this instance, you can use it to format 
a set of arguments by calling the `Format` method, which is about 2x as fast as `String.Format`.
You can also use the `TryFormat` method to format directly into your own span, which is 4x faster
than `String.Format`.

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
var str1 = new StringMaker()
    .Append("Hello ")
    .Append("World ")
    .Append(42, "", null, 5)
    .ExtractString();
Console.WriteLine(str1);     // prints Hello World      42

var span = new StringMaker()
    .Append("Hello ")
    .Append("World ")
    .Append(42)
    .ExtractSpan();
Console.WriteLine(span.ToString());     // prints Hello World 42

var sf = new StringFormatter("Hello {0}");
var str3 = sf.Format(null, "World");
Console.WriteLine(str3);     // prints Hello World
```
