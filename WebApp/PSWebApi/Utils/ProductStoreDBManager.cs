using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using WebApp.Models;

namespace PSWebApi.Utils
{
    // Этот класс будешь наследовать от интерфейса и реализовывать все методы соответственно
    public class ProductStoreDBManager
    {
        public string SqliteConnectionString { get; private set; }
        SqliteConnection dbConnection { get; set; }

        public ProductStoreDBManager(string connectionStringName)
        {
            SqliteConnectionString = "Data Source=\"" + FileSystemManager.GetPathByConnectionStringName(connectionStringName) + "\";Mode=ReadWrite";
            dbConnection = new SqliteConnection(SqliteConnectionString);
        }

        public void OpenConnection() => dbConnection.Open();
        public void Close() => dbConnection.Close();
        public List<Product> Test()
        {
            string queryStr = "SELECT ID, name, price FROM Product"; 
            var productList = new List<Product>();

            SqliteCommand getAllProductsQuery = new SqliteCommand(queryStr, dbConnection);
            try
            {
                var resultReader = getAllProductsQuery.ExecuteReader();

                while (resultReader.Read())
                    productList.Add(
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

            return productList;
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