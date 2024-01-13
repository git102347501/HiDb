using HiDb.DataProvider.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Factory
{
    public static class ServiceFactory
    {
        public static IDataSorceDataPorvider? GetDataSource(DbTypeEnum dbType)
        {
            switch (dbType)
            {
                case DbTypeEnum.SqlServer:
                    return new SqlServer.DataSourceDataProvider();
                case DbTypeEnum.MySql:
                    return new MySql.DataSourceDataProvider();
                case DbTypeEnum.PgSql:
                default:
                    return null;
            }
        }

        public static IMenuDataProvider? GetMenu(DbTypeEnum dbType)
        {
            return dbType switch
            {
                DbTypeEnum.SqlServer => new SqlServer.MenuDataProvider(),
                DbTypeEnum.MySql => new MySql.MenuDataProvider(),
                _ => null
            };
        }

        public static ISearchDataProvider? GetSearch(DbTypeEnum dbType)
        {
            return dbType switch
            {
                DbTypeEnum.SqlServer => new SqlServer.SearchDataProvider(),
                DbTypeEnum.MySql => new MySql.SearchDataProvider(),
                _ => null
            };
        }

        public static ITableDataProvider? GetTable(DbTypeEnum dbType)
        {
            return dbType switch
            {
                DbTypeEnum.SqlServer => new SqlServer.TableDataProvider(),
                DbTypeEnum.MySql => new MySql.TableDataProvider(),
                _ => null
            };
        }
    }
}
