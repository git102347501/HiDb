using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Search
{
    public class SearchOutput
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<dynamic> List { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public long Count { get; set; }

        /// <summary>
        /// 耗时毫秒
        /// </summary>
        public double ElapsedTime { get; set; }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
