using System.Collections.Generic;

namespace ArgSharpCLI.Interfaces;

public interface ICommand
{
    void Run();

    string GetHelpText()
    {
        // Use reflection to gather and format option info
        // return the formatted string
        return string.Empty;
    }
}

