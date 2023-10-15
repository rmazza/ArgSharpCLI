using ArgSharpCLI.Extensions;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ArgSharpCLI.Commands;

public class GlobalHelpCommand : ICommand
{
    private readonly Dictionary<string, Type> _commands;

    public GlobalHelpCommand(Dictionary<string, Type> commands)
    {
        _commands = commands;
    }

    public string GetHelpText()
    {
        var helpText = new StringBuilder();
        helpText.Append("Global Options:\n");
        helpText.Append("  -h|--help        Show command line help.\n");

        helpText.Append("\nAvailable commands:\n");

        foreach (var cmd in _commands)
        {
            var commandAttribute = cmd.Value.GetCommandAttribute();

            helpText.Append($"  {commandAttribute.Name}             {commandAttribute.Description}\n");
        }

        return helpText.ToString();
    }
    public void Run()
    {
        Debug.WriteLine( GetHelpText() );
        Console.WriteLine(GetHelpText());
    }
}
