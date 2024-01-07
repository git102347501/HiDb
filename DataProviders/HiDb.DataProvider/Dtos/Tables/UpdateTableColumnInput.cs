namespace HiDb.DataProvider.Dtos.Tables;

public class UpdateTableColumnInput
{
    public string DataBase { get; set; }

    public string Mode { get; set; }

    public string Table { get; set; }
    
    public string Column { get; set; }

    public string Type { get; set; }

    public bool Required { get; set; }
}