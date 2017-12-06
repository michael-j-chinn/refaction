using refactor_me.Helpers;
using refactor_me.Repositories;
using System.Web.Http;
using Unity;
using Unity.Lifetime;

namespace refactor_me
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			var formatters = GlobalConfiguration.Configuration.Formatters;
			formatters.Remove(formatters.XmlFormatter);
			formatters.JsonFormatter.Indent = true;

			config.Formatters.JsonFormatter
						.SerializerSettings
						.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			// IoC Container
			var container = new UnityContainer();
			container.RegisterType<IProductRepository, ProductRepository>(new HierarchicalLifetimeManager());
			container.RegisterType<IProductOptionsRepository, ProductOptionsRepository>(new HierarchicalLifetimeManager());
			config.DependencyResolver = new UnityResolver(container);
		}
	}
}
