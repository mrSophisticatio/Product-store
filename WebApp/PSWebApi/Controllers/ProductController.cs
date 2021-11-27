using PSWebApi.Models;
using System;
using System.Web.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
	public class ProductController : ApiController
	{
		public IHttpActionResult GetProducts()
		{
			try
			{
				var allProducts = WebApiConfig.ProductStore.FilterProductStore("");
				return Json(allProducts);
			}
			catch (Exception ex)
			{
				return Json(new ActionResult{ code = 500, message = ex.StackTrace });
			}
		}

		public IHttpActionResult GetProduct(int id)
		{
			try
			{
				Product product = WebApiConfig.ProductStore.GetProductById(id);
				if (product == null)
					return Json(new ActionResult { code = 404, message = "Not found!" });
				
				return Json(product);
			}
			catch (Exception ex)
			{
				return Json(new ActionResult { code = 500, message = ex.StackTrace });
			}
		}

		[HttpPost]//применяем атрибуты, чтобы система знала, с каким методом надо сопоставлять запрос, согласно условностям при наименовании методов
		public IHttpActionResult AddProduct([FromBody]Product product)
		{
			try
			{				
				bool resultCheck = product.Name == "" || product.Price <= 0;

				if (resultCheck)
					return Json(new ActionResult { code = 400, message = "Невалидные параметры наименования/стоимости!" });

				resultCheck = WebApiConfig.ProductStore.ContainsProduct(product.Name);
				if (resultCheck)
					return Json(new ActionResult{ code = 400, message = "Продукт с таким наименованием уже имеется в системе!" });

				WebApiConfig.ProductStore.AddProduct(product);
				return Ok(new ActionResult { code = 200, message = "Success" });
			}
			catch (Exception ex)
			{
				return Json(new ActionResult{ code = 500, message = ex.StackTrace });
			}
		}

		public IHttpActionResult DeleteProduct(int id)
		{
			try
			{
				WebApiConfig.ProductStore.DeleteProduct(id);
				return Ok(new ActionResult { code = 200, message = "Success" });
			}
			catch (Exception ex)
			{
				return Json(new ActionResult { code = 500, message = ex.StackTrace });
			}
		}

		[HttpGet]
		public IHttpActionResult FilterProducts(string subString)
		{
			try
			{
				subString = subString == null ? "" : subString;
				var selectedProduct = WebApiConfig.ProductStore.FilterProductStore(subString);
				return Ok(selectedProduct);
			}
			catch (Exception ex)
			{
				return Json(new ActionResult { code = 500, message = ex.StackTrace });
			}
		}		
	}
}