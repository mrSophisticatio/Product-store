using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;
using MoreLinq;          //Этот фреймворк позволяет абстрагироваться от структуры конкретной базы данных и вести все операции с данными через модель.
using Excel = Microsoft.Office.Interop.Excel;
using PSWebApi.Models;

namespace WebApp.Utils
{
	public class ProductStore
	{
		/*поля и св-ва*/
		Excel.Worksheet ExcelSheet;
		int index;
		public int rowCount;

		public ProductStore(string filename)
		{
			new ProductStore(filename, 1);
		}
		public ProductStore(string filename, int worksheetNumber)
		{
			Excel.Application ExcelApp2 = new Excel.Application();
			Excel.Workbook xlWb = ExcelApp2.Workbooks.Open(filename); //открываем Excel файл
			ExcelApp2.Visible = true;
			ExcelSheet = xlWb.Sheets[worksheetNumber]; //первый лист в файле

			Init();
		}

		public void Init()
		{
			Constants.HEADERNAME_COLUMNNAME.Add(Constants.Heading.Id, Constants.EXCEL_COLUMNS[0]);
			Constants.HEADERNAME_COLUMNNAME.Add(Constants.Heading.Name, Constants.EXCEL_COLUMNS[1]);
			Constants.HEADERNAME_COLUMNNAME.Add(Constants.Heading.Price, Constants.EXCEL_COLUMNS[2]);
			rowCount = ExcelSheet.Cells[ExcelSheet.Rows.Count, Constants.HEADERNAME_COLUMNNAME[Constants.Heading.Id]].End[Excel.XlDirection.xlUp].Row;

			for (int i = 0; i < rowCount - 1; i++)
			{
				Constants.PRODUCT_LIST.Add(
											new Product
											{
												Id = Convert.ToInt32(ExcelSheet.Cells[i + 2, Constants.Heading.Id + 1].Value),
												Name = ExcelSheet.Cells[i + 2, Constants.Heading.Name + 1].Value,
												Price = Convert.ToInt32(ExcelSheet.Cells[i + 2, Constants.Heading.Price + 1].Value)
											}
										);
			}
			index = FindMaxId();
		}
		int FindMaxId()
		{
			int maxID = 0;
			Product product = Constants.PRODUCT_LIST.MaxBy(p => p.Id).First();
			maxID = product.Id;
			return maxID;
		}
		public void AddProduct(Product product)//параметр продукт
		{
			product.Id = ++index;
			rowCount++;
			ExcelSheet.Cells[rowCount, Constants.HEADERNAME_COLUMNNAME[Constants.Heading.Id]].Value = product.Id.ToString();
			ExcelSheet.Cells[rowCount, Constants.HEADERNAME_COLUMNNAME[Constants.Heading.Name]].Value = product.Name.ToString();
			ExcelSheet.Cells[rowCount, Constants.HEADERNAME_COLUMNNAME[Constants.Heading.Price]].Value = product.Price.ToString();
			Constants.PRODUCT_LIST.Add(new Product { Id = Convert.ToInt32(ExcelSheet.Cells[rowCount, 1].Value), Name = ExcelSheet.Cells[rowCount, 2].Value, Price = Convert.ToInt32(ExcelSheet.Cells[rowCount, 3].Value) });
		}
		public void DeleteProduct(int id)
		{
			int numberString = FindProductIndex(id);
			Excel.Range TempRange = ExcelSheet.get_Range(Constants.HEADERNAME_COLUMNNAME[Constants.Heading.Id] + numberString, Constants.HEADERNAME_COLUMNNAME[Constants.Heading.Price] + numberString);
			TempRange.EntireRow.Delete(Type.Missing);
			rowCount--;
			Constants.PRODUCT_LIST.RemoveAt(numberString - 2);
		}
		int FindProductIndex(int id) => Constants.PRODUCT_LIST.FindIndex(p => p.Id == id) + 2;
		public List<Product> FilterProductStore(string subString) => Constants.PRODUCT_LIST.Where(p => p.Name.Contains(subString)).ToList();
		public bool ContainsProduct(string subString) => Constants.PRODUCT_LIST.Exists(p => p.Name == subString);
		public Product GetProductById(int id) => Constants.PRODUCT_LIST.Find(p => p.Id == id);
	}
}