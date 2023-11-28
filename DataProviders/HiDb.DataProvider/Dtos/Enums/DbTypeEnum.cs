using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Enums
{
    public enum DbTypeEnum
    {
        SqlServer = 0,
        MySql = 1,
        PgSql = 2
    }

    public class DbTypeHelper
    {
        public static DbTypeEnum? GetDbType(string name)
        {
            if (name.Trim().ToLower() == "sqlserver")
            {
                return DbTypeEnum.SqlServer;
            } 
            else if (name.Trim().ToLower() == "mysql")
            {
                return DbTypeEnum.MySql;
            }
            else
            {
                return null;
            }
        }
    }
}
