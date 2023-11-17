﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.SqlServer
{
    public class SqlConnectionFactory
    {
        private static SqlConnection connection = null;

        private static string _connectionString = "";

        private SqlConnectionFactory()
        {
            
        }

        public static void InitConnection(string connectionString)
        {
            connection = new SqlConnection(connectionString ?? _connectionString);
            connection.Open();
        }
         
        public static SqlConnection GetConnection(string connectionString = "")
        {
            // 如果未初始化，初始化连接
            if (connection == null)
            {
                connection = new SqlConnection(connectionString ?? _connectionString);
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
    }
}
