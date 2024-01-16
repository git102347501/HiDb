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
        private IDbConnection _connection = null;
        private static SqlConnectionFactory curr = new SqlConnectionFactory();
        private string _connectionString;

        private SqlConnectionFactory()
        {
        }

        public bool Close()
        {
            this._connectionString = "";
            curr = new SqlConnectionFactory();
            return true;
        }

        public async Task<bool> InitAsync(string conn, CancellationToken cancellationToken)
        {
            this._connectionString = conn;
            var res = await CreateConnectionAsync(cancellationToken);
            if (res is {State: ConnectionState.Open})
            {
                res.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static SqlConnectionFactory Get()
        {
            return curr;
        }
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default, 
            string? database = "", string connectionString = "")
        {
            var conn = "";
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                conn = _connectionString;
            }

            if (string.IsNullOrWhiteSpace(conn))
            {
                throw new Exception("连接已断开，请重新连接数据库");
            }
            if (!string.IsNullOrWhiteSpace(database))
            {
                conn = conn + "Database=" + database;
            }
            var connection = new SqlConnection(conn);
            try
            {
                await connection.OpenAsync(cancellationToken);
            }
            catch
            {
                throw new Exception("连接失败，请重新连接数据库");
            }

            return connection;
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
