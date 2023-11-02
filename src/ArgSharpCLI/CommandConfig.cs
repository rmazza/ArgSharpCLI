using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgSharpCLI;

internal class CommandConfig : ICommandConfig
{
    private readonly Dictionary<string, Type> _subCommands = new();

    public ICommandConfig AddSubCommand<T>()
    {
        if (typeof(T)
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr is CommandAttribute) is not CommandAttribute attribute)
            throw new InvalidOperationException($"The type {typeof(T).Name} must have a {nameof(CommandAttribute)}.");

        _subCommands.Add(attribute.Name, typeof(T));

        return this;
    }

    public Dictionary<string, Type> GetSubCommands() => _subCommands;
}
