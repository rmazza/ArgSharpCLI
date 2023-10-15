using ArgSharpCLI.Extensions;
using ArgSharpCLI.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ArgSharpCLI.Commands;

public class HelpCommand : ICommand
{
    private readonly ICommand _innerCommand;

    public HelpCommand(ICommand innerCommand)
    {
        _innerCommand = innerCommand;
    }

    public void Run()
    {
        Debug.WriteLine(GetHelpText());
        Console.WriteLine(GetHelpText());
    }

    private string GetHelpText()
    {
        var sb = new StringBuilder();
        var cmd = _innerCommand.GetType().GetCommandAttribute();
        var options = _innerCommand.GetType().GetProperties().GetOptionProperties().OrderBy(prop => prop.Name);

        sb.AppendLine($"{nameof(cmd.Description)}:");
        sb.AppendLine($"  {cmd.Description}");
        sb.AppendLine();

        sb.AppendLine("Options:");

        var maxOptionLength = 0;

        foreach (var option in options)
        {
            var len = option.GetOptionAttribute().ToString().Length;
            if (len > maxOptionLength)
                maxOptionLength = len;
        }

        foreach (var option in options)
        {
            var optionAttr = option.GetOptionAttribute();
            var paddedOption = optionAttr.ToString().PadRight(maxOptionLength);
            sb.AppendLine($"  {paddedOption}      {optionAttr.Description}");
        }

        return sb.ToString();
    }
}
