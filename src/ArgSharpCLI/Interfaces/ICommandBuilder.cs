﻿using LanguageExt.Common;
using System;
using System.Collections.Generic;

namespace ArgSharpCLI.Interfaces
{
    public interface ICommandBuilder
    {
        ICommandBuilder AddArguments(string[] args);
        ICommandBuilder AddCommand<T>() where T : ICommand;
        ICommandBuilder AddCommand<T>(Action<ICommandConfig<T>> addSubCommands) where T : ICommand;
        Result<ICommand> Build();
    }
}