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
    /// 获取表属性列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("column/list")]
    public async Task<List<TableColumnFullOutput>> GetDbColumnListAsync(TableColumnInput input,
        CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).GetDbColumnListAsync(input,
            cancellationToken);
        return data;
    }

    /// <summary>
    /// 获取支持的类型
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("db-type")]
    public async Task<List<TableDbTypeOutput>> GetDbTypeList(CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).GetDbTypeListAsync(cancellationToken);
        return data;
    }

    /// <summary>
    /// 删除表
    /// </summary>
    /// <param name="database"></param>
    /// <param name="mode"></param>
    /// <param name="table"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteTable(string database, string mode, string table,
        CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).DeleteTableAsync(database, mode, table,
            cancellationToken);
        return data;
    }
    
    /// <summary>
    /// 清空表
    /// </summary>
    /// <param name="database"></param>
    /// <param name="mode"></param>
    /// <param name="table"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("clear")]
    [HttpPut]
    public async Task<bool> ClearTable(string database, string mode, string table,
        CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).ClearTableAsync(database, mode, table,
            cancellationToken);
        return data;
    }

    /// <summary>
    /// 更新表列配置
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut("column/config")]
    public async Task<bool> UpdateColumnConfigAsync(UpdateTableColumnInput input,
        CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).UpdateColumnConfigAsync(input,
            cancellationToken);
        return data;
    }

    [HttpPost("column/config")]
    public async Task<bool> AddColumnConfigAsync(AddTableColumnInput input,
        CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).AddColumnConfigAsync(input,
            cancellationToken);
        return data;
    }

    [HttpDelete("column/config")]
    public async Task<bool> DeleteColumnConfigAsync(DeleteTableColumnInput input,
        CancellationToken cancellationToken = default)
    {
        var data = await GetService(ServiceFactory.GetTable).DeleteColumnConfigAsync(input,
            cancellationToken);
        return data;
    }
}