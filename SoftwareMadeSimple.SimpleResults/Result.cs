using System.Diagnostics.CodeAnalysis;

namespace SoftwareMadeSimple.SimpleResults;

public sealed class Result<T, E>
{
    private readonly T? value;
    private readonly E? error;

    private Result(T value)
    {
        IsSuccess = true;
        this.value = value;
    }

    private Result(E error)
    {
        IsSuccess = false;
        this.error = error;
    }

    public T Value
    {
        get
        {
            if (IsFailure)
                throw new InvalidOperationException("Cannot access Value when result is a failure.");

            return value!;
        }
    }

    public E Error
    {
        get
        {
            if (IsSuccess)
                throw new InvalidOperationException("Cannot access Error when result is a success.");

            return error!;
        }
    }

    public static Result<T, E> Success(T value) => new(value);

    public static Result<T, E> Failure(E error) => new(error);

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFailure => !IsSuccess;

    public static implicit operator Result<T, E>(T value) => new(value);
    public static implicit operator Result<T, E>(E error) => new(error);
}