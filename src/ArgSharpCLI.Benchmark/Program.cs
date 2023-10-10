using ArgSharpCLI.Benchmark;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

var summary = BenchmarkRunner.Run<CommandBuilderBenchmark>();