using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArgSharpCLI.Commands;

//public class HelpCommandBase : ICommand
//{
//    public virtual string PrintCommandHelp(string command, IReadOnlyList<ICommandOption> options)
//    {
//        var sb = new StringBuilder();

//        sb.AppendLine($"Usage: {command} [options]");
//        sb.AppendLine();

//        if (options.Any())
//        {
//            sb.AppendLine("Options:");

//            foreach (ICommandOption option in options)
//            {
//                sb.AppendLine($"  {option.ShortOption}, {option.LongOption}     {option.Description}");
//            }
//        }

//        return sb.ToString();
//    }
//}

