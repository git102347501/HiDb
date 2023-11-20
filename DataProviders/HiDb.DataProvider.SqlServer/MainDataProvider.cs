using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.SqlServer
{
    public class MainDataProvider
    {
        public List<T> GetList<T>(string sql)
        {
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.Query<T>(sql);
            return result.ToList();
        }

        public List<dynamic> GetList(string sql)
        {
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.Query<dynamic>(sql);
            return result.ToList();
        }

        public T GetFirst<T>(string sql)
        {
            var connection = SqlConnectionFactory.GetConnection();
            var result = connection.QueryFirst<T>(sql);
            return result;
        }
    }
}
