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
				return Json(new ActionResult{ Code = 500, Message = ex.StackTrace });
			}
		}

		public IHttpActionResult GetProduct(int id)
		{
			try
			{
				Product product = Constants.PRODUCT_LIST.Find(p => p.Id == id);
				if (product == null)
				{
					return Json(new ActionResult { Code = 404, Message = "Not found!" });
				}
				return Json(product);
			}
			catch (Exception ex)
			{
				return Json(new ActionResult { Code = 500, Message = ex.StackTrace });
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
				var allProducts = WebApiConfig.ProductStore.FilterProductStore("");
				return Ok(allProducts);
			}
			catch (Exception ex)
			{
				return Json(new ActionResult{ Code = 500, Message = ex.StackTrace });
			}
		}

		public IHttpActionResult DeleteProduct(int id)
		{
			try
			{
				WebApiConfig.ProductStore.DeleteProduct(id);
				return Json(new ActionResult { Code = 200, Message = "Success" });
			}
			catch (Exception ex)
			{
				return Json(new ActionResult { Code = 500, Message = ex.StackTrace });
			}
		}

		[HttpGet]
		public IHttpActionResult FilterProducts(string subString)
		{
			try
			{
				var selectedProduct = WebApiConfig.ProductStore.FilterProductStore(subString);
				//var result = (selectedProduct, Json(new ActionResult { Code = 200, Message = "Success" }));
				return Ok(selectedProduct);
			}
			catch (Exception ex)
			{
				return Json(new ActionResult { Code = 500, Message = ex.StackTrace });
			}
		}
	}
}