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
    [Route("Menu")]
    public class MenuController : MainController
    {

        [HttpGet("db")]
        public IEnumerable<MenuDataBaseOutput> GetMenu()
        {
            return GetService(ServiceFactory.GetMenu).GetDataBaseList();
        }

        [HttpGet("mode")]
        public IEnumerable<MenuDbModeOutput> GetModeByDb(string database)
        {
            return GetService(ServiceFactory.GetMenu).GetDbModeList(database);
        }

        [HttpGet("table")]
        public IEnumerable<MenuDbTableOutput> GetModeByDb(string database, string mode)
        {
            return GetService(ServiceFactory.GetMenu).GetDbTableList(database, mode);
        }
    }
}
