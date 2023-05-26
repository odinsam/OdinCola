using SqlSugar;

namespace ConsoleApp1Test.EFTest;

[SugarTable("tb_PushEvent")]
public class PushEvent : IStatus
{
    /// <summary>
    ///     Id主键
    /// </summary>
    [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = false)]
    public long? Id { get; set; }

    /// <summary>
    ///     Remark
    /// </summary>
    [SugarColumn(ColumnName = "Remark", Length = 128)]
    public string? Remark { get; set; }

    /// <summary>
    ///     CreateUser default: odin
    /// </summary>
    [SugarColumn(ColumnName = "CreateUser", Length = 32)]
    public string? CreateUser { get; set; }

    /// <summary>
    ///     CreateTime
    /// </summary>
    [SugarColumn(ColumnName = "CreateTime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    ///     Remark
    /// </summary>
    [SugarColumn(ColumnName = "UpdateUser", Length = 32)]
    public string? UpdateUser { get; set; }

    /// <summary>
    ///     Remark
    /// </summary>
    [SugarColumn(ColumnName = "UpdateTime")]
    public DateTime? UpdateTime { get; set; }


    [SugarColumn(ColumnName = "EventName")]
    public string? EventName { get; set; }

    [SugarColumn(ColumnName = "EventInfo")]
    public string? EventInfo { get; set; }

    [SugarColumn(ColumnName = "Direction")]
    public string? Direction { get; set; }

    [SugarColumn(ColumnName = "EventParams")]
    public string? EventParams { get; set; }

    [SugarColumn(ColumnName = "IsDelete")] public bool IsDelete { get; set; }
}