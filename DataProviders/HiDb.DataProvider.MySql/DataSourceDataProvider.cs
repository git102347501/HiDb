using Dapper;
using HiDb.DataProvider.Dtos.Connect;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.MySql
{
    /// <summary>
    /// 数据库连接实现
    /// </summary>
    public class DataSourceDataProvider : MainDataProvider, IDataSorceDataPorvider
    {
        public async Task<ConnectDbOutput> ConnectDbAsync(string dataSource, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                await SqlConnectionFactory.Get().InitAsync(dataSource, cancellationToken);
                return new ConnectDbOutput() { Success = true };
            }
            catch (Exception ex)
            {
                return new ConnectDbOutput() { Success = false, Message = ex.Message };
            }
        }

        public async Task<ConnectDbOutput> ConnectDbAsync(ConnectDbInput input, 
            CancellationToken cancellationToken = default)
        {
            return await ConnectDbAsync(SqlConnectionFactory.GeneratorDataSource(input), cancellationToken);
        }
    }
}
