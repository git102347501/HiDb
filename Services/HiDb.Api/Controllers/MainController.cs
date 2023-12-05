using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Enums;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers
{
    /// <summary>
    /// Base Main Controller
    /// </summary>
    public class MainController : ControllerBase
    {
        public T GetService<T>(Func<DbTypeEnum,T?> func)
        {
            var dbType = Request.Headers.FirstOrDefault(c => c.Key == "dbtype");
            var type = DbTypeHelper.GetDbType(dbType.Value.ToString());
            if (!type.HasValue)
            {
#if DEBUG
                type = DbTypeEnum.SqlServer;
#else
                throw new Exception("不支持的数据库类型");
#endif
            }
            var service = func(type.Value);
            if (service == null)
            {
                throw new Exception("改数据库类型不支持此类型操作");
            }
            return service;
        }
    }
}
