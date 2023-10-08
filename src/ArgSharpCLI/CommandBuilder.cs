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
        var optionDictionary = new Dictionary<string, PropertyInfo>();

        var optionProperties = cmd.GetType()
                                  .GetProperties()
                                  .GetOptionProperties();

        foreach(var property in optionProperties)
        {
            IOptionAttribute attribute = property.GetOptionAttribute();

            if (string.IsNullOrEmpty(attribute.LongName))
                throw new Exception("Long name attribute cannot be null");

            optionDictionary.TryAdd(attribute.LongName, property);

            if (!string.IsNullOrEmpty(attribute.ShortName))
                optionDictionary.TryAdd(attribute.ShortName, property);
        }

        for(int i = 1; i < args.Count; i++) 
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

            if (argument.StartsWith("-"))
            {

            }

            throw new CommandNotFoundException($"{argument} is not a valid argument");

        }

        //// Todo: move into separate method
        //foreach (var property in optionProperties)
        //{
        //    IOptionAttribute attribute = property.GetOptionAttribute();
        //    var index = args.IndexOf($"--{attribute.LongName}");
        //    if (index == -1)
        //    {
        //        index = args.IndexOf($"-{ attribute.ShortName}");
        //    }

        //    if (index != -1 && property.PropertyType == typeof(bool))
        //    {
        //        property.SetValue(cmd, true);
        //    }
        //    else if (index != -1 && index + 1 < args.Count)
        //    {
        //        // Assigning the next value after the flag to the property
        //        property.SetValue(cmd, args[index + 1]);
        //    }
        //}
    }

    private static void SetValue(ICommand cmd, PropertyInfo prop, object value)
    {
        switch(prop.PropertyType)
        {
            case Type t when t == typeof(bool):
                prop.SetValue(cmd, true);
                break;
            default:
                prop.SetValue(cmd, value);
                break;
        }
    }
}
   