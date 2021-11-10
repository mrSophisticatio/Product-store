using System;
using System.Web.Http;
using WebApp.Utils;

namespace WebApp
{
	public static class WebApiConfig
	{
		public static ProductStore ProductStore;

		public static void Register(HttpConfiguration config)
		{
			string filename = System.IO.Path.GetFullPath("TestVeb.xlsx");
			ProductStore = new ProductStore(filename, 1);
			// Конфигурация и службы веб-API

			// Маршруты веб-API
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
