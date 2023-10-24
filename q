[1mdiff --git a/src/ArgSharpCLI.Tests/CommandRunnerBuilderTests.cs b/src/ArgSharpCLI.Tests/CommandRunnerBuilderTests.cs[m
[1mindex 0892378..a3e313a 100644[m
[1m--- a/src/ArgSharpCLI.Tests/CommandRunnerBuilderTests.cs[m
[1m+++ b/src/ArgSharpCLI.Tests/CommandRunnerBuilderTests.cs[m
[36m@@ -201,6 +201,7 @@[m [mpublic class CommandRunnerBuilderTests[m
 [m
     [Theory][m
     [InlineData(new[] { "test", "-b", "subcommand" }, typeof(SubTestCommand))][m
[32m+[m[32m    [InlineData(new[] { "test", "subcommand" }, typeof(SubTestCommand))][m
     public void Build_WithTestArgument_ReturnsSubCommand(string[] args, Type typeReturned)[m
     {[m
         var commandToRun = new CommandBuilder()[m
[1mdiff --git a/src/ArgSharpCLI.Tests/SubTestCommand.cs b/src/ArgSharpCLI.Tests/SubTestCommand.cs[m
[1mindex 41bcc3b..25a228f 100644[m
[1m--- a/src/ArgSharpCLI.Tests/SubTestCommand.cs[m
[1m+++ b/src/ArgSharpCLI.Tests/SubTestCommand.cs[m
[36m@@ -6,8 +6,12 @@[m [mnamespace ArgSharpCLI.Tests;[m
 [Command("subcommand", Description = "Sub Test command.")][m
 public class SubTestCommand : ICommand[m
 {[m
[31m-    public SubTestCommand() { } [m
[31m-    [m
[32m+[m[32m    [Option("test-option", "t", "test option")][m
[32m+[m[32m    public string? TestOption { get; set; }[m
[32m+[m
[32m+[m[32m    [Option("test-boolean-option", "b", "test boolean option")][m
[32m+[m[32m    public bool TestBooleanOption1 { get; set; }[m
[32m+[m
     public void Run()[m
     {[m
         [m
[1mdiff --git a/src/ArgSharpCLI/CommandBuilder.cs b/src/ArgSharpCLI/CommandBuilder.cs[m
[1mindex 5905a5e..61ab2ab 100644[m
[1m--- a/src/ArgSharpCLI/CommandBuilder.cs[m
[1m+++ b/src/ArgSharpCLI/CommandBuilder.cs[m
[36m@@ -123,8 +123,9 @@[m [mpublic class CommandBuilder : ICommandBuilder[m
         if (_argumentQueue.Any())[m
         {[m
             command = GetCommandFromQueue(_argumentQueue, _subCommands[command.GetType()]);[m
[31m-            optionParser.SetCommand(command).BuildOptions();[m
[31m-[m
[32m+[m[32m            optionParser[m
[32m+[m[32m                .SetCommand(command)[m
[32m+[m[32m                .BuildOptions();[m
         }[m
 [m
         return new Result<ICommand>(command);[m
