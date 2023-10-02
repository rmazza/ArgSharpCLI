using System;
using System.Collections.Generic;
using System.Linq;
using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI;

public class CommandRunnerBuilder : ICommandRunnerBuilder
{
    private readonly Dictionary<string, Type> _commands = new();
    private readonly List<string> _arguments = new();

    public ICommandRunnerBuilder AddArguments(string[] args)
    {
        Ensure.IsNotNull(args, nameof(args));

        _arguments.AddRange(args);
        return this;
    }

    public ICommandRunnerBuilder AddCommand<T>() where T : ICommand
    {
        CommandAttribute? attribute = typeof(T)
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr is CommandAttribute) as CommandAttribute;

        if (attribute is null)
            throw new InvalidOperationException($"The type {typeof(T).Name} must have a {nameof(CommandAttribute)}.");

        _commands.Add(attribute.Name, typeof(T));
        return this;
    }

    public ICommand Build()
    {
        bool isOptionBeforeCommand = _arguments[0].StartsWith("-");

        if (isOptionBeforeCommand)
        {
            // Todo: implement cli option, ie (help, version)
        }

        var strCommand = _arguments[0];

        if (_commands.TryGetValue(strCommand, out var command) 
            && Activator.CreateInstance(command) is ICommand cmd)
        {
            return cmd;
        }

        return null;
    }

}
