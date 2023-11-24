using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Dtos.Search;
using HiDb.DataProvider.SqlServer;
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
    [Route("Search")]
    public class SearchController(ISearchDataProvider searchDataProvider) : ControllerBase
    {
        private readonly ISearchDataProvider _searchDataProvider = searchDataProvider;

        /// <summary>
        /// search data
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        [HttpGet]
        public List<dynamic> Get(string sql, int? pageIndex = null, int? pageSize = null, string? database = "")
        {
            return _searchDataProvider.GetSearchData(new SearchInput()
            {
                DataBase = database,
                Sql = sql,
                PageIndex = pageIndex,
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
            return _searchDataProvider.Execute(new SearchInput()
            {
                DataBase = database,
                Sql = sql
            });
        }
    }
}
