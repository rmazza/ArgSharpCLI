# ArgSharpCLI

![NuGet](https://img.shields.io/nuget/v/ArgSharpCLI)
[![Build Status](https://github.com/rmazza/ArgSharpCLI/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/rmazza/ArgSharpCLI/actions/workflows/dotnet.yml)

## Overview

`ArgSharpCLI` is a feature-rich, yet lightweight, command-line argument parser designed for C# applications. It's built with SOLID principles, making your CLI apps both easy to develop and maintain.

## Table of Contents

- [Features](#features)
- [Getting Started](#getting-started)
  - [Installation](#installation)
  - [Basic Usage](#basic-usage)
- [Advanced Usage](#advanced-usage)
  - [Adding Sub-Commands](#adding-sub-commands)
  - [Custom Global Help Command](#custom-global-help-command)
- [Documentation](#documentation)
- [Contributing](#contributing)
- [License](#license)

## Features

- 📦 Out-of-the-box functionality
- 🛠️ Easy command and sub-command registration
- 👁️ Support for short (`-h`) and long (`--help`) option names
- 📘 Built-in help features
- 🔌 Extensibility for complex scenarios
- 🌟 SOLID principles for high maintainability

## Getting Started

### Installation

```bash
dotnet add package ArgSharpCLI
```

## Basic Usage
The following example demonstrates adding a simple TestCommand class and executing it.

```csharp
using ArgSharpCLI;

// Define a simple command
[Command(Name = "test")]
public class TestCommand : ICommand
{
    [Option("test-option", "t", "test option")]
    public string? TestOption { get; set; }

    [Option("test-boolean-option", "b", "test boolean option")]
    public bool TestBooleanOption { get; set; }
}

// In your Main method
var command = new CommandBuilder()
    .AddArguments(args)
    .AddCommand<TestCommand>()
    .Build();

// Execute the built command
command.Match(
    Success: cmd => cmd.Run(),
    Failure: err => Console.WriteLine($"Error: {err}")
);
```

## Advanced Usage

You can organize your commands into sub-commands as shown below:

```csharp
using ArgSharpCLI;

// Define the main command
[Command(Name = "main")]
public class MainCommand : ICommand
{
    // Implementation here
}

// Define a sub-command
[Command(Name = "sub")]
public class SubCommand : ICommand
{
    // Implementation here
}

// In your Main method
var command = new CommandBuilder()
    .AddArguments(args)
    .AddCommand<MainCommand>(cmd => {
        cmd.AddSubCommand<SubCommand>();
    })
    .Build();

// Execute the command
command.Run();

```

## License

This project is licensed under the MIT License. 
Feel free to copy and paste this markdown into your README.md file, and adjust it as necessary to fit your project.
