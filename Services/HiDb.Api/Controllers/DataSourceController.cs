using HiDb.DataProvider.Dtos.Connect;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("datasource")]
    public class DataSourceController : MainController
    {
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("connect")]
        public async Task<ConnectDbOutput> ConnectDbAsync(ConnectDbInput input, CancellationToken cancellationToken)
        {
            return await GetService(ServiceFactory.GetDataSource).ConnectDbAsync(input,cancellationToken);
        }
    }
}
