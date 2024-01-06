using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Tables
{
    public class TableColumnOutput
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据库字段类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否允许Null
        /// </summary>
        public string AllowNullStr { get; set; }
        
        /// <summary>
        /// 是否允许Null
        /// </summary>
        public bool AllowNull => this.AllowNullStr == "YES";
    }
}
