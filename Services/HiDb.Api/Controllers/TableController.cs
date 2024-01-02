using HiDb.DataProvider.Dtos.Search;
using HiDb.DataProvider.Dtos.Tables;
using HiDb.DataProvider.Factory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiDb.Api.Controllers;

/// <summary>
/// Search
/// </summary>
/// <param name="searchDataProvider"></param>
[ApiController]
[AllowAnonymous]
[Route("table")]
public class TableController : MainController
{
    /// <summary>
    /// 获取属性详情
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("column/detail")]
    public TableColumnFullOutput GetDbColumnFullInfo(TableColumnFullInput input)
    {
        var data = GetService(ServiceFactory.GetTable).GetDbColumnFullInfo(input);
        return data;
    }

    /// <summary>
    /// 获取表属性列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("column/list")]
    public List<TableColumnOutput> GetDbColumnList(TableColumnInput input)
    {
        var data = GetService(ServiceFactory.GetTable).GetDbColumnList(input);
        return data;
    }

    [HttpPost("db-type")]
    public List<TableDbTypeOutput> GetDbTypeList()
    {
        var data = GetService(ServiceFactory.GetTable).GetDbTypeList();
        return data;
    }
}