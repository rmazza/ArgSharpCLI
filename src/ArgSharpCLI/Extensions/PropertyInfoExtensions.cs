using ArgSharpCLI.Attributes;
using ArgSharpCLI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ArgSharpCLI.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static IOptionAttribute GetOptionAttribute(this PropertyInfo propertyInfo) =>
            // Todo: find better way to do this
            propertyInfo.GetCustomAttribute<OptionAttribute>();

        public static IEnumerable<PropertyInfo> GetOptionProperties(this PropertyInfo[] propertyInfos) =>
            propertyInfos.Where(property => Attribute.IsDefined(property, typeof(OptionAttribute)));
    }

    public static class TypeExtensions
    {
        public static ICommandAttribute GetCommandAttribute(this Type type) =>
            type.GetCustomAttribute<CommandAttribute>();
    }
}
