using ArgSharpCLI.Interfaces;
using ArgSharpCLI.Tests;
using BenchmarkDotNet.Attributes;

namespace ArgSharpCLI.Benchmark;

[MemoryDiagnoser]
[InProcessAttribute]
public class CommandBuilderBenchmark
{

    private ICommandBuilder? _commandToRun;

    [ParamsSource(nameof(Args))]
    public string[]? _arguments;

    public List<string[]>? Args { get; } = 
    new()
    {
        new string[] { "test", "-b" },
        new string[] { "test", "--test-option", "hello world", "-b"}
    };

    [GlobalSetup]
    public void GlobalSetup()
    {
        _commandToRun = new CommandBuilder()
        .AddArguments(_arguments)
        .AddCommand<TestBenchmarkCommand>();
    }

    [Benchmark]
    public void BenchmarkBuild()
    {
        _commandToRun.Build();
    }
}
