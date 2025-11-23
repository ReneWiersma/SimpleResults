using System.Diagnostics.CodeAnalysis;

namespace SoftwareMadeSimple.SimpleResults;

public sealed class Success<T, E>(T value) : Result<T, E>
{
    public override T Value { get; } = value;

    public override E Error => throw new InvalidOperationException("Cannot access Error when result is a success.");

    public override bool IsSuccess => true;
}

public sealed class Failure<T, E>(E error) : Result<T, E>
{
    public override T Value => throw new InvalidOperationException("Cannot access Value when result is a failure.");

    public override E Error { get; } = error;

    public override bool IsSuccess => false;
}

public abstract class Result<T, E>
{
    public abstract T Value { get; }

    public abstract E Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public abstract bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFailure => !IsSuccess;

    public static Result<T, E> Success(T value) => new Success<T, E>(value);

    public static Result<T, E> Failure(E error) => new Failure<T, E>(error);

    public static implicit operator Result<T, E>(T value) => new Success<T, E>(value);

    public static implicit operator Result<T, E>(E error) => new Failure<T, E>(error);
}