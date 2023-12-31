﻿using ArgSharpCLI.Interfaces;
using ArgSharpCLI.Tests;
using BenchmarkDotNet.Attributes;

namespace ArgSharpCLI.Benchmark;

[MemoryDiagnoser]
[InProcess]
public class CommandBuilder_BuildBenchmark
{
    private ICommandBuilder? _commandToRun;

    [ParamsSource(nameof(Args))]
    public required string[] _arguments;

    public List<string[]>? Args { get; } =
    new()
    {
        new string[] { "test", "-x" },
        new string[] { "test", "--test-option", "hello world", "-z"},
        new string[] { "test", "-xyz", "-t", "hello", "-u", "hello", "-v", "hello"}
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
