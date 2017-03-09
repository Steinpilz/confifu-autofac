using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Confifu.Abstractions;
using Confifu.Abstractions.DependencyInjection;

namespace Confifu.Autofac
{
    /// <summary>
    /// Class holding AppSetup extensions to help setup Autofac container
    /// </summary>
    public static class AppSetupExtensions
    {
        /// <summary>
        /// Use Autofac container as IServiceProvider in given <para>appSetup</para> phase.
        /// </summary>
        /// <param name="appConfig">IAppConfig instance</param>
        /// <param name="configAction">Autofac Containerbuilder custom configAction called before 
        /// building the container</param>
        /// <returns>built IContainer instance</returns>
        public static IContainer SetupAutofacContainer(this IAppConfig appConfig, Action<ContainerBuilder> configAction = null)
        {
            if (appConfig == null) throw new ArgumentNullException(nameof(appConfig));

            var builder = new ContainerBuilder();
            var serviceCollection = appConfig.GetServiceCollection();

            if (serviceCollection == null)
                throw new InvalidOperationException("Can't setup AutofacContainer: appConfig doesn't contain ServiceCollection");

            builder.Populate(serviceCollection);

            configAction?.Invoke(builder);

            var container = builder.Build();
            appConfig.SetServiceProvider(container.Resolve<IServiceProvider>());

            return container;
        }
    }
}