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
        SearchOutput GetSearchData(SearchInput input);

        int Execute(SearchInput input);
    }
}
