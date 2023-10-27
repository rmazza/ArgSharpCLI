namespace ArgSharpCLI.Extensions;
public static class StringExtensions
{
    public static bool IsOption(this string option) =>
        IsHelpOption(option) || IsLongOption(option) || IsShortOption(option);
    public static bool IsHelpOption(this string str) =>
        string.Equals(str, "-h", System.StringComparison.InvariantCultureIgnoreCase) ||
        string.Equals(str, "--help", System.StringComparison.InvariantCultureIgnoreCase) ||
        string.Equals(str, "help", System.StringComparison.InvariantCultureIgnoreCase);

    public static bool IsLongOption(this string str) => 
        str.StartsWith("--");

    public static bool IsShortOption(this string str) => 
        str.StartsWith("-");
}

