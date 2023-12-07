using DataProvider.UnitTest;
using HiDb.DataProvider;
using HiDb.DataProvider.Dtos.Menus;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

var app = builder.Build();

var serviceCollection = new ServiceCollection();
// 调用配置依赖注入服务的方法
var startup = new Startup();
startup.ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();
var _menuDataProvider = serviceProvider.GetService<IMenuDataProvider>();

var todosApi = app.MapGroup("/menu");
todosApi.MapGet("/", () =>
{
    if (_menuDataProvider != null)
    {
        var res = _menuDataProvider.GetDataBaseList();
        Results.Ok(res);
    }
    else
    {
        Results.NotFound();
    }
});

todosApi.MapGet("/mode/{database}", (string database) =>
{
    if (_menuDataProvider != null)
    {
        var res = _menuDataProvider.GetDbModeList(database);
        Results.Ok(res);
    }
    else
    {
        Results.NotFound();
    }
});

app.Run();

public record Todo(int Id, string? Title, DateOnly? DueBy = null, bool IsComplete = false);

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}

[JsonSerializable(typeof(MenuDataBaseOutput[]))]
internal partial class MenuDataBaseOutputSerializerContext : JsonSerializerContext
{

}

//MenuDbModeOutput