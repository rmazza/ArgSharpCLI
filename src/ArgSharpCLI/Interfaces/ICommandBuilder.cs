using ArgSharpCLI.Core;
using System;

namespace ArgSharpCLI.Interfaces
{
    public interface ICommandBuilder
    {
        ICommandBuilder AddArguments(string[] args);
        ICommandBuilder AddCommand<T>() where T : ICommand;
        ICommandBuilder AddCommand<T1, T2>()
            where T1 : ICommand
            where T2 : ICommand;

        ICommandBuilder AddCommand<T1, T2, T3>()
            where T1 : ICommand
            where T2 : ICommand
            where T3 : ICommand;

        ICommandBuilder AddCommand<T1, T2, T3, T4>()
            where T1 : ICommand
            where T2 : ICommand
            where T3 : ICommand
            where T4 : ICommand;

        ICommandBuilder AddCommand<T>(Action<ICommandConfig> addSubCommands) where T : ICommand;
        CommandResult<ICommand> Build();
    }
}