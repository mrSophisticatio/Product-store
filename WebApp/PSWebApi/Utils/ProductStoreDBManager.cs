using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using WebApp.Models;
using PSWebApi.Models;
using System.Linq;
using PSWebApi.Interface;

namespace PSWebApi.Utils
{
    // Этот класс будешь наследовать от интерфейса и реализовывать все методы соответственно
    public class ProductStoreDBManager : IProductStore
	{
        public string SqliteConnectionString { get; private set; }
        SqliteConnection dbConnection { get; set; }

		public ProductStoreDBManager(string connectionStringName)
        {
            SqliteConnectionString = "Data Source=\"" + FileSystemManager.GetPathByConnectionStringName(connectionStringName) + "\";Mode=ReadWrite";
            //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            dbConnection = new SqliteConnection(SqliteConnectionString);
			dbConnection.Open();
			Unit();
		}

        //public void OpenConnection() => dbConnection.Open();
        public void Close() => dbConnection.Close();

        public void Unit()
        {
            string queryStr = "SELECT * FROM Product"; 
            SqliteCommand getAllProductsQuery = new SqliteCommand(queryStr, dbConnection);
            try
            {
				var resultReader = getAllProductsQuery.ExecuteReader();
				while (resultReader.Read())
                    Constants.PRODUCT_LIST.Add(
                                        new Product
                                        {
                                            Id = resultReader.GetInt32(0),
                                            Name = resultReader.GetString(1),
                                            Price = resultReader.GetInt32(2)
                                        }
                                    );
            }
            catch (Exception ex)
            {
                LoggerFile.Error(ex.StackTrace);
            }
        }

		public Product GetProductById(int id)
		{
			string queryStrGet = $"SELECT * FROM Product WHERE ID = {id}";
			SqliteCommand getProductQuery = new SqliteCommand(queryStrGet, dbConnection);
			var resultReader = getProductQuery.ExecuteReader();
			Product product = new Product { Id = resultReader.GetInt32(0), Name = resultReader.GetString(1), Price = resultReader.GetInt32(2) };
			return product;
		}

		public void AddProduct(Product product)//параметр продукт
		{
			string queryStrAdd = $"INSERT INTO Product (name, price) VALUES ('{product.Name}', {product.Price})";
			SqliteCommand addProductQuery = new SqliteCommand(queryStrAdd, dbConnection);
			addProductQuery.ExecuteNonQuery();     
		}

		public void DeleteProduct(int id)
		{
			string queryStr = $"DELETE FROM Product WHERE ID = {id}";
			SqliteCommand deleteProductQuery = new SqliteCommand(queryStr, dbConnection);
			deleteProductQuery.ExecuteNonQuery();
		}

		public List<Product> FilterProductStore(string subString)
		{
			string queryStrGet = $"SELECT * FROM Product WHERE name like '%{subString}%'";
			SqliteCommand getProductQuery = new SqliteCommand(queryStrGet, dbConnection);
			var resultReader = getProductQuery.ExecuteReader();
			var selectedProduct = new List<Product>();
			while (resultReader.Read())
				selectedProduct.Add(
									new Product
									{
										Id = resultReader.GetInt32(0),
										Name = resultReader.GetString(1),
										Price = resultReader.GetInt32(2)
									}
								);
			return selectedProduct;
		}

		public bool ContainsProduct(string subString)
		{
			string queryStrGet = $"SELECT * FROM Product WHERE name = '{subString}'";
			SqliteCommand getProductQuery = new SqliteCommand(queryStrGet, dbConnection);
			var resultReader = getProductQuery.ExecuteReader();
			return resultReader.HasRows;
		}

		/*
         * Крч тут реализуешь необходимые методы.
         * 
         * Важные знание о текущей таблице Product, вот ее структура:
         *  CREATE TABLE "Product" (
         *  	"ID"	INTEGER NOT NULL,
         *  	"name"	TEXT NOT NULL UNIQUE,
         *  	"price"	INTEGER NOT NULL DEFAULT 0,
         *  	PRIMARY KEY("ID" AUTOINCREMENT)
         *  );
         *  
         *  AUTOINCREMENT означает, что при создании записи (INSERT or UPDATE) ID у нас заполняется сам.
         *  Даже если мы что-то удалим оттуда, то SQLite (как и др. СУБД) сами рассчитают какое след значение нужно для них.
         *  Следовательно, тебе при создании/обновлении записи ни коим образом не нужно пытаться передавать значение ID :)
         */

	}
}