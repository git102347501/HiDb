using HiDb.DataProvider.Dtos.Connect;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.SqlServer
{
    public class SqlConnectionFactory
    {
        private static IDbConnection connection = null;
        private static string _connectionString = "";

        private SqlConnectionFactory()
        {
            
        }

        public static void InitConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _connectionString = connectionString;
            }
            connection = new SqlConnection(connectionString ?? _connectionString);
            connection.Open();
        }
         
        public static IDbConnection GetConnection(string connectionString = "")
        {
            // 如果未初始化，初始化连接
            if (connection is not {State: ConnectionState.Open})
            {
                InitConnection(_connectionString);
                return connection;
            }
            else
            {
                return connection;
            }
        }

        public static void InitConnection(ConnectDbInput input)
        {
            InitConnection(GeneratorDataSource(input));
        }

        public static IDbConnection GetConnection(ConnectDbInput input)
        {
            return GetConnection(GeneratorDataSource(input));
        }

        /// <summary>
        /// 拼接MySQL连接字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GeneratorDataSource(ConnectDbInput input)
        {
            var connectionString = new StringBuilder();
            connectionString.Append("Server=");
            if (input.Port > 0)
            {
                connectionString.Append(input.Address + "," + input.Port);
            } 
            else
            {
                connectionString.Append(input.Address + ",1433");
            }
            //connectionString.Append(";Database=master");
            connectionString.Append(";Uid=");
            connectionString.Append(input.Account);
            connectionString.Append(";Pwd=");
            connectionString.Append(input.PassWord);
            connectionString.Append(";");
            if (input.TrustCert.HasValue)
            {
                connectionString.Append($"TrustServerCertificate={input.TrustCert.ToString()};");
            }
            if (input.Encrypt.HasValue)
            {
                connectionString.Append($"Encrypt={input.Encrypt.ToString()};");
            }
            if (input.TrustedConnection.HasValue)
            {
                connectionString.Append($"Trusted_Connection={input.TrustedConnection.ToString()};");
            }
            return connectionString.ToString();
        }
    }
}
