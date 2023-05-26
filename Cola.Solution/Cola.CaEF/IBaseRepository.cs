using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SqlSugar;

namespace Cola.CaEF;

public interface IBaseRepository<TEntity>
{
    /// <summary>
    ///     EF ORM 方法
    /// </summary>
    /// <param name="func"></param>
    /// <returns></returns>
    object? ColaOrm(Func<DbContext, object?> func);

    /// <summary>
    ///     功能描述:自定义数据查询
    /// </summary>
    /// <returns>数据列表</returns>
    object? ColaSqlSugar(Func<ISqlSugarClient, object?> func);

    #region 事务

    void BeginTran();

    void Commit();

    void Rollback();

    #endregion

    #region 查询

    /// <summary>
    ///     功能描述:按照条件查询数据是否存在
    /// </summary>
    /// <param name="anyExpression">anyExpression</param>
    /// <returns>数据列表</returns>
    bool QueryEntityAny(Expression<Func<TEntity, bool>>? anyExpression);

    /// <summary>
    ///     功能描述:查询所有数据
    /// </summary>
    /// <returns>数据列表</returns>
    List<TEntity> QueryEntities();

    /// <summary>
    ///     功能描述:按照Id查询唯一数据
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>数据列表</returns>
    TEntity QueryEntityById<T>(T id);

    /// <summary>
    ///     功能描述:按照条件查询唯一数据
    /// </summary>
    /// <param name="singleExpression">singleExpression</param>
    /// <returns>数据列表</returns>
    TEntity QueryEntitySingle(Expression<Func<TEntity, bool>>? singleExpression);

    /// <summary>
    ///     功能描述:查询数据列表
    /// </summary>
    /// <param name="whereExpression">whereExpression</param>
    /// <returns>数据列表</returns>
    List<TEntity> QueryEntities(Expression<Func<TEntity, bool>>? whereExpression);

    /// <summary>
    ///     功能描述:查询一个列表
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <param name="orderByExpression"></param>
    /// <param name="isAsc"></param>
    /// <returns>数据列表</returns>
    List<TEntity> QueryEntities(Expression<Func<TEntity, bool>>? whereExpression,
        Expression<Func<TEntity, object>>? orderByExpression, bool isAsc = true);

    /// <summary>
    ///     功能描述:分页查询
    /// </summary>
    /// <param name="whereExpression">条件表达式</param>
    /// <param name="orderByExpression">排序表达式</param>
    /// <param name="isAsc">是否asc排序</param>
    /// <param name="intPageIndex">页索引</param>
    /// <param name="intPageSize">页记录数</param>
    /// <param name="intTotalCount">总页数</param>
    /// <returns>数据列表</returns>
    List<TEntity> QueryEntities(
        Expression<Func<TEntity, bool>>? whereExpression,
        Expression<Func<TEntity, object>>? orderByExpression,
        bool isAsc,
        int intPageIndex,
        int intPageSize,
        out int intTotalCount);

    #endregion


    #region 新增

    /// <summary>
    ///     写入实体数据(同步方法)
    /// </summary>
    /// <param name="entity">实体类</param>
    /// <returns>返回受影响的行数</returns>
    int AddEntity(TEntity entity);

    /// <summary>
    ///     写入实体数据
    /// </summary>
    /// <param name="listEntities">实体类集合</param>
    /// <returns>返回被影响的行数</returns>
    int AddEntities(List<TEntity> listEntities);

    /// <summary>
    ///     大数据批量插入实体(速度快)
    /// </summary>
    /// <param name="listEntities">实体类集合</param>
    /// <returns>影响行数</returns>
    int FastAddEntities(List<TEntity> listEntities);

    #endregion


    #region 更新

    /// <summary>
    ///     同步 更新
    /// </summary>
    /// <param name="entity">update entity</param>
    /// <returns>影响行数</returns>
    int UpdateEntity(TEntity entity);

    /// <summary>
    ///     同步 更新
    /// </summary>
    /// <param name="entities">update entities</param>
    /// <returns>影响行数</returns>
    int UpdateEntities(List<TEntity> entities);

    /// <summary>
    ///     大数据快速更新
    /// </summary>
    /// <param name="entities">update entities</param>
    /// <returns>影响行数</returns>
    int FastUpdateEntities(List<TEntity> entities);

    #endregion


    #region 删除

    /// <summary>
    ///     根据实体删除一条数据
    /// </summary>
    /// <param name="entity">delete 实体类</param>
    /// <returns>影响行数</returns>
    int DeleteEntity(TEntity entity);

    /// <summary>
    ///     删除指定条件的数据
    /// </summary>
    /// <param name="whereExpression">delete whereExpression</param>
    /// <returns>影响行数</returns>
    int DeleteByWhere(Expression<Func<TEntity, bool>>? whereExpression);

    /// <summary>
    ///     删除指定条件的数据
    /// </summary>
    /// <param name="entities">delete entities</param>
    /// <returns>影响行数</returns>
    int DeleteEntities(List<TEntity> entities);

    #endregion
}