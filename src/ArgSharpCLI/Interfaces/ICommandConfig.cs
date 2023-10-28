using System;
using System.Collections.Generic;

namespace ArgSharpCLI.Interfaces;

public interface ICommandConfig
{
    ICommandConfig AddSubCommand<T2>();
    Dictionary<string, Type> GetSubCommands();
}
