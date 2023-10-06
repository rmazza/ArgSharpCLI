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
}