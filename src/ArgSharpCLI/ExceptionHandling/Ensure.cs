using System;
using System.Linq;

namespace ArgSharpCLI.ExceptionHandling;

internal static class Ensure
{
    public static void IsNotNull(string[] args, string message)
    {
        if (!args.Any())
            throw new ArgumentNullException(nameof(args), message);
    }

    public static void IsNotNull(object obj, string message)
    {
        if (obj is null)
            throw new ArgumentNullException(message);
    }
}