using System;

namespace ArgSharpCLI.Interfaces;

public interface ICommandRunner
{
    void Execute(string[] args);
    void HandleException(Exception ex);
}