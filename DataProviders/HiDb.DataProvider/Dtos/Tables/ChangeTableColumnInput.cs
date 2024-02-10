namespace HiDb.DataProvider.Dtos.Tables;

public interface ChangeTableColumnInput
{
    public string DataBase { get; set; }

    public string Mode { get; set; }

    public string Table { get; set; }

    public string Column { get; set; }

    public string Type { get; set; }

    public bool Required { get; set; }
        
    /// <summary>
    /// 默认值
    /// </summary>
    public string DftValue { get; set; }

    /// <summary>
    /// 精度
    /// </summary>
    public int? NumericPrecision { get; set; }

    /// <summary>
    /// 小数位
    /// </summary>
    public int? NumSize { get; set; }

    public string? Remark { get; set; }
}