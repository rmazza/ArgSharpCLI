﻿using ArgSharpCLI.Attributes;
using ICommand = ArgSharpCLI.Interfaces.ICommand;

namespace ArgSharpCLI.Tests;

[Command("test")]
public class TestCommand : ICommand
{
    [Option("-test", "-t", "test option")]
    public string? TestOption { get; set; }

    public void Print() =>
        throw new NotImplementedException();

    public void Run() =>
        throw new NotImplementedException();
}


