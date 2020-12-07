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
|     TestClassicStringFormat | 45.306 ms | 0.8882 ms | 1.6016 ms | 45.605 ms | 2888.8889 | 1111.1111 | 333.3333 | 16800098 B |
|           TestInterpolation | 46.278 ms | 0.9231 ms | 1.8857 ms | 45.727 ms | 3000.0000 | 1250.0000 | 416.6667 | 16800517 B |
|         TestStringFormatter | 24.990 ms | 0.4604 ms | 0.6302 ms | 25.011 ms | 2187.5000 |  906.2500 | 281.2500 | 12000040 B |
| TestStringFormatterWithSpan | 11.166 ms | 0.2147 ms | 0.5225 ms | 10.991 ms |         - |         - |        - |        4 B |
|             TestStringMaker |  8.932 ms | 0.1380 ms | 0.1152 ms |  8.922 ms | 2859.3750 |         - |        - | 11999920 B |
|     TestStringMakerWithSpan |  6.416 ms | 0.1033 ms | 0.0967 ms |  6.418 ms |         - |         - |        - |        2 B |
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
