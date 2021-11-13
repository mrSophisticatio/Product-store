using PSWebApi.Utils;
using System;
using System.Web.Http;
using WebApp.Utils;

namespace WebApp
{
	public static class WebApiConfig
	{
		public static ProductStore ProductStore;
		public static ProductStoreDBManager ProductStoreDBManager;

		public static void Register(HttpConfiguration config)
		{
			string excelFilename = FileSystemManager.GetPathByConnectionStringName("ProductStoreExcel");
			ProductStore = new ProductStore(excelFilename, 1);

			//Создадим объект для работы с БД и подготовим строку подключения к БД
			ProductStoreDBManager = new ProductStoreDBManager("ProductStoreSQLiteDB");
			//Создадим подключение к БД
			ProductStoreDBManager.OpenConnection();
			ProductStoreDBManager.Test();

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
