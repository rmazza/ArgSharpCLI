using ArgSharpCLI.Attributes;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI.Tests.Commands;

[Command("test", Description = "Test command.")]
public class TestCommand : ICommand
{
    [Option("test-option", "t", "test option")]
    public string? TestOption { get; set; }

    [Option("test-boolean-option", "b", "test boolean option")]
    public bool TestBooleanOption1 { get; set; }

    [Option("test-boolean-option-2", "z", "test boolean option 2")]
    public bool TestBooleanOption2 { get; set; }

    public void Run() =>
        throw new NotImplementedException();
}