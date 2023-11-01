using ArgSharpCLI.Attributes;
using ArgSharpCLI.Commands;
using ArgSharpCLI.Core;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI;

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
        AddTypeToCommandDictionary(new[] { typeof(T) });
        return this;
    }

    public ICommandBuilder AddCommand<T1, T2>()
            where T1 : ICommand
            where T2 : ICommand
    {
        AddTypeToCommandDictionary(new[] { typeof(T1), typeof(T2) });
        return this;
    }

    public ICommandBuilder AddCommand<T1, T2, T3>()
            where T1 : ICommand
            where T2 : ICommand
            where T3 : ICommand
    {
        AddTypeToCommandDictionary(new[] { typeof(T1), typeof(T2), typeof(T3) });
        return this;
    }

    public ICommandBuilder AddCommand<T1, T2, T3, T4>()
            where T1 : ICommand
            where T2 : ICommand
            where T3 : ICommand
            where T4 : ICommand
    {
        AddTypeToCommandDictionary(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
        return this;
    }

    public ICommandBuilder AddCommand<T>(Action<ICommandConfig> addSubCommands)
        where T : ICommand
    {
        var attribute = GetCommandAttribute<T>();

        _commands.Add(attribute.Name, typeof(T));

        ICommandConfig commandConfig = new CommandConfig();
        addSubCommands(commandConfig);

        _subCommands.Add(typeof(T), commandConfig.GetSubCommands());

        return this;
    }

    public CommandResult<ICommand> Build()
    {
        ICommand command = GetCommandFromQueue(_argumentQueue, _commands);

        if (command is not EmptyCommand)
            _ = _argumentQueue.Dequeue();

        command = BuildOptions(command);

        // has subcommand
        while (_argumentQueue.Any())
        {
            command = GetCommandFromQueue(_argumentQueue, _subCommands[command.GetType()]);
            _ = _argumentQueue.Dequeue();
            command = BuildOptions(command);
        }

        return CommandResult<ICommand>.Success(command);
    }

    private ICommand BuildOptions(ICommand command)
    {
        command = new OptionParser(command, _argumentQueue)
            .BuildOptions(MapHelpCommand());
        return command;
    }

    private Func<ICommand, ICommand> MapHelpCommand() =>
            cmd =>
                cmd switch
                {
                    EmptyCommand => GenerateGlobalHelp(_commands),
                    ICommand => GenerateSpecificHelp(cmd),
                    _ => throw new Exception("Help Command not found")
                };

    private void AddTypeToCommandDictionary(Type[] commandTypes)
    {
        foreach (var t in commandTypes)
        {
            if (t
               .GetCustomAttributes(false)
               .SingleOrDefault(attr => attr is CommandAttribute) is not CommandAttribute attribute)
                throw new InvalidOperationException($"The type {t.Name} must have a {nameof(CommandAttribute)}.");
            _commands.Add(attribute.Name, t);
        }
    }

    private static ICommand GenerateGlobalHelp(Dictionary<string, Type> commands) =>
        new GlobalHelpCommand(commands);

    private static ICommand GenerateSpecificHelp(ICommand cmd)
    {
        return new HelpCommand(cmd);
    }

    private static ICommand GetCommandFromQueue(Queue<string> argumentQueue, Dictionary<string, Type> commands)
    {
        if (!argumentQueue.TryPeek(out string argument))
            return new EmptyCommand();

        if (commands.TryGetValue(argument, out Type commandType)
            && Activator.CreateInstance(commandType) is ICommand cmd)
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