# Fast Formatting

This project is a proof-of-concept showing how to substantially improve string formatting performance in .NET.
There are two innovations in this code relative to the classic String.Format method:

* The API is divided into parts. One part parses the composite format string, while
the other part is responsible for the actual formatting work. You typically parse
your format strings as part of initializing static StringFormatter instances,
and then during execution your code can use the static StringFormatter instances
to actually format arguments.

* The StringFormatter.Format API avoids boxing for up to 3 formatting arguments.
In the case, the API does only a single allocation for the final string and typically
has no other intermediate allocations.

This code is designed to eventually be put into the .NET libraries. As such,
I liberally borrowed private code from the .NET libraries, you'll find those 
files in the FastFormatting/Borrowed folder.

# Example

```csharp
    var sf = new StringFormatter("Hello {0}");
    var str = sf.Format("World");

    Console.WriteLine(str);     // prints Hello World
```

# Note

It's likely that the trick used to avoid boxing of arguments can be used for the
regular String.Format method as well.
