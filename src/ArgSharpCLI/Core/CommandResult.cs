using ArgSharpCLI.ExceptionHandling;
using System;

namespace ArgSharpCLI.Core;

public class CommandResult<T>
{
    private CommandResult(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        Value = default;
    }

    private CommandResult(T value, bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public T Value { get; private set; }

    public Error Error { get; }

    public static CommandResult<T> Success() => new(true, Error.None);

    public static CommandResult<T> Success(T value) => new CommandResult<T>(value, true, Error.None);

    public static CommandResult<T> Failure(Error error) => new(false, error);
}