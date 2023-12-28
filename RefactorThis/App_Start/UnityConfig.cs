using Microsoft.Practices.Unity;
using refactor_me.DataAccessLibraray;
using refactor_me.DataAccessLibraray.Interface;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace refactor_me
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductOptionRepository, ProductOptionRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}