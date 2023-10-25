using LanguageExt.Common;
using System;

namespace ArgSharpCLI.Interfaces
{
    public interface ICommandBuilder
    {
        ICommandBuilder AddArguments(string[] args);
        ICommandBuilder AddCommand<T>() where T : ICommand;
        ICommandBuilder AddCommand<T>(Action<ICommandConfig> addSubCommands) where T : ICommand;
        Result<ICommand> Build();
    }
}