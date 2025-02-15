using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Unity.Injection;
using E_Commerce.Models;
using E_Commerce.Contract.AuthContract;
using E_Commerce.Contract;
using NLog;
using NLog.Web;
using Unity;
using System;

public static class UnityConfig
{
    private static IUnityContainer _container;

    public static void RegisterComponents()
    {
        if (_container == null)
        {
            _container = new UnityContainer();
        }

        // serilog --> fail
        //    if (Log.Logger == null || Log.Logger.GetType().ToString() == "Serilog.Core.LoggerConfiguration")
        //    {
        //        Log.Logger = new LoggerConfiguration()
        //.Enrich.FromLogContext()
        //.Enrich.WithThreadId()
        //.MinimumLevel.Verbose()
        //.WriteTo.File("Logs/log-.txt",
        //    rollingInterval: RollingInterval.Day,
        //    flushToDiskInterval: TimeSpan.FromSeconds(1))
        //.CreateLogger();

        //    }

        //_container.RegisterInstance<ILogger>(Log.Logger);

        var logger = LogManager.GetCurrentClassLogger();

        // ? Register NLog as a Singleton
        _container.RegisterInstance<ILogger>(logger);
        _container.RegisterType<AppDbContext>(new HierarchicalLifetimeManager());

        _container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<AppDbContext>()));

        _container.RegisterType<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>(new HierarchicalLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<AppDbContext>()));

        _container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<IUserStore<ApplicationUser>>()));

        _container.RegisterType<RoleManager<IdentityRole>>(new HierarchicalLifetimeManager(),
            new InjectionConstructor(new ResolvedParameter<IRoleStore<IdentityRole, string>>()));

        _container.RegisterType<IJwtProvider, JwtProvider>(new HierarchicalLifetimeManager(),
            new InjectionConstructor("R&hO6]0=::JC*Yeiww>eYB2[ql>]lFK@4FT>oFNWMy^|d`+Ds%2aZ^~vyF(1G(;",
                "E-Commerice", "E-Commerice Users", new ResolvedParameter<UserManager<ApplicationUser>>()));

        _container.RegisterType(typeof(IGenericRepo<>), typeof(GenericRepo<>));
        _container.RegisterType<ICustomerRepo, CustomerRepo>();
        _container.RegisterType<IOrderRepo, OrderRepo>();
        _container.RegisterType<ICustomersOrdersRepo, CustomersOrdersRepo>();

        GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(_container);
    }

    public static IUnityContainer GetConfiguredContainer()
    {
        return _container;
    }
}
