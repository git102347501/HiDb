using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Search
{
    public class SearchInput
    {
        /// <summary>
        /// 当前数据库
        /// </summary>
        public string? DataBase { get; set; }

        /// <summary>
        /// 查询语句
        /// </summary>
        public string Sql { get; set; }

        public bool noPage { get; set; }

        public int? PageSize { get; set; }
    }
}
