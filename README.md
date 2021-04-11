# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are two new types to support faster formatting:

* The `CompositeFormat` type is built is designed to support the
normal .NET composite formatting model, such as you use with `String.Format`. The type splits
the normal formatting step in two in order to boost performance of the formatting process. 
The first step is parsing the composite format string into an efficient form. This step is done
by creating a `CompositeFormat` instance. Once you have this instance, you can use it repeatedly
to format arguments by calling the `Format` method, which is about 2x as fast as `String.Format`.
You can also use the `TryFormat` method to format directly into your own span, which is 4x faster
than `String.Format`.

* The `StringMaker` type is a supercharged version of the classic `StringBuilder` type. It
is designed for efficiently building up a string or span by appending together bits and
pieces. The implementation tries to avoid any memory allocations when it can, and if you
supply your own span to the constructor, it can operate entirely without allocating memory.
When used in this mode, total performance is around 9x faster than `StringBuilder`. This type is 
nominally internal, but can also be exposed publically by defining the PUBLIC_STRINGMAKER
compilation symbol.

## Template-Based Format Strings

The `LoggerMessage.Define` method in .NET supports template-based format strings,
which use template names instead of argument indices in the format string. The
`CompositeFormat` provides an overload which supports this use case. This overload
returns the set of encountered template names.

## Benchmark

Here's output from the benchmark:

```
|                  Method |      Mean |     Error |    StdDev |     Gen 0 |     Gen 1 |    Gen 2 |  Allocated |
|------------------------ |----------:|----------:|----------:|----------:|----------:|---------:|-----------:|
|     ClassicStringFormat | 45.036 ms | 26.859 ms | 1.4723 ms | 3083.3333 | 1333.3333 | 416.6667 | 16800613 B |
|           Interpolation | 43.340 ms |  3.324 ms | 0.1822 ms | 3000.0000 | 1187.5000 | 312.5000 | 16799929 B |
|           StringBuilder | 51.144 ms |  3.827 ms | 0.2098 ms | 2900.0000 | 1100.0000 | 200.0000 | 16799922 B |
|         CompositeFormat | 26.178 ms | 16.042 ms | 0.8793 ms | 2062.5000 |  781.2500 | 156.2500 | 11999929 B |
| CompositeFormatWithSpan | 10.382 ms |  1.172 ms | 0.0642 ms |         - |         - |        - |        2 B |
|             StringMaker |  9.573 ms |  1.488 ms | 0.0816 ms | 5546.8750 |         - |        - | 23199601 B |
|     StringMakerWithSpan |  8.550 ms |  6.963 ms | 0.3817 ms | 2671.8750 |         - |        - | 11199680 B |

```

## Example

Using `CompositeFormat`

```csharp
private static readonly _cf = new CompositeFormat("Hello {0}");

public void Foo()
{
    var str3 = _cf.Format("World");
    Console.WriteLine(str3);     // prints Hello World
}
```

Using `StringMaker` directly:

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
```

## Implementation Notes

This has 100% unit test coverage. A special emphasis was made on
maintaining 100% compatibility with classic .NET composite formatting
semantics.
