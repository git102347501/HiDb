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
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return (DbTypeEnum)int.Parse(name);
        }
    }
}
