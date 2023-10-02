namespace ArgSharpCLI.Interfaces;

public interface ICommandBase
{
    string Name { get; }
    string Description { get; }
    void Run(string[] args);
}