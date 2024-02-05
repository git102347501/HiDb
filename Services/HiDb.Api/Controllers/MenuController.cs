using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Enums;
using HiDb.DataProvider.Dtos.Menus;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace HiDb.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("menu")]
    public class MenuController : MainController
    {

        [HttpGet("db")]
        public async Task<IEnumerable<MenuDataBaseOutput>> GetMenu(string? name = null, bool searchTable = false,
            CancellationToken cancellationToken = default)
        {
            return await GetService(ServiceFactory.GetMenu).GetDataBaseListAsync(name, searchTable, cancellationToken);
        }

        [HttpGet("mode")]
        public async Task<IEnumerable<MenuDbModeOutput>> GetModeByDb(string database, CancellationToken cancellationToken = default)
        {
            return await GetService(ServiceFactory.GetMenu).GetDbModeListAsync(database, cancellationToken);
        }

        [HttpGet("table")]
        public async Task<IEnumerable<MenuDbTableOutput>> GetModeByDb(string database,
            int pageSize, int pageIndex, string mode = "", CancellationToken cancellationToken = default)
        {
            return await GetService(ServiceFactory.GetMenu).GetDbTableListAsync(database,
                pageSize, pageIndex, mode, cancellationToken);
        }
    }
}
