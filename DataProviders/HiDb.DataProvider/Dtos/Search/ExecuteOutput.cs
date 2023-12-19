using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.Dtos.Search
{
    public class ExecuteOutput
    {
        /// <summary>
        /// 毫秒
        /// </summary>
        public int ChangeCount { get; set; }

        /// <summary>
        /// 耗时毫秒
        /// </summary>
        public double ElapsedTime { get; set; }


        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
