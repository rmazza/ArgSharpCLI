using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace ArgSharpCLI
{
    public interface ICommandFactory
    {
        ICommand CreateCommand();
    }
    public class CommandFactory
    {
        private readonly string[] _cliArguments;

        public CommandFactory(string[] cliArguments)
        {
            _cliArguments = cliArguments;
        }

        //public ICommand CreateCommand()
        //{
        //    return _cliArguments switch
        //    {
                
        //    };
        //}
    }

    //public class  Command
    //{
    //    private readonly ICommand _command;

    //    public Command(ICommandFactory factory)
    //    {
    //        _command = factory.CreateCommand();
    //    }

    //    public void Execute()
    //    {
    //        _command.Run();
    //    }
    //}
}
