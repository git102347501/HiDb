using Dapper;
using HiDb.DataProvider.Dtos.Connect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.SqlServer
{
    /// <summary>
    /// 数据库连接实现
    /// </summary>
    public class DataSorceDataPorvider : MainDataProvider, IDataSorceDataPorvider
    {
        public ConnectDbOutput ConnectDb(string dataSource)
        {
            try
            {
                var res = SqlConnectionFactory.Get().Init(dataSource);
                return new ConnectDbOutput() { Success = res };
            }
            catch (Exception ex)
            {
                return new ConnectDbOutput() { Success = false, Message = ex.Message };
            }
        }

        public ConnectDbOutput ConnectDb(ConnectDbInput input)
        {
            return ConnectDb(SqlConnectionFactory.GeneratorDataSource(input));
        }
    }
}
