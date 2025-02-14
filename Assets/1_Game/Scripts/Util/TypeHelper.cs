using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _1_Game.Scripts.Util
{
    public static class TypeHelper
    {
        public static List<Type> GetTypesInFolder(string folderNamespace)
        {
            var types = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes().Where(t => t.Namespace != null && t.Namespace.StartsWith(folderNamespace)));
            }

            return types;
        }

        public static List<Type> GetTypesInFolder<T>(string folderNamespace)
        {
            var types = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (!string.IsNullOrEmpty(type.Namespace) && type.Namespace.StartsWith(folderNamespace))
                        {
                            if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract)
                            {
                                types.Add(type);
                            }
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Console.WriteLine($"Error loading types from assembly {assembly.FullName}: {ex}");
                }
            }
            return types;
        }

    }
}