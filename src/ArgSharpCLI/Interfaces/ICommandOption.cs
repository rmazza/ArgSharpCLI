using System;

namespace ArgSharpCLI.Interfaces;

public interface ICommandOption
{
    string ShortOption { get; }
    string LongOption { get; }
    string Description { get; }

    void Invoke(Action action);
}