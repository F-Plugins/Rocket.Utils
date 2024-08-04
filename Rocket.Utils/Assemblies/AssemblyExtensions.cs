using System;
using System.IO;
using System.Reflection;

namespace Feli.Rocket.Utils.Assemblies;

public static class AssemblyExtensions
{
    public static string ReadManifestResourceAsString(this Assembly assembly, string name)
    {
        var resouces = assembly.GetManifestResourceNames();
        foreach (var resource in resouces)
        {
            if (!resource.EndsWith(name))
                continue;

            using var resourceStream = assembly.GetManifestResourceStream(resource);
            using var reader = new StreamReader(resourceStream);
            return reader.ReadToEnd();
        }

        throw new Exception($"The resource {name} was not found");
    }
}
