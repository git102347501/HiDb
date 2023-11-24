using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Dtos.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.SqlServer
{
    public class SearchDataProvider : MainDataProvider, ISearchDataProvider
    {
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<dynamic> GetSearchData(SearchInput input)
        {
            var sql = input.PageSize.HasValue && input.PageIndex.HasValue ? GetPageSql(input.Sql, input.PageIndex.Value, input.PageSize.Value) : input.Sql;
            if (string.IsNullOrWhiteSpace(input.DataBase))
            {
                return this.GetList(sql);
            } 
            else
            {
                return this.GetList(@$"use {input.DataBase};" + sql);
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Execute(SearchInput input)
        {
            var connection = SqlConnectionFactory.GetConnection();
            if (string.IsNullOrWhiteSpace(input.DataBase))
            {
                return connection.Execute(input.Sql);
            }
            else
            {
                return connection.Execute(@$"use {input.DataBase};" + input.Sql);
            }
        }

        private string GetPageSql(string sql, int pageIndex, int pageSize)
        {
            // 计算需要跳过的记录数

            // 非查询不转分页
            if (!sql.ToLower().Contains("select"))
            {
                return sql;
            }

            int offset = (pageIndex - 1) * pageSize;
            // 构建分页查询SQL
            string pageSql = $"SELECT * FROM ({sql}) AS TempTable ORDER BY (SELECT NULL) OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            return pageSql;
        }
    }
}
