using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using System.Data.SqlClient;

namespace HiDb.DataProvider.SqlServer
{
    public class MenuDataProvider : IMenuDataProvider
    {
        public List<MenuDataBaseOutput> GetDataBaseList()
        {
            var connection = SqlConnectionFactory.GetConnection();
            var sql = "SELECT name FROM sys.databases";
            var result = connection.Query<MenuDataBaseOutput>(sql);
            return result.ToList();
        }

        public List<MenuDbTableOutput> GetDbSpList()
        {
            throw new NotImplementedException();
        }

        public List<MenuDbTableOutput> GetDbTableList()
        {
            throw new NotImplementedException();
        }

        public List<MenuDbTableOutput> GetDbViewList()
        {
            throw new NotImplementedException();
        }
    }
}