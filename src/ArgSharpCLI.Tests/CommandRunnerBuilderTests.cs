using ArgSharpCLI.Interfaces;
using LanguageExt;
using LanguageExt.Pipes;

namespace ArgSharpCLI.Tests;

public class CommandRunnerBuilderTests
{
    private readonly string[] _args = { "test" };

    [Fact]
    public void Build_WithPingArgument_ReturnsPingCommandInstance()
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
    public void Build_WithPingArgument_ReturnsPingCommand_RunThrowsNotImplementedException()
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
    public void Build_WithPingArgument_ReturnsPingCommand_PrintThrowsNotImplementedException()
    {
        var commandToRun = new CommandBuilder()
                .AddArguments(_args)
                .AddCommand<TestCommand>()
                .Build();

        commandToRun.Match(
            Succ: command =>
            {
                Assert.Throws<NotImplementedException>(() => command.Print());
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
    public void Build_WithPingArgument_ReturnsPingCommand_WithLongOptionStringSet()
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
    [InlineData(new [] { "test", "--test-option" }, typeof(Exception))]
    [InlineData(new [] { "test", "--test-option", " " }, typeof(Exception))]
    [InlineData(new[] { "test", "--test-option", "--test-boolean-option" }, typeof(Exception))]
    public void Build_WithPingArgument_ReturnsPingCommand_WithLongOptionStringSet_WithNoValue(string[] args, Type exceptionType)
    {
        Assert.Throws(exceptionType, () => new CommandBuilder()
                .AddArguments(args)
                .AddCommand<TestCommand>()
                .Build());
    }

    [Fact]
    public void Build_WithPingArgument_ReturnsPingCommand_WithLongOptionBoolSet()
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
                    Assert.Equal(expectedOptionValue, testCommand.TestBooleanOption);
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
    public void Build_WithPingArgument_ReturnsPingCommand_WithLongOptionStringAndBoolSet(string[] args, object expectedValue1, object expectedValue2)
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
                    Assert.Equal(expectedValue1, testCommand.TestBooleanOption);
                    Assert.Equal(expectedValue2, testCommand.TestOption);
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
}