using SqlSugar;

namespace Cola.CaEF.Models;

public class AopOnErrorModel
{
    public string? ConfigId { get; set; }
    public Action<SqlSugarException>? AopOnError { get; set; }
}