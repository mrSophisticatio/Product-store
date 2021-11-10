using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Models;
using MoreLinq;          //Этот фреймворк позволяет абстрагироваться от структуры конкретной базы данных и вести все операции с данными через модель.
using Excel = Microsoft.Office.Interop.Excel;

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
			ValueLib.headerNameColumnName.Add(ValueLib.Heading.Id, ValueLib.mas[0]);
			ValueLib.headerNameColumnName.Add(ValueLib.Heading.Name, ValueLib.mas[1]);
			ValueLib.headerNameColumnName.Add(ValueLib.Heading.Price, ValueLib.mas[2]);
			rowCount = ExcelSheet.Cells[ExcelSheet.Rows.Count, ValueLib.headerNameColumnName[ValueLib.Heading.Id]].End[Excel.XlDirection.xlUp].Row;

			for (int i = 0; i < rowCount - 1; i++)
			{
				ValueLib.productList.Add(
											new Product
											{
												Id = Convert.ToInt32(ExcelSheet.Cells[i + 2, ValueLib.Heading.Id + 1].Value),
												Name = ExcelSheet.Cells[i + 2, ValueLib.Heading.Name + 1].Value,
												Price = Convert.ToInt32(ExcelSheet.Cells[i + 2, ValueLib.Heading.Price + 1].Value)
											}
										);
			}
			index = FindMaxId();
		}

		public void AddProduct(Product product)//параметр продукт
		{
			product.Id = ++index;
			rowCount++;
			ExcelSheet.Cells[rowCount, ValueLib.headerNameColumnName[ValueLib.Heading.Id]].Value = product.Id.ToString();
			ExcelSheet.Cells[rowCount, ValueLib.headerNameColumnName[ValueLib.Heading.Name]].Value = product.Name.ToString();
			ExcelSheet.Cells[rowCount, ValueLib.headerNameColumnName[ValueLib.Heading.Price]].Value = product.Price.ToString();
			ValueLib.productList.Add(new Product { Id = Convert.ToInt32(ExcelSheet.Cells[rowCount, 1].Value), Name = ExcelSheet.Cells[rowCount, 2].Value, Price = Convert.ToInt32(ExcelSheet.Cells[rowCount, 3].Value) });
		}

		public void DeleteProduct(int id)
		{
			int numberString = FindProductIndex(id);
			Excel.Range TempRange = ExcelSheet.get_Range(ValueLib.headerNameColumnName[ValueLib.Heading.Id] + numberString, ValueLib.headerNameColumnName[ValueLib.Heading.Price] + numberString);
			TempRange.EntireRow.Delete(Type.Missing);
			rowCount--;
			ValueLib.productList.RemoveAt(numberString - 2);
		}

		int FindMaxId()
		{
			int maxID = 0;
			Product product = ValueLib.productList.MaxBy(p => p.Id).First();
			maxID = product.Id;
			return maxID;
		}

		int FindProductIndex(int id)
		{
			int numberString = ValueLib.productList.FindIndex(p => p.Id == id) + 2;
			return numberString;
		}

		public List<Product> FilterProductStore(string subString)
		{
			var selectedProduct = ValueLib.productList.Where(p => p.Name.Contains(subString)).ToList();

			return selectedProduct;
		}

		public bool ContainsProduct(string subString)
		{
			return ValueLib.productList.Exists(p => p.Name == subString);
		}
	}
}