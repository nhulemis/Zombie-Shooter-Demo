using System;
using System.Collections.Generic;
using System.Linq;

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

    }
}