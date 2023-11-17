using Dapper;
using HiDb.DataProvider.Dtos.Connect;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.SqlServer
{
    /// <summary>
    /// 数据库连接实现
    /// </summary>
    public class DataSorceDataPorvider : IDataSorceDataPorvider
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
            return ConnectDb(GeneratorDataSource(input));
        }

        /// <summary>
        /// 拼接MySQL连接字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GeneratorDataSource(ConnectDbInput input)
        {
            var connectionString = new StringBuilder();
            connectionString.Append("Server=");
            connectionString.Append(input.Address);
            //connectionString.Append(";Database=YourDatabaseName;");
            connectionString.Append(";Uid=");
            connectionString.Append(input.Account);
            connectionString.Append(";Pwd=");
            connectionString.Append(input.PassWord);
            connectionString.Append(";");

            return connectionString.ToString();
        }
    }
}
