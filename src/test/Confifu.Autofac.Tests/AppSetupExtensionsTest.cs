using Confifu.Abstractions.DependencyInjection;
using System;
using Xunit;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollection = Confifu.Abstractions.DependencyInjection.ServiceCollection;


namespace Confifu.Autofac.Tests
{
    public class AppSetupExtensionsTest
    {
        static AppSetupExtensionsTest()
        {
            ModuleInitializer.Initialize();

        }

        [Fact]
        public void it_throws_exception_if_there_is_no_service_collection()
        {
            var appConfig = new AppConfig();

            Assert.Throws<InvalidOperationException>(() =>
            {
                appConfig.SetupAutofacContainer();
            });   
        }

#if NET8_0_OR_GREATER
        [Fact]
        public void it_supports_keyed_service_registration_for_net8()
        {
            var appConfig = new AppConfig();
            var sc = new ServiceCollection();
            sc.AddKeyedTransient<IServiceA, ServiceA>("test-key");
            appConfig.SetServiceCollection(sc); 
            appConfig.SetupAutofacContainer();
            var sp = appConfig.GetServiceProvider();
            var keyedServiceA = sp.GetKeyedService<IServiceA>("test-key");
            keyedServiceA.ShouldNotBeNull();
            keyedServiceA.AName().ShouldBe(new ServiceA().AName());
        }
#endif

        [Fact]
        public void it_creates_valid_service_provider()
        {
            var appConfig = new AppConfig();

            var sc = new ServiceCollection();

            sc.AddTransient<IServiceA, ServiceA>();
            sc.AddTransient<IServiceB, ServiceB>();

            appConfig.SetServiceCollection(sc);

            appConfig.SetupAutofacContainer();

            var sp = appConfig.GetServiceProvider();

            var a = sp.GetService<IServiceA>();

            a.ShouldNotBeNull();
            a.AName().ShouldBe(new ServiceA().AName());

            var b = sp.GetService<IServiceB>();
            b.ShouldNotBeNull();

            b.BName().ShouldBe(new ServiceB(new ServiceA()).BName());
        }
    }
    public interface IServiceA {
        string AName();
    }

    public interface IServiceB
    {
        string BName();
    }

    public class ServiceA : IServiceA
    {
        public string AName() => "ServiceA";
    }

    public class ServiceB : IServiceB
    {
        private IServiceA serviceA;

        public ServiceB(IServiceA serviceA)
        {
            this.serviceA = serviceA;
        }

        public string BName() => $"{serviceA.AName()}ServiceB";
    }
}
