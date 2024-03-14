using System;
using Autofac;
using System.Reflection;

namespace Confifu.Autofac
{

    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var name = new AssemblyName(args.Name);
                if (name.Name == "Autofac")
                {
                    return typeof(IContainer).Assembly;
                }
                return null;
            };
        }
    }
}