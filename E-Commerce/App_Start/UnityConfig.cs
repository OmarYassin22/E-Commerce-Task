using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using System.Web.Http;
using E_Commerce.Contract.ReposContract;
using E_Commerce.Models;

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        // Register IRepo<Customer> to a concrete implementation
        container.RegisterType<IRepo<Customer>, Repo<Customer>>(new HierarchicalLifetimeManager());

        // Register other dependencies if needed

        // Set the Web API Dependency Resolver to Unity
        GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
    }
}
