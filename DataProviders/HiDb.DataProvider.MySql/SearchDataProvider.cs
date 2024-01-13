using Dapper;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Dtos.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider.MySql
{
    public class SearchDataProvider : MainDataProvider, ISearchDataProvider
    {
        /// <summary>
        /// 获取查询数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SearchOutput GetSearchData(SearchInput input)
        {
            var query = input.noPage ? (input.Sql, "") : GetPageSql(input.Sql, input.PageSize.Value);

            var res = new SearchOutput()
            {
                List = this.GetList(query.Item1, input.DataBase),
                Count = this.GetCount(query.Item2, input.DataBase)
            };
            return res;
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int Execute(SearchInput input)
        {
            var connection = SqlConnectionFactory.GetConnection(input.DataBase);
            return connection.Execute(input.Sql, input.DataBase);
        }

        private (string,string) GetPageSql(string sql, int pageSize)
        {
            // 计算需要跳过的记录数

            // 非查询不转分页
            if (!sql.ToLower().Contains("select"))
            {
                return (sql,"");
            } 
            // 构建分页查询SQL
            var pageSql = $"{sql}  LIMIT {pageSize}";
            var countSql = $"SELECT count(1) FROM ({sql}) AS a";
            return (pageSql, countSql);
        }
    }
}
