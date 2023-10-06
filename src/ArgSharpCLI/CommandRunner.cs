//using ArgSharpCLI.Interfaces;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;

//namespace ArgSharpCLI;

//public class CommandRunner : ICommandRunner
//{
//    private readonly ILogger _logger;
//    private readonly IConfiguration _config;
    

//    public CommandRunner(IEnumerable<Type> commands)
//    {

//    }

//    public void Execute(string[] args)
//    {
//        // Help
//        if (!args.Any())
//        {
//            //PrintCliHelp(CommandFactory.commands, _config);
//        }


//        //ICommandBase cmd = CommandFactory.GetCommand(args[0]);

//        //if (cmd is null) return;

//        //cmd.Run(args);

//    }

//    public bool IsVersion(string firstArg)
//    {
//        return firstArg.ToLower().Equals("-v") || firstArg.ToLower().Equals("--version");
//    }


//    public void HandleException(Exception ex)
//    {
//        _logger.LogError(ex, ex.Message);
//    }

//    public static void PrintCliHelp(List<ICommandBase> commands, IConfiguration config)
//    {
//        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

//        var sb = new StringBuilder();

//        //sb.AppendLine($"{assemblyName} ({config.GetValue<string>("version")})");
//        sb.AppendLine($"Usage: {assemblyName} [command] [command-options]");
//        sb.AppendLine();

//        if (commands.Count > 0)
//        {
//            sb.AppendLine("Commands:");
//            foreach (ICommandBase command in commands)
//            {
//                sb.AppendLine($"  {command.Name}              {command.Description}");
//            }
//        }

//        sb.AppendLine();
//        sb.AppendLine($"Run '{assemblyName} [command] --help' for more information on a command.");
//        Console.WriteLine(sb.ToString());
//    }
//}