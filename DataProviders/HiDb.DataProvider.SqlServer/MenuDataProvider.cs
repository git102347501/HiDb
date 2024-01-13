using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using System.Data.SqlClient;

namespace HiDb.DataProvider.SqlServer
{
    /// <summary>
    /// MenuDataProvider
    /// </summary>
    public class MenuDataProvider : MainDataProvider, IMenuDataProvider
    {
        public async Task<List<MenuDataBaseOutput>> GetDataBaseListAsync(string? name = "", 
            CancellationToken cancellationToken = default)
        {
            var sql = "SELECT name AS Name FROM sys.databases";
            if (!string.IsNullOrWhiteSpace(name))
            {
                sql = @$"SELECT name AS Name FROM sys.databases where name like '%{name}%'";
            }
            return await GetListAsync<MenuDataBaseOutput>(sql, cancellationToken);
        }

        public async Task<List<MenuDbTableOutput>> GetDbTableListAsync(string database, string mode,
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync<MenuDbTableOutput>(@$"SELECT TABLE_NAME AS Name
                                                FROM [{database}].INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_SCHEMA = '{mode}'",cancellationToken);
        }

        public async Task<List<MenuDbModeOutput>> GetDbModeListAsync(string database, 
            CancellationToken cancellationToken = default)
        {
            return await GetListAsync<MenuDbModeOutput>(@$"USE [{database}]
                                                SELECT SCHEMA_NAME(schema_id) AS Name
                                                FROM sys.schemas;",cancellationToken);
        }

        public async Task<List<MenuDbViewOutput>> GetDbViewListAsync(string mode, string database,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuDbSpOutput>> GetDbSpListAsync(string mode, string database,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}