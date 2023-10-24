using System.Collections.Generic;

namespace ArgSharpCLI.Interfaces;

public interface ICommand
{
    void Run();

    Dictionary<string, ICommand> SubCommands => new();
}

