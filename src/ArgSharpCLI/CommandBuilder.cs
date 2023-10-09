using ArgSharpCLI.Attributes;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Extensions;
using ArgSharpCLI.Interfaces;
using LanguageExt.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        var optionDictionary = new Dictionary<string, PropertyInfo>();

        var optionProperties = cmd.GetType()
                                  .GetProperties()
                                  .GetOptionProperties();

        foreach (var property in optionProperties)
        {
            IOptionAttribute attribute = property.GetOptionAttribute();

            if (string.IsNullOrEmpty(attribute.LongName))
                throw new Exception("Long name attribute cannot be null");

            optionDictionary.TryAdd(attribute.LongName, property);

            if (!string.IsNullOrEmpty(attribute.ShortName))
                optionDictionary.TryAdd(attribute.ShortName, property);
        }

        for (int i = 1; i < args.Count; i++)
        {
            var argument = args[i];

            if (argument.StartsWith("--") && optionDictionary.TryGetValue(argument[2..], out PropertyInfo property))
            {
                if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(cmd, true);
                    continue;
                }

                if (i == args.Count - 1)
                    throw new Exception("No value provided for argument");

                if (args[i + 1].StartsWith("-") || string.IsNullOrWhiteSpace(args[i + 1]))
                    throw new Exception($"no value provided for {argument}");

                property.SetValue(cmd, args[i++ + 1]);

                continue;
            }

            // short hand parsing
            if (argument.StartsWith("-"))
            {
                // all must be boolean properties to group shorthands
                if (argument.Length > 2)
                {
                    foreach (var opt in argument[1..])
                    {
                        if (optionDictionary.TryGetValue(opt.ToString(), out PropertyInfo sp))
                        {
                            if (sp.PropertyType != typeof(bool))
                                throw new InvalidCommandException("Grouping short hand properties must all be boolean types");

                            sp.SetValue(cmd, true);
                            continue;
                        }

                        throw new CommandNotFoundException($"{ opt } not found");
                    }
                }

                string shortHandOption = argument[1].ToString();

                if (optionDictionary.TryGetValue(shortHandOption, out PropertyInfo shortProperty))
                {
                    if (shortProperty.PropertyType == typeof(bool))
                    {
                        shortProperty.SetValue(cmd, true);
                        continue;
                    }

                    if (i == args.Count - 1)
                        throw new Exception("No value provided for argument");

                    if (args[i + 1].StartsWith("-") || string.IsNullOrWhiteSpace(args[i + 1]))
                        throw new Exception($"no value provided for {argument}");

                    shortProperty.SetValue(cmd, args[i++ + 1]);
                    continue;
                }
                else
                {
                    throw new CommandNotFoundException($"{shortHandOption} not found");
                }
            }

            throw new CommandNotFoundException($"{argument} is not a valid argument");

        }
    }
}
