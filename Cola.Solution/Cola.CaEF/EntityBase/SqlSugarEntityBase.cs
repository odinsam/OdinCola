namespace Cola.CaEF.EntityBase;

/// <summary>
///     SqlSugarEntityBase
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlSugarEntityBase<T> : IEntityBase<T>, IDeleted
{
    /// <summary>
    ///     Id主键
    /// </summary>
    public T? Id { get; set; }
    
    /// <summary>
    ///     Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    ///     CreateUser
    /// </summary>
    public string? CreateUser { get; set; }

    /// <summary>
    ///     CreateTime
    /// </summary>
    public DateTime? CreateTime { get; set; }

    /// <summary>
    ///     Remark
    /// </summary>
    public string? UpdateUser { get; set; }

    /// <summary>
    ///     Remark
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    ///     isDelete 0 false other true
    /// </summary>
    public bool IsDelete { get; set; }
    
}