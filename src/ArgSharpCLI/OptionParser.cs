using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Extensions;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ArgSharpCLI;

internal class OptionParser
{
    private readonly Queue<string> _arguments;
    private readonly ICommand _cmd;
    private readonly IDictionary<string, PropertyInfo> _optionDictionary;

    public OptionParser(ICommand command, Queue<string> arguments)
    {
        _cmd = command;
        _arguments = arguments;
        _optionDictionary = GetOptionAttributesFromCommandProperties(command);
    }

    public ICommand BuildOptions(Func<ICommand, ICommand> helpFunction)
    {
        Ensure.IsNotNull(_cmd, "Command cannot be null");

        while (_arguments.Count > 0)
        {
            _arguments.TryPeek(out string argument);

            if (!argument.IsOption())
            {
                return _cmd;
            }

            _arguments.Dequeue();

            if (argument.IsHelpOption())
            {
                return helpFunction(_cmd);
            }
            
            if (argument.IsLongOption())
            {
                HandleLongOption(argument, _arguments);
            }
            else if (argument.IsShortOption())
            {
                HandleShortOption(argument, _arguments);
            }
        }

        return _cmd;
    }

    private void HandleLongOption(string argument, Queue<string> args)
    {
        if (!_optionDictionary.TryGetValue(argument[2..], out PropertyInfo property))
            throw new CommandNotFoundException("");

        SetValue(property, argument, args);
    }

    private void HandleShortOption(string argument, Queue<string> args)
    {
        // for grouped shorthand options
        if (argument.Length <= 2)
        {
            HandleSingleShortOption(argument, args);
            return;
        }

        foreach (var opt in argument[1..])
        {
            HandleGroupedShortOption(opt);
        }
    }

    private void HandleGroupedShortOption(char opt)
    {
        if (!_optionDictionary.TryGetValue(opt.ToString(), out PropertyInfo sp))
            throw new CommandNotFoundException($"{opt} not found");

        if (sp.PropertyType != typeof(bool))
            throw new InvalidCommandException("Grouping short hand properties must all be boolean types");

        sp.SetValue(_cmd, true);
    }

    private void HandleSingleShortOption(string argument, Queue<string> args)
    {
        if (!_optionDictionary.TryGetValue(argument[1].ToString(), out PropertyInfo shortProperty))
            throw new CommandNotFoundException($"{argument[1]} not found");

        SetValue(shortProperty, argument, args);
    }

    private void SetValue(PropertyInfo property, string argument, Queue<string> args)
    {
        if (property.PropertyType == typeof(bool))
        {
            property.SetValue(_cmd, true);
            return;
        }

        if (!args.TryPeek(out string result))
            throw new Exception("No value provided for argument");

        if (result.StartsWith("-") || string.IsNullOrWhiteSpace(result))
            throw new Exception($"No value provided for {argument}");

        property.SetValue(_cmd, result);
        _ = args.Dequeue();
    }

    private static IDictionary<string, PropertyInfo> GetOptionAttributesFromCommandProperties(ICommand cmd)
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

        return optionDictionary;
    }
}
