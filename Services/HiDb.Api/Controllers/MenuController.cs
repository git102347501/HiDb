using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("Menu")]
    public class MenuController(IMenuDataProvider menuDataProvider) : ControllerBase
    {
        private readonly IMenuDataProvider _menuDataProvider = menuDataProvider;

        [HttpGet("db")]
        public IEnumerable<MenuDataBaseOutput> GetMenu()
        {
            return _menuDataProvider.GetDataBaseList();
        }

        [HttpGet("mode")]
        public IEnumerable<MenuDbModeOutput> GetModeByDb(string database)
        {
            return _menuDataProvider.GetDbModeList(database);
        }

        [HttpGet("table")]
        public IEnumerable<MenuDbTableOutput> GetModeByDb(string database, string mode)
        {
            return _menuDataProvider.GetDbTableList(database, mode);
        }
    }
}
