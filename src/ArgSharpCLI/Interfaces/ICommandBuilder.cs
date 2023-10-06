using LanguageExt.Common;

namespace ArgSharpCLI.Interfaces
{
    public interface ICommandBuilder
    {
        ICommandBuilder AddArguments(string[] args);
        ICommandBuilder AddCommand<T>() where T : ICommand;
        Result<ICommand> Build();
    }
}