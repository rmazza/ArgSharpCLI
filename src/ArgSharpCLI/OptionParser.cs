﻿using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Extensions;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ArgSharpCLI;

internal class OptionParser
{
    private readonly Queue<string> _arguments;

    private ICommand? _cmd;
    private IDictionary<string, PropertyInfo>? _optionDictionary;

    public OptionParser(Queue<string> arguments) => _arguments = arguments;

    public void BuildOptions()
    {
        Ensure.IsNotNull(_cmd, "Command cannot be null");
        
        while (_arguments.Count > 0)
        {
            var argument = _arguments.Dequeue();

            if (IsLongOption(argument))
            {
                HandleLongOption(argument, _arguments);
            }
            else if (IsShortOption(argument))
            {
                HandleShortOption(argument, _arguments);
            }
        }
    }

    public bool IsHelpRequested()
    {
        return _arguments.Contains("-h") || _arguments.Contains("--help");
    }

    private bool IsLongOption(string arg) => arg.StartsWith("--");

    private bool IsShortOption(string arg) => arg.StartsWith("-");

    private void HandleLongOption(string argument, Queue<string> args)
    {
        if (!_optionDictionary.TryGetValue(argument[2..], out PropertyInfo property))
            throw new CommandNotFoundException("");

        SetValue(property, argument, args);
    }

    private void HandleShortOption(string argument, Queue<string> args)
    {
        // for grouped shorthand options
        if (argument.Length > 2)
        {
            foreach (var opt in argument[1..])
            {
                HandleGroupedShortOption(opt, args);
            }
        }
        else
        {
            HandleSingleShortOption(argument, args);
        }
    }

    private void HandleGroupedShortOption(char opt, Queue<string> args)
    {
        if (_optionDictionary.TryGetValue(opt.ToString(), out PropertyInfo sp))
        {
            if (sp.PropertyType != typeof(bool))
                throw new InvalidCommandException("Grouping short hand properties must all be boolean types");

            sp.SetValue(_cmd, true);
        }
        else
        {
            throw new CommandNotFoundException($"{opt} not found");
        }
    }

    private void HandleSingleShortOption(string argument, Queue<string> args)
    {
        if (_optionDictionary.TryGetValue(argument[1].ToString(), out PropertyInfo shortProperty))
        {
            SetValue(shortProperty, argument, args);
        }
        else
        {
            throw new CommandNotFoundException($"{argument[1]} not found");
        }
    }

    private void SetValue(PropertyInfo property, string argument, Queue<string> args)
    {
        if (property.PropertyType == typeof(bool))
        {
            property.SetValue(_cmd, true);
        }
        else
        {
            if (!args.TryPeek(out string result))
                throw new Exception("No value provided for argument");

            if (result.StartsWith("-") || string.IsNullOrWhiteSpace(result))
                throw new Exception($"No value provided for {argument}");

            property.SetValue(_cmd, result);
            _ = args.Dequeue();
        }
    }

    private IDictionary<string, PropertyInfo> GetOptionAttributesFromCommandProperties(ICommand cmd)
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

    internal OptionParser SetCommand(ICommand cmd)
    {
        _cmd = cmd;
        _optionDictionary = GetOptionAttributesFromCommandProperties(cmd);
        return this;
    }
}