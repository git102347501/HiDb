using Dapper;

namespace HiDb.DataProvider.MySql
{
    public class MainDataProvider
    {
        public List<T> GetList<T>(string sql, string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<T>();
            }
            var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.Query<T>(sql);
            return result.ToList();
        }

        public long GetCount(string sql, string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return 0;
            }
            var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.QueryFirstOrDefault<long>(sql);
            return result;
        }

        public List<dynamic> GetList(string sql, string database = "")
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new List<dynamic>();
            }
            using var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.Query<dynamic>(sql);
            connection.Close();
            return result.ToList();
        }

        public T GetFirst<T>(string sql, string database = "") where T : new ()
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                return new T();
            }
            using var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.QueryFirst<T>(sql);
            connection.Close();
            return result;
        }
    }
}
