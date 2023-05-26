using SqlSugar;

namespace Cola.CaEF.Models;

public class AopOnLogExecutingModel
{
    public string? ConfigId { get; set; }
    public Action<string, SugarParameter[]>? AopOnLogExecuting { get; set; }
}