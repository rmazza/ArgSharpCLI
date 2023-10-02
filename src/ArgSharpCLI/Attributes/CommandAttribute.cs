using System;
using System.Collections.Generic;
using System.Text;

namespace ArgSharpCLI.Attributes;

public class CommandAttribute : Attribute
{
    public string Name { get; }

    public CommandAttribute(string commandName)
    {
        Name = commandName;
    }
}