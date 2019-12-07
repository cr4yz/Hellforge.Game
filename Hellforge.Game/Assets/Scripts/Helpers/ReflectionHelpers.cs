using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

public static class ReflectionHelpers 
{
    public static object MagicallyCreateInstance(string className, Type baseType)
    {
        var assembly = Assembly.GetAssembly(baseType);

        var type = AppDomain.CurrentDomain.GetAllDerivedTypes(baseType)
            .FirstOrDefault(t => t.Name == className);

        if(type == null)
        {
            return null;
        }

        return Activator.CreateInstance(type);
    }

    //public static object MagicallyCreateInstance(string className)
    //{
    //    var assembly = Assembly.GetExecutingAssembly();

    //    var type = assembly.GetTypes()
    //        .FirstOrDefault(t => t.Name == className);

    //    if (type == null)
    //    {
    //        return null;
    //    }

    //    return Activator.CreateInstance(type);
    //}

    public static Type[] GetAllDerivedTypes(this AppDomain aAppDomain, Type aType)
    {
        var result = new List<Type>();
        var assemblies = aAppDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(aType))
                    result.Add(type);
            }
        }
        return result.ToArray();
    }

}
