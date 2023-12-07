using Dapper;

namespace HiDb.DataProvider.MySql
{
    public class MainDataProvider
    {
        public List<T> GetList<T>(string sql, string database = "")
        {
            var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.Query<T>(sql);
            return result.ToList();
        }

        public long GetCount(string sql, string database = "")
        {
            var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.QueryFirstOrDefault<long>(sql);
            return result;
        }

        public List<dynamic> GetList(string sql, string database = "")
        {
            var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.Query<dynamic>(sql);
            return result.ToList();
        }

        public T GetFirst<T>(string sql, string database = "")
        {
            var connection = SqlConnectionFactory.GetConnection(database);
            var result = connection.QueryFirst<T>(sql);
            return result;
        }
    }
}
