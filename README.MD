# SoftwareMadeSimple.SimpleResults.Result\<T, E\>

A simple, immutable result type for representing success or failure in C#.

## Features

- Encapsulates either a success value (`T`) or an error (`E`).
- Provides static factory methods for success and failure.
- Implicit conversion from value or error.
- Nullable properties for value and error.
- `IsSuccess` and `IsFailure` properties for easy result checking.
- Uses `[MemberNotNullWhen]` attributes for improved nullability analysis.

## Usage

The SimpleResults library uses implicit conversion to allow you to easily create results from values or errors.

```csharp
private static SimpleResults.Result<int, string> NotZero(int input)
{
    if (input == 0)
        return "Input may not be zero";

    return input;
}
```

Use the Value property when the result IsSuccess to check the result:
```csharp
var input = 1;

var result = NotZero(input);

Assert.That(result.IsSuccess, Is.True);
Assert.That(result.Value, Is.EqualTo(input));
```

Use the Error property when the result IsFailure to check the error:
```csharp
var input = 0;

var result = NotZero(input);

Assert.That(result.IsFailure, Is.True);
Assert.That(result.Error, Is.EqualTo("Input may not be zero"));
```

Accessing the Value when the result is a failure will throw an InvalidOperationException. 
```csharp

var input = 0;

var result = NotZero(input);

Assert.That(result.IsFailure, Is.True);
Assert.That(() => result.Value, Throws.InvalidOperationException);
```

Accessing the Error when the result is a success will throw an InvalidOperationException.
```csharp
var input = 1;

var result = NotZero(input);
            
Assert.That(result.IsSuccess, Is.True);
Assert.That(() => result.Error, Throws.InvalidOperationException);
```

When the value type and error type are the same, implicit conversion doesn't work. Use the static Success and Failure methods to create the result:
```csharp
private static SimpleResults.Result<string, string> Validated(string? input)
{
    if (string.IsNullOrWhiteSpace(input))
        return SimpleResults.Result<string, string>.Failure("Input may not be null or whitespace");

    return SimpleResults.Result<string, string>.Success(input);
}
```

In practice, having the same type for both value and error should be rare, but this pattern allows you to handle such cases cleanly.

In my experience, having a dedicated error type is often more beneficial, as it allows for richer error handling and avoids confusion with the value type. For example:
```csharp
private struct NotZeroError;

private static SimpleResults.Result<int, NotZeroError> NotZero(int input)
{
    if (input == 0)
        return new NotZeroError();

    return input;
}
```

## Philosophy
Many methods don't have a single return value, but rather a success or failure value. Using exceptions for control flow is both expensive and confusing, so many developers resort to using a Result object of some kind.

The SimpleResults library is designed to provide a straightforward and efficient way to handle success and failure in C# applications. It aims to reduce boilerplate code while maintaining clarity and type safety. The use of implicit conversions simplifies the creation of results, making it easy to work with both success values and errors without excessive verbosity.

For this library, I decided not to use a Match function to access the values, as is common in a functional programming style. This is a conscious design choice. In my experience, using Match is confusing for many developers who aren't familiar with a functional style of programming. Once you go the functional route, it affects the rest of your codebase, which is not always desirable. Instead, I opted for a more traditional approach that leverages properties like `Value` and `Error`, making it easier for C# developers to understand and use without needing to learn new patterns.

I've added `[MemberNotNullWhen]` attributes to the properties to improve nullability analysis, ensuring that the compiler can better understand when these properties are guaranteed to be non-null.

## License

MIT