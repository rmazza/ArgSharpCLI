using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ArgSharpCLI.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static IOptionAttribute GetOptionAttribute(this PropertyInfo propertyInfo)
        {
            // Todo: find better way to do this
            return propertyInfo.GetCustomAttribute<OptionAttribute>();
        }
    }
}
