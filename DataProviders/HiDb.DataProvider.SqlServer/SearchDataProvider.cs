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
            if (string.IsNullOrWhiteSpace(input.DataBase))
            {
                return this.GetList(input.Sql);
            } 
            else
            {
                return this.GetList(@$"use {input.DataBase};" + input.Sql);
            }
        }
    }
}
