using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Tables
{
    public class TableColumnFullOutput : TableColumnOutput
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public string DftValue { get; set; }
        
        /// <summary>
        /// 键类型
        /// </summary>
        public int KeyType { get; set; }

        /// <summary>
        /// 列位置
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 数字精度
        /// </summary>
        public int? NumericPrecision { get; set; }

        /// <summary>
        /// 文本最大长度
        /// </summary>
        public long MaxLength { get; set; }

        /// <summary>
        /// 数值小数位
        /// </summary>
        public int NumSize { get; set; }

        /// <summary>
        /// 字段备注
        /// </summary>
        public string Remark { get; set; }

        ///// <summary>
        ///// 索引
        ///// </summary>
        //public string Index { get; set; }

        ///// <summary>
        ///// 是否自增
        ///// </summary>
        //public bool IsIncrement { get; set; }

        ///// <summary>
        ///// 增量开始
        ///// </summary>
        //public int? IncrementStart { get; set; }

        ///// <summary>
        ///// 增量种子
        ///// </summary>
        //public int? IncrementSeed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Detail { get; set; }
    }
}
