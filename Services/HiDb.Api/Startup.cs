using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Connect;
using HiDb.DataProvider.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiDb.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 注册需要注入的服务
            services.AddTransient<IDataSorceDataPorvider, DataSorceDataPorvider>();
            services.AddTransient<IMenuDataProvider, MenuDataProvider>();
            services.AddTransient<ITableDataProvider, TableDataProvider>();
            services.AddTransient<ISearchDataProvider, SearchDataProvider>();
#if DEBUG
            SqlConnectionFactory.InitConnection(new ConnectDbInput()
            {
                Account = "sa",
                Address = "127.0.0.1",
                PassWord = "Sql@123456",
                Port = 1433
            });
#endif
        }
    }
}
