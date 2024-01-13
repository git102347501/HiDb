using HiDb.DataProvider.Dtos.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.DataProvider
{
    public interface ISearchDataProvider
    {
        /// <summary>
        /// 获取查询结果
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<SearchOutput> GetSearchDataAsync(SearchInput input, CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(SearchInput input, CancellationToken cancellationToken = default);
    }
}
