using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using MySqlX.XDevAPI.Relational;
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
            return GetList<MenuDataBaseOutput>("SHOW DATABASES;");
        }
        public List<MenuDbTableOutput> GetDbTableList(string database, string mode)
        {
            return GetList<MenuDbTableOutput>(@$"SELECT TABLE_NAME
                                              FROM INFORMATION_SCHEMA.TABLES
                                              WHERE TABLE_SCHEMA = '{database}'
                                              AND TABLE_TYPE = 'BASE TABLE';");
        }

        public List<MenuDbModeOutput> GetDbModeList(string database)
        {
            return GetList<MenuDbModeOutput>($@"SHOW TABLES FROM {database};");
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