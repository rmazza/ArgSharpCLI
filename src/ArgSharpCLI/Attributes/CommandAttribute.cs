using System;
using System.Collections.Generic;
using System.Text;

namespace ArgSharpCLI.Attributes;

public interface ICommandAttribute
{
    string Name { get; }
    string Description { get; set;  }
}

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