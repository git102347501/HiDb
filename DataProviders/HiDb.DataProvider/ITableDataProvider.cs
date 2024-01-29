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
        public Task<List<TableColumnOutput>> GetDbColumnListAsync(TableColumnInput input,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 获取指定属性-全量信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Task<TableColumnFullOutput> GetDbColumnFullInfoAsync(TableColumnFullInput input,
            CancellationToken cancellationToken = default);

        Task<List<TableDbTypeOutput>> GetDbTypeListAsync(CancellationToken cancellationToken = default);

        Task<bool> DeleteTableAsync(string database, string mode, string table,
            CancellationToken cancellationToken = default);

        Task<bool> ClearTableAsync(string database, string mode, string table,
            CancellationToken cancellationToken = default);


        Task<bool> UpdateColumnConfigAsync(UpdateTableColumnInput input,
            CancellationToken cancellationToken = default);


        Task<bool> AddColumnConfigAsync(AddTableColumnInput input,
            CancellationToken cancellationToken = default);


        Task<bool> DeleteColumnConfigAsync(DeleteTableColumnInput input,
            CancellationToken cancellationToken = default);
    }
}
