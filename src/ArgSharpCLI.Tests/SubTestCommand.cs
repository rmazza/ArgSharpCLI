using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;

namespace ArgSharpCLI.Tests;

[Command("subcommand", Description = "Sub Test command.")]
public class SubTestCommand : ICommand
{
    [Option("test-option", "t", "test option")]
    public string? TestOption { get; set; }

    [Option("test-boolean-option", "b", "test boolean option")]
    public bool TestBooleanOption1 { get; set; }

    public void Run()
    {
        
    }
}
