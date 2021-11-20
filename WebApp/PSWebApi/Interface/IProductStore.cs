using System.Collections.Generic;
using WebApp.Models;

namespace PSWebApi.Interface
{
	public interface IProductStore
	{
		Product GetProductById(int id);

		void AddProduct(Product product);

		void DeleteProduct(int id);

		List<Product> FilterProductStore(string subString);

		bool ContainsProduct(string subString);
	}
}
