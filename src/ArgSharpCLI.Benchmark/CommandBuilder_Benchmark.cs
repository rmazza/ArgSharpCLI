using ArgSharpCLI.Tests;
using BenchmarkDotNet.Attributes;

namespace ArgSharpCLI.Benchmark;

[MemoryDiagnoser]
[InProcess]
public class CommandBuilder_Benchmark
{
    [ParamsSource(nameof(Args))]
    public string[]? _arguments;

    public List<string[]>? Args { get; } =
    new()
    {
        new string[] { "test", "-x" },
        new string[] { "test", "--test-option", "hello world", "-z"},
        new string[] { "test", "-xyz", "-t", "hello", "-u", "hello", "-v", "hello"}
    };

    [Benchmark]
    public void Build()
    {
        new CommandBuilder()
            .AddArguments(_arguments)
            .AddCommand<TestBenchmarkCommand>()
            .Build();
    }
}
