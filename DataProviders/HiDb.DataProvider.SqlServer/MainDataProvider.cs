using Dapper;

namespace HiDb.DataProvider.SqlServer
{
    public class MainDataProvider
    {
        public List<T> GetList<T>(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<T>();
            }
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.Query<T>(sql);
            return result.ToList();
        }

        public long GetCount(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return 0;
            }
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.QueryFirstOrDefault<long>(sql);
            return result;
        }

        public List<dynamic> GetList(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<dynamic>();
            }
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.Query<dynamic>(sql);
            return result.ToList();
        }

        public T GetFirst<T>(string sql) where T : new()
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new T();
            }
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.QueryFirst<T>(sql);
            return result;
        }
    }
}
