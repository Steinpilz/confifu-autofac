using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Confifu.Abstractions;
using Confifu.Abstractions.DependencyInjection;
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
                    return typeof(global::Autofac.IContainer).Assembly;
                }
                return null;
            };
        }
    }
}