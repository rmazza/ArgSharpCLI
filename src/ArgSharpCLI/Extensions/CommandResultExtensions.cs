using ArgSharpCLI.Core;
using ArgSharpCLI.ExceptionHandling;
using System;

namespace ArgSharpCLI.Extensions;

public static class CommandResultExtensions
{
    public static T2 MatchResult<T, T2>(
        this CommandResult<T> result,
        Func<T, T2> onSuccess,
        Func<Error, T2> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.Error);
    }
}