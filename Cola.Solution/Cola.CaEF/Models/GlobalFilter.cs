using SqlSugar;

namespace Cola.CaEF.Models;

public class GlobalQueryFilter
{
    public string? ConfigId { get; set; }
    public Action<QueryFilterProvider>? QueryFilter { get; set; }
}