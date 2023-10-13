using ArgSharpCLI.Attributes;
using ArgSharpCLI.Commands;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Interfaces;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI;

public class CommandBuilder : ICommandBuilder
{
    private readonly Dictionary<string, Type> _commands = new();
    private readonly Queue<string> _argumentQueue = new();

    public ICommandBuilder AddArguments(string[] args)
    {
        Ensure.IsNotNull(args, nameof(args));

        foreach (var arg in args)
            _argumentQueue.Enqueue(arg);

        return this;
    }

    public ICommandBuilder AddCommand<T>() where T : ICommand
    {
        if (typeof(T)
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr is CommandAttribute) is not CommandAttribute attribute)
            throw new InvalidOperationException($"The type {typeof(T).Name} must have a {nameof(CommandAttribute)}.");

        _commands.Add(attribute.Name, typeof(T));
        return this;
    }

    public Result<ICommand> Build()
    {
        var optionParser = new OptionParser(_argumentQueue);

        ICommand? command = null;

        if (optionParser.IsHelpRequested())
        {
            if (_argumentQueue.Count == 1)
            {
                return new Result<ICommand>(GenerateGlobalHelp());
            }

            command = GetCommandFromQueue();
            if (command != null)
            {
                return new Result<ICommand>(GenerateSpecificHelp(command));
            }

        }

        command = GetCommandFromQueue();

        if (command is null)
            return new Result<ICommand>(new CommandNotFoundException());

        optionParser
            .SetCommand(command)
            .BuildOptions();

        return new Result<ICommand>(command);
    }

    private ICommand GenerateGlobalHelp()
    {
        // Generate global help text based on registered commands
        return new GlobalHelpCommand(_commands);
    }

    private ICommand GenerateSpecificHelp(ICommand cmd)
    {
        // Generate help text for a specific command based on its options
        return cmd; // Or wrap it in a help-command decorator that prints its help info
    }

    private ICommand GetCommandFromQueue()
    {
        if (!_argumentQueue.TryPeek(out string argument))
            return null;

        if (_commands.TryGetValue(argument, out Type commandType) && Activator.CreateInstance(commandType) is ICommand cmd)
        {
            return cmd;
        }

        return null;
    }

}