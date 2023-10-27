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

## Usage

### Basic Usage

```csharp
using ArgSharpCLI;

// Define a simple command
[Command(Name = "test")]
public class TestCommand : ICommand
{
    // Implementation here
}

// In your Main method
var command = new CommandBuilder()
    .AddArguments(args)
    .AddCommand<TestCommand>()
    .Build();

// Execute the built command
command.Match(
    Success: cmd => cmd.Execute(),
    Failure: err => Console.WriteLine($"Error: {err}")
);

```

### Adding Custom Commands

You can easily add custom commands:

```csharp
var builder = new CommandBuilder()
    .AddArguments(args)
    .AddCommand<MyCustomCommand>();
```

### Help Support

`ArgSharpCLI` comes with built-in support for help commands. Just add `-h` or `--help` after your command:

```bash
$ cli ping -h
$ cli ping --help
```

## Contributing

We welcome contributions! Please submit PRs for any enhancements, bug fixes, or features you would like to add.

## License

MIT License. See [LICENSE](LICENSE) for details.
