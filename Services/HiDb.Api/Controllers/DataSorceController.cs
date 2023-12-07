using HiDb.DataProvider.Dtos.Connect;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("datasorce")]
    public class DataSorceController : MainController
    {
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("connect")]
        public ConnectDbOutput ConnectDb(ConnectDbInput input)
        {
            return GetService(ServiceFactory.GetDataSource).ConnectDb(input);
        }
    }
}
