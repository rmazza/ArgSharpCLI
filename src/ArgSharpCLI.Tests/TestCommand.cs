using ArgSharpCLI.Attributes;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI.Tests;

[Command("test")]
public class TestCommand : ICommand
{
    [Option("test-option", "t", "test option")]
    public string? TestOption { get; set; }

    [Option("test-boolean-option", "b", "test boolean option")]
    public bool TestBooleanOption { get; set; }

    public void Print() =>
        throw new NotImplementedException();

    public void Run() =>
        throw new NotImplementedException();
}


