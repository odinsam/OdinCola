using Cola.CaEF.EntityBase;
using SqlSugar;

namespace ConsoleApp1Test.EFTest;

[SugarTable("tb_OdinLog")]
public class OdinLog : SqlSugarEntityBase<long>, IStatus
{
    public string? LogLevel { get; set; }
    public string? LogContent { get; set; }
    public string? ExceptionInfo { get; set; }
}