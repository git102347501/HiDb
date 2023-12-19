using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Dtos.Search;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing.Printing;

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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            sql = sql.Replace("\n", "");

            try
            {
                var res = GetService(ServiceFactory.GetSearch).GetSearchData(new SearchInput()
                {
                    DataBase = database,
                    Sql = sql,
                    PageSize = pageSize
                });
                stopwatch.Stop();
                res.ElapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                return res;
            }
            catch (Exception ex)
            {
                var res = new SearchOutput()
                {
                    Message = ex.Message,
                    Success = false
                };
                stopwatch.Stop();
                res.ElapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                return res;
            }
        }

        /// <summary>
        /// Execute sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        [HttpPost("execute")]
        public ExecuteOutput Execute(string sql, string? database = "")
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var count = GetService(ServiceFactory.GetSearch).Execute(new SearchInput()
                {
                    DataBase = database,
                    Sql = sql
                });
                stopwatch.Stop();
                return new ExecuteOutput()
                {
                    ChangeCount = count,
                    ElapsedTime = stopwatch.Elapsed.TotalMilliseconds,
                };
            }
            catch (Exception ex)
            {
                var res = new ExecuteOutput()
                {
                    Message = ex.Message,
                    Success = false
                };
                stopwatch.Stop();
                res.ElapsedTime = stopwatch.Elapsed.TotalMilliseconds;
                return res;
            }
        }
    }
}
