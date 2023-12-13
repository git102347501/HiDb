using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.MySql.Models;
using MySqlX.XDevAPI.Relational;
using System.Data.SqlClient;

namespace HiDb.DataProvider.MySql
{
    /// <summary>
    /// MenuDataProvider
    /// </summary>
    public class MenuDataProvider : MainDataProvider, IMenuDataProvider
    {
        public List<MenuDataBaseOutput> GetDataBaseList(string? name = "")
        {
            var res = GetList<MySqlDataBaseList>("SHOW DATABASES;");
            if (!string.IsNullOrWhiteSpace(name))
            {
                return res.Where(c=> c.Database.Contains(name)).Select(c => new MenuDataBaseOutput() { Name = c.Database }).ToList();
            }
            return res.Select(c => new MenuDataBaseOutput() { Name = c.Database }).ToList();
        }

        public List<MenuDbTableOutput> GetDbTableList(string database, string mode = "")
        {
            var res = GetList<dynamic>($@"SHOW TABLES FROM `{database}`;");

            return res.Select(c => new MenuDbTableOutput() { 
                Name = ((IDictionary<string, object>)c)[$"Tables_in_{database}"].ToString() ?? "" }).ToList();
        }

        /// <summary>
        /// MySQL does not support data mode and returns master by default
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public List<MenuDbModeOutput> GetDbModeList(string database)
        {
            return new List<MenuDbModeOutput>() { new MenuDbModeOutput() { Name = "master" } };
        }

        public List<MenuDbViewOutput> GetDbViewList(string database, string mode = "")
        {
            throw new NotImplementedException();
        }

        public List<MenuDbSpOutput> GetDbSpList(string database, string mode = "")
        {
            throw new NotImplementedException();
        }
    }
}