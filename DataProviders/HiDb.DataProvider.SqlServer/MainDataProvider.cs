using Dapper;

namespace HiDb.DataProvider.SqlServer
{
    public class MainDataProvider
    {
        public List<T> GetList<T>(string sql, string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<T>();
            }
            using var connection = SqlConnectionFactory.Get().CreateConnection(database);
            var result = connection.Query<T>(sql).ToList();
            connection.Close();
            connection.Dispose();
            return result;
        }

        public long GetCount(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return 0;
            }
            using var connection = SqlConnectionFactory.Get().CreateConnection();
            var result = connection.QueryFirstOrDefault<long>(sql);
            connection.Close();
            connection.Dispose();
            return result;
        }

        public List<dynamic> GetList(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<dynamic>();
            }
            using var connection = SqlConnectionFactory.Get().CreateConnection();
            var result = connection.Query<dynamic>(sql);
            connection.Close();
            connection.Dispose();
            return result.ToList();
        }

        public T GetFirst<T>(string sql) where T : new()
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new T();
            }
            using var connection = SqlConnectionFactory.Get().CreateConnection();
            var result = connection.QueryFirst<T>(sql);
            connection.Close();
            connection.Dispose();
            return result;
        }
    }
}
