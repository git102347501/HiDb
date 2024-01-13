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
       
        private IDbConnection _connection = null;
        private static SqlConnectionFactory curr = new SqlConnectionFactory();
        private string _connectionString;

        private SqlConnectionFactory()
        {
        }
        
        public static SqlConnectionFactory Get()
        {
            return curr;
        }
        
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default, 
            string database = "", string connectionString = "")
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
            
            try
            {
                var connection = new MySqlConnection(conn);
                await connection.OpenAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(database))
                {
                    var useDatabaseSql = $"USE {database};";
                    await connection.ExecuteAsync(useDatabaseSql);
                }

                return connection;
            }
            catch
            {
                throw new Exception("连接失败，请重新连接数据库");
            }
        }

        public async Task<bool> InitAsync(string conn, CancellationToken cancellationToken)
        {
            this._connectionString = conn;
            var res = await CreateConnectionAsync(cancellationToken);
            if (res is { State: ConnectionState.Open })
            {
                res.Close();
                return true;
            }
            else
            {
                return false;
            }
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
            return connectionString.ToString();
        }
    }
}
