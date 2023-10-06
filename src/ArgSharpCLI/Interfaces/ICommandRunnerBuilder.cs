using LanguageExt.Common;

namespace ArgSharpCLI.Interfaces
{
    public interface ICommandRunnerBuilder
    {
        ICommandRunnerBuilder AddArguments(string[] args);
        ICommandRunnerBuilder AddCommand<T>() where T : ICommand;
        Result<ICommand> Build();
    }
}