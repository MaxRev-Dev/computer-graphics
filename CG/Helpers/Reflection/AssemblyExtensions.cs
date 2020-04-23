using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Playground.Helpers.Reflection
{
    public class AssemblyExtensions
    {
        public static IEnumerable<T> FindAllImplementationsAndActivate<T>()
        {
            var interf = typeof(T);
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => !x.IsAbstract &&
                            x.GetConstructor(Type.EmptyTypes) != default &&
                            x.GetInterfaces().Any(v => v == interf))
                .Select(Activator.CreateInstance).Cast<T>().ToArray();
        }
    }
}