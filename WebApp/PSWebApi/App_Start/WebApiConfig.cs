using PSWebApi.Utils;
using System;
using System.Web.Http;
using WebApp.Utils;
using PSWebApi.Interface;

namespace WebApp
{
	public static class WebApiConfig
	{
		public static IProductStore ProductStore;

		public static void Register(HttpConfiguration config)
		{			
			try
			{
				//Создадим объект для работы с БД и подготовим строку подключения к БД
				ProductStore = new ProductStoreDBManager("ProductStoreSQLiteDB");
			}
			catch
			{
				string excelFilename = FileSystemManager.GetPathByConnectionStringName("ProductStoreExcel");
				ProductStore = new ProductStoreExcel(excelFilename, 1);
			}

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
