using System;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class ValuesController : ApiController
	{
		public IHttpActionResult GetProducts()
		{
			try
			{
				var allProducts = WebApiConfig.ProductStore.FilterProductStore("");
				return Json(allProducts);
			}
			catch
			{
				return Json(new ActionResult{ Code = 500, Message = "Stacktrace error" });
			}
		}

		public IHttpActionResult GetProduct(int id)
		{
			try
			{
				Product product = ValueLib.productList.Find(p => p.Id == id);
				if (product == null)
				{
					return Json(new ActionResult { Code = 404, Message = "Not found!" });
				}
				return Json(product);
			}
			catch
			{
				return Json(new ActionResult { Code = 500, Message = "Stacktrace error" });
			}			
		}

		[HttpPost]//применяем атрибуты, чтобы система знала, с каким методом надо сопоставлять запрос, согласно условностям при наименовании методов
		public IHttpActionResult AddProduct([FromBody]Product product)
		{
			bool resultCheckContainsProduct = false;
			try
			{
				resultCheckContainsProduct = WebApiConfig.ProductStore.ContainsProduct(product.Name);
				if (resultCheckContainsProduct)
					return Json(new ActionResult{ Code = 400, Message = "Продукт с таким наименованием уже имеется в системе!" });
				WebApiConfig.ProductStore.AddProduct(product);
				//return Json(new ActionResult{ Code = 200, Message = "Success" });//+ вернуть все продукты
				var allProducts = WebApiConfig.ProductStore.FilterProductStore("");
				return Ok(allProducts);
			}
			//catch (Exception ex)
			//{
			//	return Json(new { Code = 500, Message = ex.StackTrace });
			//}
			catch
			{
				return Json(new ActionResult{ Code = 500, Message = "Stacktrace error" });
			}
		}

		public IHttpActionResult DeleteProduct(int id)
		{
			try
			{
				WebApiConfig.ProductStore.DeleteProduct(id);
				return Json(new ActionResult { Code = 200, Message = "Success" });
			}
			catch
			{
				return Json(new ActionResult { Code = 500, Message = "Stacktrace error" });
			}
		}

		[HttpGet]
		public IHttpActionResult FilterProductStore(string subString)
		{
			try
			{
				var selectedProduct = WebApiConfig.ProductStore.FilterProductStore(subString);
				//var result = (selectedProduct, Json(new ActionResult { Code = 200, Message = "Success" }));
				return Ok(selectedProduct);
			}
			catch
			{
				return InternalServerError();
			}
		}
	}
}