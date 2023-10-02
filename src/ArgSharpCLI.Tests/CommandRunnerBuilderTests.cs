namespace ArgSharpCLI.Tests;

public class CommandRunnerBuilderTests
{
    private readonly string[] _args = { "test" };

    [Fact]
    public void Build_WithPingArgument_ReturnsPingCommandInstance()
    {
        var commandToRun = new CommandRunnerBuilder()
                .AddArguments(_args)
                .AddCommand<TestCommand>()
                .Build();

        Assert.IsType<TestCommand>(commandToRun);
    }

    [Fact]
    public void Build_WithPingArgument_ReturnsPingCommand_RunThrowsNotImplementedException()
    {
        var commandToRun = new CommandRunnerBuilder()
                .AddArguments(_args)
                .AddCommand<TestCommand>()
                .Build();

        Assert.Throws<NotImplementedException>(() => commandToRun.Run());
    }

    [Fact]
    public void Build_WithPingArgument_ReturnsPingCommand_PrintThrowsNotImplementedException()
    {
        var commandToRun = new CommandRunnerBuilder()
                .AddArguments(_args)
                .AddCommand<TestCommand>()
                .Build();

        Assert.Throws<NotImplementedException>(() => commandToRun.Print());
    }
}