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
        public async Task<SearchOutput> GetSearchDataAsync(SearchInput input, CancellationToken cancellationToken = default)
        {
            var query = input.noPage ? (input.Sql,"") : GetPageSql(input.Sql, input.PageSize.Value);
            if (!string.IsNullOrWhiteSpace(input.DataBase))
            {
                query.Item1 = @$"use [{input.DataBase}];
                               {query.Item1}";
                if (!string.IsNullOrWhiteSpace(query.Item2))
                {
                    query.Item2 = @$"use [{input.DataBase}];
                               {query.Item2}";
                }
            }

            var list = await GetListAsync(query.Item1, cancellationToken);
            var res = new SearchOutput()
            {
                List = list,
                Count = string.IsNullOrWhiteSpace(query.Item2) ? list.Count :
                    await GetCountAsync(query.Item2, cancellationToken)
            };
            return res;
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(SearchInput input, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(input.DataBase))
            {
                using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken);
                return await connection.ExecuteAsync(input.Sql);
            }
            else
            {
                using var connection = await SqlConnectionFactory.Get().CreateConnectionAsync(cancellationToken,
                    input.DataBase);
                return await connection.ExecuteAsync(@$"{input.Sql}");
            }
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
            var pageSql = $"SELECT top {pageSize} * FROM ({sql}) AS a";
            var countSql = $"SELECT count(1) FROM ({sql}) AS a";
            return (pageSql, countSql);
        }
    }
}
