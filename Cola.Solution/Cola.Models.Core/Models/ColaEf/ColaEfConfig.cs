using Cola.CaUtils.Extensions;
using SqlSugar;

namespace Cola.Models.Core.Models.ColaEf;

public class ColaEfConfig
{
    public string? ConfigId { get; set; } = "1";
    public string? Domain { get; set; } = null;
    public string? DbType { get; set; }

    public string? ConnectionString { get; set; } = null;

    public bool IsAutoCloseConnection { get; set; } = true;

    public bool EnableLogAop { get; set; } = true;

    public bool EnableErrorAop { get; set; } = true;

    public bool EnableGlobalFilter { get; set; } = false;

    public DbType GetSqlSugarDbType()
    {
        if (string.IsNullOrEmpty(DbType)) throw new Exception("SqlSugar配置没有明确指定DbType");
        return DbType!.ConvertStringToEnum<DbType>();
    }
}