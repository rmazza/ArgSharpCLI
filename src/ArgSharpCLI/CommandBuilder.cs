using ArgSharpCLI.Attributes;
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

        foreach(var arg in args)
            _argumentQueue.Enqueue(arg);
      
        return this;
    }

    public ICommandBuilder AddCommand<T>() where T : ICommand
    {
        CommandAttribute? attribute = typeof(T)
            .GetCustomAttributes(false)
            .SingleOrDefault(attr => attr is CommandAttribute) as CommandAttribute;

        if (attribute is null)
            throw new InvalidOperationException($"The type {typeof(T).Name} must have a {nameof(CommandAttribute)}.");

        _commands.Add(attribute.Name, typeof(T));
        return this;
    }

    public Result<ICommand> Build()
    {
        if (!_argumentQueue.TryDequeue(out string argument))
            return new Result<ICommand>(new CommandNotFoundException());

        bool isOptionBeforeCommand = argument.StartsWith("-");

        if (isOptionBeforeCommand)
        {

            // Todo: implement cli option, ie (help, version)
            return new Result<ICommand>(new NotImplementedException("Options before command are not yet implemented"));
        }

        var strCommand = argument;

        if (_commands.TryGetValue(strCommand, out var command)
            && Activator.CreateInstance(command) is ICommand cmd)
        {
            new OptionParser(cmd)
                .BuildOptions(_argumentQueue);
            return new Result<ICommand>(cmd);
        }

        return new Result<ICommand>(new CommandNotFoundException());
    }
   
}