using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Dtos.Search;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers
{
    /// <summary>
    /// Search
    /// </summary>
    /// <param name="searchDataProvider"></param>
    [ApiController]
    [AllowAnonymous]
    [Route("search")]
    public class SearchController : MainController
    {
        /// <summary>
        /// search data
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        [HttpGet]
        public SearchOutput Get(string sql, int? pageSize = null, string? database = "")
        {
            sql = sql.Replace("\n", "");
            return GetService(ServiceFactory.GetSearch).GetSearchData(new SearchInput()
            {
                DataBase = database,
                Sql = sql,
                PageSize = pageSize
            });
        }

        /// <summary>
        /// Execute sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        [HttpPost("Execute")]
        public int Execute(string sql, string? database = "")
        {
            return GetService(ServiceFactory.GetSearch).Execute(new SearchInput()
            {
                DataBase = database,
                Sql = sql
            });
        }
    }
}
