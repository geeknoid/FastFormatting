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
|     ClassicStringFormat | 44.540 ms | 1.9369 ms | 2.8990 ms | 3062.5000 | 1250.0000 | 375.0000 | 16799923 B |
|           Interpolation | 45.067 ms | 1.6723 ms | 2.3983 ms | 3000.0000 | 1187.5000 | 312.5000 | 16799928 B |
|           StringBuilder | 53.153 ms | 2.2890 ms | 3.4260 ms | 2888.8889 | 1111.1111 | 222.2222 | 16799926 B |
|         CompositeFormat | 21.679 ms | 0.1913 ms | 0.2682 ms | 2062.5000 |  781.2500 | 156.2500 | 11999930 B |
| CompositeFormatWithSpan |  8.499 ms | 0.0866 ms | 0.1242 ms |         - |         - |        - |          - |
|             StringMaker | 10.090 ms | 0.1418 ms | 0.2078 ms | 5546.8750 |         - |        - | 23199600 B |
|     StringMakerWithSpan |  8.371 ms | 0.0569 ms | 0.0779 ms | 2671.8750 |         - |        - | 11199680 B |

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
