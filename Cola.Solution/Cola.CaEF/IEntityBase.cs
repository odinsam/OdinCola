namespace Cola.CaEF;

/// <summary>
///     IEntityBase
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IEntityBase<T> : IEntity
{
    /// <summary>
    ///     默认主键字段是F_Id
    /// </summary>
    T? Id { get; set; }
}