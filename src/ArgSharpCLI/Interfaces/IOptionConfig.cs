using System;
using System.Collections.Generic;

namespace ArgSharpCLI.Interfaces
{
    public interface IOptionConfig
    {
        Dictionary<Type, List<Type>> SubCommands { get; }
    }
}