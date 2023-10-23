using ArgSharpCLI.Attributes;
using ArgSharpCLI.Commands;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Interfaces;
using LanguageExt;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI;

public interface ICommandConfig<T>
{
    Dictionary<string, Type> SubCommands { get; }
    ICommandConfig<T> AddSubCommand<T2>();
}

internal class CommandConfig<T> : ICommandConfig<T>
{
    public Dictionary<string, Type> SubCommands { get; } = new();

    public ICommandConfig<T> AddSubCommand<T2>()
    {
        if (typeof(T2)
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr is CommandAttribute) is not CommandAttribute attribute)
            throw new InvalidOperationException($"The type {typeof(T2).Name} must have a {nameof(CommandAttribute)}.");

        SubCommands.Add(attribute.Name, typeof(T2));

        return this;

    }
}

public class CommandBuilder : ICommandBuilder
{
    private ICommand? _customGlobalHelpCommand;

    private readonly Dictionary<string, Type> _commands = new();
    private readonly Queue<string> _argumentQueue = new();
    private readonly Dictionary<Type, Dictionary<string, Type>> _subCommands = new();

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

    public ICommandBuilder AddCommand<T>(Action<ICommandConfig<T>> addSubCommands)
        where T : ICommand
    {
        var attribute = GetCommandAttribute<T>();

        _commands.Add(attribute.Name, typeof(T));

        ICommandConfig<T> commandConfig = new CommandConfig<T>();
        addSubCommands(commandConfig);

        _subCommands.Add(typeof(T), commandConfig.SubCommands);

        return this;
    }

    public Result<ICommand> Build()
    {
        var optionParser = new OptionParser(_argumentQueue);

        ICommand? command;
        if (optionParser.IsHelpRequested())
        {
            if (_argumentQueue.Count == 1)
            {
                var helpCommand = _customGlobalHelpCommand ?? GenerateGlobalHelp();
                return new Result<ICommand>(helpCommand);
            }

            command = GetCommandFromQueue(_argumentQueue, _commands);
            if (command != null)
            {
                return new Result<ICommand>(GenerateSpecificHelp(command));
            }

        }

        command = GetCommandFromQueue(_argumentQueue, _commands);
        _ = _argumentQueue.Dequeue();

        if (command is null)
            return new Result<ICommand>(new CommandNotFoundException());

        optionParser
            .SetCommand(command)
            .BuildOptions();

        // has subcommand
        if (_argumentQueue.Any())
        {
            command = GetCommandFromQueue(_argumentQueue, _subCommands[command.GetType()]);
            optionParser.SetCommand(command).BuildOptions();

        }

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

    private static ICommand GetCommandFromQueue(Queue<string> argumentQueue, Dictionary<string, Type> commands)
    {
        if (!argumentQueue.TryPeek(out string argument))
            return new EmptyCommand();

        if (commands.TryGetValue(argument, out Type commandType) && Activator.CreateInstance(commandType) is ICommand cmd)
        {
            return cmd;
        }

        return new EmptyCommand();
    }

    private static ICommandAttribute GetCommandAttribute<T>() where T : ICommand
    {
        return GetCommandAttribute(typeof(T));
    }

    private static ICommandAttribute GetCommandAttribute(Type t)
    {
        if (t
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr is CommandAttribute) is not CommandAttribute attribute)
            throw new InvalidOperationException($"The type {t.Name} must have a {nameof(CommandAttribute)}.");
        return attribute;
    }

}