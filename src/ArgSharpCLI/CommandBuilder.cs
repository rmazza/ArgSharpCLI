using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using ArgSharpCLI.Attributes;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Extensions;
using ArgSharpCLI.Interfaces;
using LanguageExt;
using LanguageExt.Common;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI;

public class CommandBuilder : ICommandBuilder
{
    private readonly Dictionary<string, Type> _commands = new();
    private readonly List<string> _arguments = new();

    public ICommandBuilder AddArguments(string[] args)
    {
        Ensure.IsNotNull(args, nameof(args));

        _arguments.AddRange(args);
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
        bool isOptionBeforeCommand = _arguments[0].StartsWith("-");

        if (isOptionBeforeCommand)
        {

            // Todo: implement cli option, ie (help, version)
            return new Result<ICommand>(new NotImplementedException("Options before command are not yet implemented"));
        }
        
        var strCommand = _arguments[0];

        if (_commands.TryGetValue(strCommand, out var command)
            && Activator.CreateInstance(command) is ICommand cmd)
        {
            BuildOptions(cmd, _arguments);
            return new Result<ICommand>(cmd);
        }

        return new Result<ICommand>(new CommandNotFoundException());
    }

    public static void BuildOptions(ICommand cmd, List<string> args)
    {
        var optionProperties = cmd.GetType()
                                  .GetProperties()
                                  .GetOptionProperties();

        foreach (var property in optionProperties)
        {
            IOptionAttribute attribute = property.GetOptionAttribute();
            var index = args.IndexOf($"--{attribute.LongName}");
            if (index == -1)
            {
                index = args.IndexOf($"-{ attribute.ShortName}");
            }

            if (index != -1 && property.PropertyType == typeof(bool))
            {
                property.SetValue(cmd, true);
            }
            else if (index != -1 && index + 1 < args.Count)
            {
                // Assigning the next value after the flag to the property
                property.SetValue(cmd, args[index + 1]);
            }
        }
    }
}
   