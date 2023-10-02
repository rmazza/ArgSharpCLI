using ArgSharpCLI.Attributes;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI.Tests;

[Command("test")]
public class TestCommand : ICommand
{
    public void Print() =>
        throw new NotImplementedException();

    public void Run() =>
        throw new NotImplementedException();
}


