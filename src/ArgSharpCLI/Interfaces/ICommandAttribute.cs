namespace ArgSharpCLI.Interfaces;

public interface ICommandAttribute
{
    string Name { get; }
    string Description { get; set; }
}
