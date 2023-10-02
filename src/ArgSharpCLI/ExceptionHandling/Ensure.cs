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
}