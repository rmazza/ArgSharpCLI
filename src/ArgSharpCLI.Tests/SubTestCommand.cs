using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;

namespace ArgSharpCLI.Tests;

[Command("subcommand", Description = "Sub Test command.")]
public class SubTestCommand : ICommand
{
    public SubTestCommand() { } 
    
    public void Run()
    {
        
    }
}
