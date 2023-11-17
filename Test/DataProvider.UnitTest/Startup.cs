using HiDb.DataProvider;
using HiDb.DataProvider.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.UnitTest
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册需要注入的服务
            services.AddTransient<IDataSorceDataPorvider, DataSorceDataPorvider>();
            services.AddTransient<IMenuDataProvider, MenuDataProvider>();
        }
    }
}
