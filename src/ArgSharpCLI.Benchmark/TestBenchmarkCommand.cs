using ArgSharpCLI.Attributes;
using ICommand = ArgSharpCLI.Interfaces.ICommand;
using OptionAttribute = ArgSharpCLI.Attributes.OptionAttribute;

namespace ArgSharpCLI.Tests;

[Command("test")]
public class TestBenchmarkCommand : ICommand
{
    [Option("test-option", "t", "test option")]
    public string? TestOptionT { get; set; }

    [Option("test-option", "u", "test option")]
    public string? TestOptionU { get; set; }

    [Option("test-option", "v", "test option")]
    public string? TestOptionV { get; set; }

    [Option("test-boolean-option", "x", "test boolean option")]
    public bool TestBooleanOptionX { get; set; }

    [Option("test-boolean-option-2", "y", "test boolean option 2")]
    public bool TestBooleanOptionY { get; set; }

    [Option("test-boolean-option-2", "z", "test boolean option 2")]
    public bool TestBooleanOptionZ { get; set; }

    public void Print() =>
        throw new NotImplementedException();

    public void Run() =>
        throw new NotImplementedException();
}


