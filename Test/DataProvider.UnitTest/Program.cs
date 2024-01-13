// See https://aka.ms/new-console-template for more information
using DataProvider.UnitTest;
using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Connect;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Start DataProvider UT...");
try
{

    // ...
    var serviceCollection = new ServiceCollection();
    // 调用配置依赖注入服务的方法
    var startup = new Startup();
    startup.ConfigureServices(serviceCollection);

    var serviceProvider = serviceCollection.BuildServiceProvider();

    var mainService = serviceProvider.GetService<IDataSorceDataPorvider>();
    if (mainService == null)
    {
        throw new Exception("缺少服务[IDataSourceDataProvider]实现");
    }

    Console.WriteLine("1. Start Connect Test...");
    // 使用注入的服务
    var connRes = await mainService.ConnectDbAsync(new ConnectDbInput()
    {
        Account = "sa",
        Address = "127.0.0.1",
        PassWord = "Sql@123456",
        Port = 1433
    });

    if (connRes.Success)
    {
        Console.WriteLine("1. Connect Test Ok!");
    }
    else
    {
        Console.WriteLine("1. Connect Test Err:" + connRes.Message);
    }

    var menuService = serviceProvider.GetService<IMenuDataProvider>();
    if (menuService == null)
    {
        throw new Exception("缺少服务[IMenuDataProvider]实现");
    }

    Console.WriteLine("2. Start Get DbList Test...");
    var dbList = await menuService.GetDataBaseListAsync();
    Console.WriteLine("2. Get DbList:" + string.Join(",", dbList.Select(c => c.Name).ToList()));

    Console.WriteLine("2. Get DbList Ok!");

    Console.WriteLine("3. Get DbMode Test...");
    var database = dbList.FirstOrDefault()?.Name;
    if (database != null)
    {
        var modeList = await menuService.GetDbModeListAsync(database);
        Console.WriteLine("3. Get DbMode:" + string.Join(",", modeList.Select(c => c.Name).ToList()));
        Console.WriteLine("3. Get DbMode Ok!");

        Console.WriteLine("4. Get DbTable Test...");
        var name = dbList.FirstOrDefault()?.Name;
        if (name != null)
        {
            var s = modeList.FirstOrDefault()?.Name;
            if (s != null)
            {
                var tableList = await menuService.GetDbTableListAsync(s, name);
                Console.WriteLine("4. Get DbTable:" + string.Join(",", tableList.Select(c => c.Name).ToList()));
            }
        }
    }

    Console.WriteLine("4. Get DbTable Ok!");
}
catch (Exception ex)
{
    Console.WriteLine("Test Err:" + ex.Message);
}
Console.WriteLine("DataProvider UT Finish!");
Console.WriteLine("Exit");
