using HiDb.DataProvider.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Factory
{
    public class ServiceFactory
    {
        public static IDataSorceDataPorvider? GetDataSource(DbTypeEnum dbType)
        {
            switch (dbType)
            {
                case DbTypeEnum.SqlServer:
                    return new SqlServer.DataSorceDataPorvider();
                case DbTypeEnum.MySql:
                    return null;
                default:
                    return null;
            }
        }

        public static IMenuDataProvider? GetMenu(DbTypeEnum dbType)
        {
            switch (dbType)
            {
                case DbTypeEnum.SqlServer:
                    return new SqlServer.MenuDataProvider();
                case DbTypeEnum.MySql:
                    return null;
                default:
                    return null;
            }
        }

        public static ISearchDataProvider? GetSearch(DbTypeEnum dbType)
        {
            switch (dbType)
            {
                case DbTypeEnum.SqlServer:
                    return new SqlServer.SearchDataProvider();
                case DbTypeEnum.MySql:
                    return null;
                default:
                    return null;
            }
        }

        public static ITableDataProvider? GetTable(DbTypeEnum dbType)
        {
            switch (dbType)
            {
                case DbTypeEnum.SqlServer:
                    return new SqlServer.TableDataProvider();
                case DbTypeEnum.MySql:
                    return null;
                default:
                    return null;
            }
        }
    }
}
