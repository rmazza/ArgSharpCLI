using System;

namespace ArgSharpCLI.Attributes
{
    public class OptionAttribute : Attribute
    {
        public string LongName { get; }
        public string ShortName { get; }
        public string Description { get; }

        public OptionAttribute(string longName, string shortName, string description)
        {
            LongName = longName;
            ShortName = shortName;
            Description = description;
        }
    }
}
