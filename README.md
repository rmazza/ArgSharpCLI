# ArgSharpCLI

## Description

`ArgSharpCLI` is a lightweight and extensible command-line argument parser for C# applications. It makes building robust CLI apps easy and maintainable by embracing SOLID principles.

## Features

- Easy command registration with `CommandBuilder`
- Support for both short (`-h`) and long (`--help`) option names
- Adheres to SOLID principles for high maintainability
- Out-of-the-box support for help commands
- Extensible for complex use-cases

## Installation

You can install the package via NuGet (when available):

```bash
dotnet add package ArgSharpCLI
```

## Usage

### Basic Usage

```csharp
var commandToRun = new CommandBuilder()
    .AddArguments(args)
    .AddCommand<TestCommand>()
    .Build();

// Do something with commandToRun...
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
