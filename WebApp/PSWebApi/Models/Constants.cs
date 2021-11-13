using System;
using System.Collections.Generic;
using WebApp.Models;

namespace PSWebApi.Models
{
	public class Constants
	{
		public static readonly string[] EXCEL_COLUMNS = { "A", "B", "C" };
		public enum Heading { Id, Name, Price };
		public static Dictionary<Heading, string> HEADERNAME_COLUMNNAME = new Dictionary<Heading, string>(3);
		public static List<Product> PRODUCT_LIST = new List<Product>();

		public static readonly string CURRENT_DATE = DateTime.Now.ToString("dd-MM-yyyy");

		public static readonly string RELATIVE_LOG_PATH = "\\Logs\\";
	}
}