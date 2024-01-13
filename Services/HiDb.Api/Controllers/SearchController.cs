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
        public async Task<SearchOutput> GetAsync(string sql, int? pageSize = null, string? database = "", bool noPage = true,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            sql = sql.Replace("\n", "");

            try
            {
                var res = await GetService(ServiceFactory.GetSearch).GetSearchDataAsync(new SearchInput()
                {
                    DataBase = database,
                    Sql = sql,
                    PageSize = pageSize,
                    noPage = noPage
                }, cancellationToken);
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
        public async Task<ExecuteOutput> ExecuteAsync(string sql, string? database = "",
            CancellationToken cancellationToken = default)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                var count = await GetService(ServiceFactory.GetSearch).ExecuteAsync(new SearchInput()
                {
                    DataBase = database,
                    Sql = sql
                }, cancellationToken);
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
