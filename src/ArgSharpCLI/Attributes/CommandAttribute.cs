using ArgSharpCLI.Interfaces;
using System;

namespace ArgSharpCLI.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute, ICommandAttribute
{
    public string Name { get; }
    public string? Description { get; set; }

    public CommandAttribute(string commandName)
    {
        Name = commandName;
    }
}