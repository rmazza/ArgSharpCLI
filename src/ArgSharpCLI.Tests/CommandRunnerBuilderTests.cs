using ArgSharpCLI.Commands;
using ArgSharpCLI.ExceptionHandling;
using ArgSharpCLI.Tests.Commands;
using ArgSharpCLI.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;

namespace ArgSharpCLI.Tests;

public class CommandRunnerBuilderTests
{
    [Theory]
    [InlineData(new[] { "test" }, typeof(TestCommand))]
    [InlineData(new[] { "second" }, typeof(SecondCommand))]
    public void Build_WithTestArgument_AddCommandWithTwoGenerics_ReturnsCommandInstance(string[] args, Type t)
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand, SecondCommand>()
                .Build();

        commandToRun.Match(
            onSuccess: command =>
            {
                Assert.IsType(t, command);
                return 1;
            },
            onFailure: ex => 1);
    }

    [Theory]
    [InlineData(new[] { "test" }, typeof(TestCommand))]
    [InlineData(new[] { "second" }, typeof(SecondCommand))]
    [InlineData(new[] { "third" }, typeof(ThirdCommand))]
    public void Build_WithTestArgument_AddCommandWithThreeGenerics_ReturnsCommandInstance(string[] args, Type t)
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand, SecondCommand, ThirdCommand>()
                .Build();

        commandToRun.Match(
                onSuccess: command =>
                {
                    Assert.IsType(t, command);
                    return 1;
                },
                onFailure: ex =>
                {
                    Assert.Fail(ex.Description);
                    return 1;
                }
            );
    }

    [Theory]
    [InlineData(new[] { "test" }, typeof(TestCommand))]
    [InlineData(new[] { "second" }, typeof(SecondCommand))]
    [InlineData(new[] { "third" }, typeof(ThirdCommand))]
    [InlineData(new[] { "fourth" }, typeof(FourthCommand))]
    public void Build_WithTestArgument_AddCommandWithFourGenerics_ReturnsCommandInstance(string[] args, Type t)
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand, SecondCommand, ThirdCommand, FourthCommand>()
                .Build();

        commandToRun.Match(
                onSuccess: command =>
                {
                    Assert.IsType(t, command);
                    return 1;
                },
                onFailure: ex =>
                {
                    Assert.Fail(ex.Description);
                    return 1;
                }
            );
    }


    [Fact]
    public void Build_WithTestArgument_ReturnsTestCommand_RunThrowsNotImplementedException()
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(new[] {"test"})
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
            onSuccess: command =>
            {
                Assert.Throws<NotImplementedException>(() => command.Run());
                return 1;
            },
            onFailure: ex =>
            {

                Assert.Fail(ex.Description);
                return 1;
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
            onSuccess: command =>
            {
                Assert.IsType<TestCommand>(command);
                if (command is TestCommand testCommand)
                {
                    Assert.Equal(expectedOptionValue, testCommand.TestOption);
                }
                return 1;
            },
            onFailure: ex =>
            {

                Assert.Fail(ex.Description);
                return 1;
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
            onSuccess: command =>
            {
                Assert.IsType<TestCommand>(command);
                if (command is TestCommand testCommand)
                {
                    Assert.Equal(expectedOptionValue, testCommand.TestBooleanOption1);
                }
                return 1;
            },
            onFailure: ex =>
            {

                Assert.Fail(ex.Description);
                return 1;
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
            onSuccess: command =>
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
                return 1;
            },
            onFailure: ex =>
            {

                Assert.Fail(ex.Description);
                return 1;
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
            onSuccess: command =>
            {
                Assert.IsType(typeReturned, command);
                command.Run();
                return 1;
            },
            onFailure: error => 1);
    }

    [Theory]
    [InlineData(new[] { "test", "-bz", "subcommand", "--help" }, typeof(HelpCommand))]
    [InlineData(new[] { "test", "subcommand", "--help" }, typeof(HelpCommand))]
    [InlineData(new[] { "test", "subcommand", "-h" }, typeof(HelpCommand))]
    public void Build_WithTestArgument_ReturnsSubHelpCommand(string[] args, Type typeReturned)
    {
        var commandToRun = new CommandBuilder()
        .AddArguments(args)
        .AddCommand<TestCommand>(config => 
            config.AddSubCommand<SubTestCommand>())
        .Build()
        .Match(
            onSuccess: command =>
            {
                Assert.IsType(typeReturned, command);
                command.Run();
                return 1;
            },
            onFailure: error => 1);
    }

    [Theory]
    [InlineData(new[] { "test", "-b", "subcommand" }, typeof(SubTestCommand))]
    [InlineData(new[] { "test", "subcommand" }, typeof(SubTestCommand))]
    public void Build_WithTestArgument_ReturnsSubCommand(string[] args, Type typeReturned)
    {
        var commandToRun = new CommandBuilder()
        .AddArguments(args)
        .AddCommand<TestCommand>(subCommandConfig => 
            subCommandConfig.AddSubCommand<SubTestCommand>())
        .Build()
        .Match(
            onSuccess: command =>
            {
                Assert.IsType(typeReturned, command);
                return 1;
            },
            onFailure: error => 1);
    }

    [Theory]
    [InlineData(new[] { "test", "-b", "subcommand" }, typeof(SubTestCommand))]
    [InlineData(new[] { "test", "subcommand" }, typeof(SubTestCommand))]
    [InlineData(new[] { "test", "-b" }, typeof(TestCommand))]
    [InlineData(new[] { "second" }, typeof(SecondCommand))]
    [InlineData(new[] { "third" }, typeof(ThirdCommand))]
    [InlineData(new[] { "fourth" }, typeof(FourthCommand))]
    public void Build_WithTestArgument_ReturnsCorrectCommand(string[] args, Type typeReturned)
    {
        var commandToRun = new CommandBuilder()
        .AddArguments(args)
        .AddCommand<TestCommand>(subCommandConfig =>
            subCommandConfig.AddSubCommand<SubTestCommand>())
        .AddCommand<SecondCommand>()
        .AddCommand<ThirdCommand, FourthCommand>()
        .Build()
        .Match(
            onSuccess: command =>
            {
                Assert.IsType(typeReturned, command);
                return 1;
            },
            onFailure: error => 1);
    }
}