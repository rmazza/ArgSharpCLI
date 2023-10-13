using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        helpText.Append("Available commands:\n");

        foreach (var cmd in _commands)
        {
            helpText.Append($"- {cmd.Key}\n");
        }

        return helpText.ToString();
    }

    public void Print()
    {
        throw new NotImplementedException();
    }

    public void Run()
    {
        throw new NotImplementedException();
    }

    // ... other ICommand implementations
}
