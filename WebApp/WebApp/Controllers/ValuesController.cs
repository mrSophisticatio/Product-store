using System;
using System.Web.Http;
using WebApp.Models;
using System.Net.Http;

namespace WebApp.Controllers
{
	public class ValuesController : ApiController
	{
		public IHttpActionResult GetProducts()
		{
				var allProducts = WebApiConfig.ProductStore.FilterProductStore("");
				return Json(allProducts);	
		}

		public IHttpActionResult GetProduct(int id)
		{			
			Product product = ValueLib.productList.Find(p => p.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			return Json(product);			
		}

		[HttpPost]//применяем атрибуты, чтобы система знала, с каким методом надо сопоставлять запрос, согласно условностям при наименовании методов
		public IHttpActionResult AddProduct([FromBody]Product product)
		{
			bool resultCheckContainsProduct = false;			
			try
			{
				resultCheckContainsProduct = WebApiConfig.ProductStore.ContainsProduct(product.Name);
				if (resultCheckContainsProduct)
					return Json(new { Code = 400, Message = "Продукт с таким наименованием уже имеется в системе!" });
				WebApiConfig.ProductStore.AddProduct(product);
				return Json(new { Code = 200, Message = "Success" });												
			}			
			catch (Exception ex)
			{
				return Json(new { Code = 500, Message = ex.StackTrace });
			}
		}

		public IHttpActionResult DeleteProduct(int id)
		{			
			try
			{
				WebApiConfig.ProductStore.DeleteProduct(id);
				return Ok();
			}
			catch
			{
				return InternalServerError();
			}
		}

		[HttpGet]
		public IHttpActionResult FilterProductStore(string subString)
		{			
			try
			{
				var selectedProduct = WebApiConfig.ProductStore.FilterProductStore(subString);
				return Json(selectedProduct);
			}
			catch
			{
				return InternalServerError();
			}
		}
	}
}
