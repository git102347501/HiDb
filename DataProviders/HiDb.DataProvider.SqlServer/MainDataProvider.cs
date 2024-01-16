using Dapper;
using System.Linq;

namespace HiDb.DataProvider.SqlServer
{
    public class MainDataProvider
    {
        public async Task<List<T>> GetListAsync<T>(string sql, 
            CancellationToken cancellationToken = default, string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<T>();
            }
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            var result = await connection.QueryAsync<T>(sql);
            return result.ToList();
        }

        public async Task<long> GetCountAsync(string sql, CancellationToken cancellationToken = default,
            string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return 0;
            }
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            var result = await connection.QueryFirstOrDefaultAsync<long>(sql);
            return result;
        }

        public async Task<List<dynamic>> GetListAsync(string sql, CancellationToken cancellationToken = default,
            string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<dynamic>();
            }
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            var result = await connection.QueryAsync<dynamic>(sql);
            return result.ToList();
        }

        public async Task<(List<dynamic>, long)> GetListAsync(string sql, CancellationToken cancellationToken = default,
          string? database = "", int? pageSize = 100)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return (new List<dynamic>(), 0);
            }
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            var result = await connection.QueryAsync<dynamic>(sql);
            return (pageSize.HasValue ? result.Take(pageSize.Value).ToList() : result.ToList(), result.Count());
        }

        public async Task<T> GetFirstAsync<T>(string sql, CancellationToken cancellationToken = default,
            string database = "") where T : new()
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new T();
            }
            using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken, database);
            var result = await connection.QueryFirstAsync<T>(sql);
            return result;
        }
    }
}
