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
        public async Task<IEnumerable<MenuDataBaseOutput>> GetMenu(string? name = null)
        {
            return await GetService(ServiceFactory.GetMenu).GetDataBaseListAsync(name);
        }

        [HttpGet("mode")]
        public async Task<IEnumerable<MenuDbModeOutput>> GetModeByDb(string database)
        {
            return await GetService(ServiceFactory.GetMenu).GetDbModeListAsync(database);
        }

        [HttpGet("table")]
        public async Task<IEnumerable<MenuDbTableOutput>> GetModeByDb(string database, string mode)
        {
            return await GetService(ServiceFactory.GetMenu).GetDbTableListAsync(database, mode);
        }
    }
}
