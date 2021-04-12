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
|     ClassicStringFormat | 46.075 ms | 2.1046 ms | 3.0849 ms | 3000.0000 | 1272.7273 | 363.6364 | 16799928 B |
|           Interpolation | 46.858 ms | 1.7064 ms | 2.3921 ms | 3000.0000 | 1187.5000 | 312.5000 | 16799924 B |
|           StringBuilder | 52.890 ms | 1.4441 ms | 2.1167 ms | 2900.0000 | 1100.0000 | 200.0000 | 16799926 B |
|         CompositeFormat | 22.959 ms | 0.4265 ms | 0.6384 ms | 2062.5000 |  781.2500 | 156.2500 | 11999928 B |
| CompositeFormatWithSpan |  8.770 ms | 0.0831 ms | 0.1218 ms |         - |         - |        - |          - |
|             StringMaker | 10.497 ms | 0.4242 ms | 0.5947 ms | 5546.8750 |         - |        - | 23199600 B |
|     StringMakerWithSpan |  9.232 ms | 0.1310 ms | 0.1837 ms | 2671.8750 |         - |        - | 11199680 B |

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
