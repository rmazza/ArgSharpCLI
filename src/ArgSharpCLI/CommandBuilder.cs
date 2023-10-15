using ArgSharpCLI.Attributes;
using ArgSharpCLI.Commands;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Interfaces;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI;

public class CommandBuilder : ICommandBuilder
{
    private ICommand? _customGlobalHelpCommand;

    private readonly Dictionary<string, Type> _commands = new();
    private readonly Queue<string> _argumentQueue = new();

    public ICommandBuilder AddCustomGlobalHelpCommand(ICommand customGlobalHelpCommand)
    {
        _customGlobalHelpCommand = customGlobalHelpCommand;
        return this;
    }

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
                var helpCommand = _customGlobalHelpCommand ?? GenerateGlobalHelp();
                return new Result<ICommand>(helpCommand);
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
        return new GlobalHelpCommand(_commands);
    }

    private ICommand GenerateSpecificHelp(ICommand cmd)
    {
        return new HelpCommand(cmd);
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