using Dapper;
using HiDb.DataProvider.Dtos.Connect;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.MySql
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
         
        public static IDbConnection GetConnection(string database = "", string connectionString = "")
        {
            if (connection != null)
            {
                if (!string.IsNullOrWhiteSpace(database))
                {
                    var useDatabaseSql = $"USE `{database}`;";
                    connection.Execute(useDatabaseSql);
                }
                return connection;
            } 
            else
            {
                connection = new MySqlConnection(connectionString ?? _connectionString);
                if (!string.IsNullOrWhiteSpace(database))
                {
                    var useDatabaseSql = $"USE {database};";
                    connection.Execute(useDatabaseSql);
                }

                connection.Open();
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
                connectionString.Append(input.Address + ";" + $"Port={input.Port}");
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
            //if (input.TrustCert.HasValue)
            //{
            //    connectionString.Append($"SslMode={input.TrustCert.ToString()};");
            //}
            //if (input.Encrypt.HasValue)
            //{
            //    connectionString.Append($"AllowPublicKeyRetrieval={input.Encrypt.ToString()};");
            //}
            //if (input.TrustedConnection.HasValue)
            //{
            //    connectionString.Append($"TrustServerCertificate={input.TrustedConnection.ToString()};");
            //}
            return connectionString.ToString();
        }
    }
}
