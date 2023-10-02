using System;
using System.Collections.Generic;

namespace ArgSharpCLI;
public class Command
{
    private readonly HashSet<Type> _commands;
    public Command(HashSet<Type> commands)
    {
        _commands = commands;
    }
}
