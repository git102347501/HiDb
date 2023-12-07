using HiDb.DataProvider.Dtos.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider
{
    /// <summary>
    /// 数据库表操作抽象
    /// </summary>
    public interface ITableDataProvider
    {
        /// <summary>
        /// 获取指定表-属性列表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public List<TableColumnOutput> GetDbColumnList(TableColumnInput input);

        /// <summary>
        /// 获取指定属性-全量信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TableColumnFullOutput GetDbColumnFullInfo(TableColumnFullInput input);
    }
}
