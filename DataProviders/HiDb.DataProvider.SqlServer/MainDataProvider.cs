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
    }
}
