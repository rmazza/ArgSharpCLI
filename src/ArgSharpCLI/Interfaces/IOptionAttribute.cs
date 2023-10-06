namespace ArgSharpCLI.Interfaces
{
    public interface IOptionAttribute
    {
        string LongName { get; }
        string ShortName { get; }
        string Description { get; }
    }
}
