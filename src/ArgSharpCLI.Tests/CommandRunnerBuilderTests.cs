using ArgSharpCLI.Commands;
using ArgSharpCLI.ExceptionHandling;
using LanguageExt;
using ArgSharpCLI.Extensions;

namespace ArgSharpCLI.Tests;

public class CommandRunnerBuilderTests
{
    private readonly string[] _args = { "test" };

    [Fact]
    public void Build_WithTestArgument_ReturnsTestCommandInstance()
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(_args)
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
                Succ: command =>
                {
                    Assert.IsType<TestCommand>(command);
                    return Unit.Default;
                },
                Fail: ex =>
                {
                    Assert.Fail(ex.Message);
                    return Unit.Default;
                }
            );
    }

    [Fact]
    public void Build_WithTestArgument_ReturnsTestCommand_RunThrowsNotImplementedException()
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(_args)
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
            Succ: command =>
            {
                Assert.Throws<NotImplementedException>(() => command.Run());
                return Unit.Default;
            },
            Fail: ex =>
            {

                Assert.Fail(ex.Message);
                return Unit.Default;
            }
        );
    }

    [Fact]
    public void Build_WithTestArgument_ReturnsTestCommand_WithLongOptionStringSet()
    {
        var expectedOptionValue = "test value";
        var args = new string[] { "test", "--test-option", expectedOptionValue };
        var commandToRun = new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
            Succ: command =>
            {
                Assert.IsType<TestCommand>(command);
                if (command is TestCommand testCommand)
                {
                    Assert.Equal(expectedOptionValue, testCommand.TestOption);
                }
                return Unit.Default;
            },
            Fail: ex =>
            {

                Assert.Fail(ex.Message);
                return Unit.Default;
            }
        );
    }

    [Theory]
    [InlineData(new[] { "test", "--test-option" }, typeof(Exception))]
    [InlineData(new[] { "test", "--test-option", " " }, typeof(Exception))]
    [InlineData(new[] { "test", "--test-option", "--test-boolean-option" }, typeof(Exception))]
    [InlineData(new[] { "test", "-bd" }, typeof(CommandNotFoundException))]
    [InlineData(new[] { "test", "-bt" }, typeof(InvalidCommandException))]
    [InlineData(new[] { "test", "-bt", "--test-boolean-option" }, typeof(InvalidCommandException))]
    [InlineData(new[] { "test", "-tb" }, typeof(InvalidCommandException))]
    [InlineData(new[] { "test", "-tb", "--test-option", "test command" }, typeof(InvalidCommandException))]

    public void Build_WithTestArgument_ReturnsTestCommand_WithNoValue(string[] args, Type exceptionType)
    {
        Assert.Throws(exceptionType, () => new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand>()
                .Build());
    }

    [Fact]
    public void Build_WithTestArgument_ReturnsTestCommand_WithLongOptionBoolSet()
    {
        bool expectedOptionValue = true;
        var args = new string[] { "test", "--test-boolean-option" };

        var commandToRun = new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
            Succ: command =>
            {
                Assert.IsType<TestCommand>(command);
                if (command is TestCommand testCommand)
                {
                    Assert.Equal(expectedOptionValue, testCommand.TestBooleanOption1);
                }
                return Unit.Default;
            },
            Fail: ex =>
            {

                Assert.Fail(ex.Message);
                return Unit.Default;
            }
        );
    }

    [Theory]
    [InlineData(new string[] { "test", "--test-boolean-option", "--test-option", "hello world" }, true, "hello world")]
    [InlineData(new string[] { "test", "--test-option", "hello world", "--test-boolean-option" }, true, "hello world")]
    [InlineData(new string[] { "test", "-b", "-t", "hello world" }, true, "hello world")]
    [InlineData(new string[] { "test", "-b", "--test-option", "hello world" }, true, "hello world")]
    [InlineData(new string[] { "test", "--test-option", "hello world", "-b" }, true, "hello world")]
    [InlineData(new string[] { "test", "--test-boolean-option", "-t", "hello world" }, true, "hello world")]
    [InlineData(new string[] { "test", "--test-boolean-option" }, true, null)]
    [InlineData(new string[] { "test", "-b" }, true, null)]
    [InlineData(new string[] { "test", "-t", "hello world" }, false, "hello world")]
    [InlineData(new string[] { "test", "-bz" }, true, null, true)]
    [InlineData(new string[] { "test", "-zb", "-t", "hello world" }, true, "hello world", true)]
    [InlineData(new string[] { "test", "-zb", "--test-option", "hello world" }, true, "hello world", true)]
    public void Build_WithTestArgument_ReturnsTestCommand_WithMixedOptionsSet(string[] args, object expectedBooleanValue, object expectedStringValue, object expectedBooleanOption2 = null)
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
            Succ: command =>
            {
                Assert.IsType<TestCommand>(command);
                if (command is TestCommand testCommand)
                {
                    Assert.Equal(expectedBooleanValue, testCommand.TestBooleanOption1);

                    if (expectedStringValue is not null)
                        Assert.Equal(expectedStringValue, testCommand.TestOption);

                    if (expectedBooleanOption2 is not null)
                        Assert.Equal(expectedBooleanOption2, testCommand.TestBooleanOption2);
                }
                return Unit.Default;
            },
            Fail: ex =>
            {

                Assert.Fail(ex.Message);
                return Unit.Default;
            }
        );
    }


    [Theory]
    [InlineData(new[] { "--help" }, typeof(GlobalHelpCommand))]
    [InlineData(new[] { "test", "--help" }, typeof(HelpCommand))]
    [InlineData(new[] { "test", "-h" }, typeof(HelpCommand))]
    [InlineData(new[] { "test", "-bz", "-h" }, typeof(HelpCommand))]
    [InlineData(new[] { "test", "-bz", "--help" }, typeof(HelpCommand))]
    public void Build_WithTestArgument_ReturnsHelpCommand(string[] args, Type typeReturned)
    {
        var commandToRun = new CommandBuilder()
        .AddArguments(args)
        .AddCommand<TestCommand>()
        .Build()
        .Match(
            Succ: command =>
            {
                Assert.IsType(typeReturned, command);
                command.Run();
                return Unit.Default;
            },
            Fail: error => Unit.Default);
    }

    [Theory]
    [InlineData(new[] { "test", "-b", "subcommand" }, typeof(SubTestCommand))]
    public void Build_WithTestArgument_ReturnsSubCommand(string[] args, Type typeReturned)
    {
        var commandToRun = new CommandBuilder()
        .AddArguments(args)
        .AddCommand<TestCommand>(subCommands => 
            subCommands.Add(typeof(SubTestCommand)))
        .Build()
        .Match(
            Succ: command =>
            {
                Assert.IsType(typeReturned, command);
                return Unit.Default;
            },
            Fail: error => Unit.Default);
    }
}