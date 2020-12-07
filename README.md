# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are two new types to support faster formatting:

* The `StringMaker` type is a supercharged version of the classic `StringBuilder` type. It
is designed for efficiently building up a string or span by appending together bits and
pieces. The implementation tries to avoid any memory allocations when it can, and if you
supply your own span to the constructor, it can operate entirely without allocating memory.
When used in this memory, totally performance is around 7x faster than using `StringBuilder`.

* The `StringFormatter` type is built on top of `StringMaker` and is designed to support the
normal .NET composite formatting model, such as you use with `String.Format`. The type splits
the normal formatting step in two in order to boost performance of the formatting process. 
The first step is parsing the composite format string into an efficient form. This step is done
by creating a `StringFormatter` instance. Once you have this instance, you can use it repeatedly
to format arguments by calling the `Format` method, which is about 2x as fast as `String.Format`.
You can also use the `TryFormat` method to format directly into your own span, which is 4x faster
than `String.Format`.

Here's output from the benchmark:

```
|                      Method |      Mean |     Error |    StdDev |     Gen 0 |     Gen 1 |    Gen 2 |  Allocated |
|---------------------------- |----------:|----------:|----------:|----------:|----------:|---------:|-----------:|
|     TestClassicStringFormat | 46.979 ms | 0.9339 ms | 1.3393 ms | 2909.0909 | 1181.8182 | 363.6364 | 16800153 B |
|           TestInterpolation | 45.558 ms | 0.8942 ms | 1.1309 ms | 3000.0000 | 1250.0000 | 416.6667 | 16800541 B |
|         TestStringFormatter | 24.727 ms | 0.4271 ms | 0.5554 ms | 2187.5000 |  906.2500 | 281.2500 | 12000022 B |
| TestStringFormatterWithSpan | 10.121 ms | 0.2002 ms | 0.4757 ms |         - |         - |        - |        4 B |
|             TestStringMaker |  7.744 ms | 0.1469 ms | 0.3066 ms | 2867.1875 |         - |        - | 11999922 B |
|     TestStringMakerWithSpan |  5.224 ms | 0.0606 ms | 0.0538 ms |         - |         - |        - |        2 B |
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
