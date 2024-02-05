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
        public async Task<List<MenuDataBaseOutput>> GetDataBaseListAsync(string? name = "", bool searchTable = false,
            CancellationToken cancellationToken = default)
        {
            var res = await GetListAsync<MySqlDataBaseList>("SHOW DATABASES;", cancellationToken);
            return !string.IsNullOrWhiteSpace(name) ? 
                res.Where(c=> c.Database.Contains(name)).Select(c => new MenuDataBaseOutput() { Name = c.Database }).ToList() : 
                res.Select(c => new MenuDataBaseOutput() { Name = c.Database }).ToList();
        }

        public async Task<List<MenuDbTableOutput>> GetDbTableListAsync(string database,
            int pageSize, int pageIndex, string mode = "", CancellationToken cancellationToken = default)
        {
            var offset = pageSize * pageIndex;
            var res = await GetListAsync<dynamic>($@"SHOW TABLES FROM `{database}`", cancellationToken);

            return res.Skip(offset).Take(pageSize).Select(c => new MenuDbTableOutput() { 
                Name = ((IDictionary<string, object>)c)[$"Tables_in_{database}"].ToString() ?? "" }).ToList();
        }

        /// <summary>
        /// MySQL does not support data mode and returns master by default
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public async Task<List<MenuDbModeOutput>> GetDbModeListAsync(string database,
            CancellationToken cancellationToken = default)
        {
            return new List<MenuDbModeOutput>() { new MenuDbModeOutput() { Name = "master" } };
        }

        public async Task<List<MenuDbViewOutput>> GetDbViewListAsync(string database, string mode = "",
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuDbSpOutput>> GetDbSpListAsync(string database, string mode = "",
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}