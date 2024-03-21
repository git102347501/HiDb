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
        public async Task<List<MenuDataBaseOutput>> GetDataBaseListAsync(string? name = "", bool searchTable = false,
            CancellationToken cancellationToken = default)
        {
            var sql = "SELECT name AS Name FROM sys.databases";
            if (!searchTable && !string.IsNullOrWhiteSpace(name))
            {
                sql = @$"SELECT name AS Name FROM sys.databases where name like '%{name}%'";
            }
            var res = await GetListAsync<MenuDataBaseOutput>(sql, cancellationToken);
            if (searchTable)
            {
                var list = new List<MenuDataBaseOutput>();
                foreach (var db in res)
                {
                    try
                    {
                        // 可能会没权限访问
                        if (await GetDbTableCountAsync(db.Name, name, cancellationToken) > 0)
                        {
                            list.Add(db);
                        }
                    }
                    catch
                    {
                        // 如果产生异常跳过
                        continue;
                    }
                }

                return list;
            }

            return res;
        }
        
        private async Task<long> GetDbTableCountAsync(string database, string tableName, CancellationToken cancellationToken = default)
        {
            return await GetFirstAsync<long>(
                @$"SELECT count(1) AS Name
                            FROM [{database}].INFORMATION_SCHEMA.TABLES
                            WHERE TABLE_NAME like '%{tableName}%'",cancellationToken);
        }

        public async Task<List<MenuDbTableOutput>> GetDbTableListAsync(string database,
            int pageSize, int pageIndex, string mode, CancellationToken cancellationToken = default)
        {
            var offset = pageSize * pageIndex;
            return await GetListAsync<MenuDbTableOutput>(
                        @$"SELECT TABLE_NAME AS Name
                            FROM [{database}].INFORMATION_SCHEMA.TABLES
                            WHERE TABLE_SCHEMA = '{mode}'
                            ORDER BY TABLE_NAME
                            OFFSET {offset} ROWS
                            FETCH NEXT {pageSize} ROWS ONLY",cancellationToken);
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