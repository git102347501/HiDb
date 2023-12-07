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
        public List<MenuDataBaseOutput> GetDataBaseList()
        {
            return GetList<MenuDataBaseOutput>("SELECT name AS Name FROM sys.databases");
        }

        public List<MenuDbTableOutput> GetDbTableList(string database, string mode)
        {
            return GetList<MenuDbTableOutput>(@$"SELECT TABLE_NAME AS Name
                                                FROM [{database}].INFORMATION_SCHEMA.TABLES
                                                WHERE TABLE_SCHEMA = '{mode}'");
        }

        public List<MenuDbModeOutput> GetDbModeList(string database)
        {
            return GetList<MenuDbModeOutput>(@$"USE [{database}]
                                                SELECT SCHEMA_NAME(schema_id) AS Name
                                                FROM sys.schemas;");
        }

        public List<MenuDbViewOutput> GetDbViewList(string mode, string database)
        {
            throw new NotImplementedException();
        }

        public List<MenuDbSpOutput> GetDbSpList(string mode, string database)
        {
            throw new NotImplementedException();
        }
    }
}