# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are two new types to support faster formatting:

* The `StringMaker` type is a supercharged version of the classic `StringBuilder` type. It
is designed for efficiently building up a string or span by appending together bits and
pieces. The implementation tries to avoid any memory allocations when it can, and if you
supply your own span to the constructor, it can operate entirely without allocating memory.
When used in this mode, total performance is around 9x faster than `StringBuilder`.

* The `StringFormatter` type is built on top of `StringMaker` and is designed to support the
normal .NET composite formatting model, such as you use with `String.Format`. The type splits
the normal formatting step in two in order to boost performance of the formatting process. 
The first step is parsing the composite format string into an efficient form. This step is done
by creating a `StringFormatter` instance. Once you have this instance, you can use it repeatedly
to format arguments by calling the `Format` method, which is about 2x as fast as `String.Format`.
You can also use the `TryFormat` method to format directly into your own span, which is 4x faster
than `String.Format`.

* 'StringFormatter` also includes some static methods that provide a 1-1 replacement for String.Format,
but run 2x as fast. Just go through your code and replace all uses of `String.Format` with
`StringFormatter.Format` and you're done: your code runs faster.

Here's output from the benchmark:

```
|                      Method |      Mean |     Error |    StdDev |     Gen 0 |     Gen 1 |    Gen 2 |  Allocated |
|---------------------------- |----------:|----------:|----------:|----------:|----------:|---------:|-----------:|
|     TestClassicStringFormat | 44.866 ms | 0.8916 ms | 1.4897 ms | 3000.0000 | 1250.0000 | 416.6667 | 16800539 B |
|           TestInterpolation | 45.272 ms | 0.8490 ms | 1.5524 ms | 3000.0000 | 1250.0000 | 416.6667 | 16800515 B |
|           TestStringBuilder | 49.877 ms | 0.9750 ms | 1.3016 ms | 2888.8889 | 1111.1111 | 333.3333 | 16800055 B |
|         TestStringFormatter | 25.101 ms | 0.5015 ms | 0.9170 ms | 2187.5000 |  906.2500 | 281.2500 | 12000022 B |
| TestStringFormatterWithSpan |  9.574 ms | 0.1785 ms | 0.1490 ms |         - |         - |        - |        4 B |
|             TestStringMaker |  7.308 ms | 0.1203 ms | 0.1564 ms | 2859.3750 |         - |        - | 11999924 B |
|     TestStringMakerWithSpan |  5.043 ms | 0.0988 ms | 0.1832 ms |         - |         - |        - |        2 B |
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

var str4 = StringFormatter.Format("Hello {0}", name);
Console.WriteLine(str4);     // prints Hello World
```
