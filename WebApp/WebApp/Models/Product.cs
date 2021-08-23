using System.Collections.Generic;

namespace WebApp.Models
{
	static class ValueLib
	{
		public static readonly string[] mas = { "A", "B", "C" };
		public static int id;
		public enum Heading { Id, Name, Price };
		public static Dictionary<Heading, string> headerNameColumnName = new Dictionary<Heading, string>(3);
		public static List<Product> productList = new List<Product>();
	}
	
	public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
	}	
}