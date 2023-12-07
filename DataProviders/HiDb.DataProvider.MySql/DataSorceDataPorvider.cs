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
    public class DataSorceDataPorvider : MainDataProvider, IDataSorceDataPorvider
    {
        public ConnectDbOutput ConnectDb(string dataSource)
        {
            try
            {
                SqlConnectionFactory.InitConnection(dataSource);
                return new ConnectDbOutput() { Success = true };
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
