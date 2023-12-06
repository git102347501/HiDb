using HiDb.DataProvider.Dtos.Connect;
using MySql.Data.MySqlClient;
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
            connection = new MySqlConnection(connectionString ?? _connectionString);
            connection.Open();
        }
         
        public static IDbConnection GetConnection(string connectionString = "")
        {
            // 如果未初始化，初始化连接
            if (connection == null)
            {
                connection = new MySqlConnection(connectionString ?? _connectionString);
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    // 连接打开异常，抛出错误
                    throw ex;
                }
                return connection;
            }
            else
            {
                // 正常返回连接
#warning 这里考虑初始化连接的时候，获取到数据库的超时配置，比如说30000毫秒，则设定时间戳，在30000毫秒临近的时候，进行connection Open重连操作，以免连接超时查询异常
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
                connectionString.Append(input.Address + ";" + "Port=" + input.Port);
            }
            else
            {
                connectionString.Append(input.Address + ";" + "Port=3306");
            }
            if (!string.IsNullOrWhiteSpace(input.Database))
            {
                connectionString.Append(";Database=");
                connectionString.Append(input.Database);
            }
            connectionString.Append(";Uid=");
            connectionString.Append(input.Account);
            connectionString.Append(";Pwd=");
            connectionString.Append(input.PassWord);
            connectionString.Append(";");
            if (input.TrustCert.HasValue)
            {
                connectionString.Append($"SslMode={input.TrustCert.ToString()};");
            }
            if (input.Encrypt.HasValue)
            {
                connectionString.Append($"AllowPublicKeyRetrieval={input.Encrypt.ToString()};");
            }
            if (input.TrustedConnection.HasValue)
            {
                connectionString.Append($"TrustServerCertificate={input.TrustedConnection.ToString()};");
            }
            return connectionString.ToString();
        }
    }
}
