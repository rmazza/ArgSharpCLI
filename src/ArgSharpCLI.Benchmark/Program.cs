using ArgSharpCLI.Benchmark;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<CommandBuilder_Benchmark>();