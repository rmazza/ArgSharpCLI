using System;
using ArgSharpCLI.Interfaces;

namespace ArgSharpCLI.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : Attribute, IOptionAttribute
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

        public override string ToString()
        {
            return $"-{ShortName}|--{LongName}";
        }
    }
}
