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
|                      Method |     Mean |    Error |   StdDev |   Median |     Gen 0 |     Gen 1 |    Gen 2 |  Allocated |
|---------------------------- |---------:|---------:|---------:|---------:|----------:|----------:|---------:|-----------:|
|     TestClassicStringFormat | 46.68 ms | 0.922 ms | 1.882 ms | 46.27 ms | 2909.0909 | 1181.8182 | 363.6364 | 16800055 B |
|           TestInterpolation | 45.17 ms | 0.888 ms | 1.578 ms | 44.94 ms | 2909.0909 | 1181.8182 | 363.6364 | 16800053 B |
|         TestStringFormatter | 25.31 ms | 0.495 ms | 0.463 ms | 25.07 ms | 2187.5000 |  906.2500 | 281.2500 | 12000024 B |
| TestStringFormatterWithSpan | 12.01 ms | 0.366 ms | 1.079 ms | 11.52 ms |         - |         - |        - |        4 B |
```

# Example

```csharp
var sm = new StringMaker();
sm.Append("Hello ");
sm.Append("World ");
sm.Append(42, "", null, 5);
var str1 = sm.ExtractString();
Console.WriteLine(str1);     // prints Hello World      42

sm = new StringMaker();
sm.Append("Hello ");
sm.Append("World ");
sm.Append(42);
var span = sm.ExtractSpan();
var str2 = span.ToString();
Console.WriteLine(str2);     // prints Hello World 42

var sf = new StringFormatter("Hello {0}");
var str3 = sf.Format(null, "World");
Console.WriteLine(str3);     // prints Hello World
```
