// See https://aka.ms/new-console-template for more information
using DataProvider.UnitTest;
using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Connect;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

Console.WriteLine("Start DataProvder UT...");
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
        throw new Exception("缺少服务[IDataSorceDataPorvider]实现");
    }

    Console.WriteLine("1. Start Connect Test...");
    // 使用注入的服务
    var connRes = mainService.ConnectDb(new ConnectDbInput()
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
    var dbList = menuService.GetDataBaseList();
    Console.WriteLine("2. Get DbList:" + string.Join(",", dbList.Select(c => c.Name).ToList()));

    Console.WriteLine("2. Get DbList Ok!");

    Console.WriteLine("3. Get DbMode Test...");
    var modeList = menuService.GetDbModeList(dbList.FirstOrDefault().Name);
    Console.WriteLine("3. Get DbMode:" + string.Join(",", modeList.Select(c => c.Name).ToList()));
    Console.WriteLine("3. Get DbMode Ok!");

    Console.WriteLine("4. Get DbTable Test...");
    var tableList = menuService.GetDbTableList(modeList.FirstOrDefault().Name, dbList.FirstOrDefault().Name);
    Console.WriteLine("4. Get DbTable:" + string.Join(",", tableList.Select(c => c.Name).ToList()));
    Console.WriteLine("4. Get DbTable Ok!");
}
catch (Exception ex)
{
    Console.WriteLine("Test Err:" + ex.Message);
}
Console.WriteLine("DataProvder UT Finish!");
Console.WriteLine("Exit");
