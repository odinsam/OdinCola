using System.Linq.Expressions;
using SqlSugar;

namespace Cola.CaEF.BaseRepository;

public interface ISqlSugarRepository
{
    SqlSugarClient Db { get; }
    void BeginTran();
    void CommitTran();
    void RollbackTran();
    bool Insert<T>(T entity) where T : class, new();
    bool InsertRange<T>(List<T> entities) where T : class, new();
    int InsertBulkCopy<T>(List<T> entities) where T : class, new();
    bool Update<T>(T entity) where T : class, new();
    bool Update<T>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T>> updateExpression) where T : class, new();
    bool Delete<T>(T entity) where T : class, new();
    bool Delete<T>(Expression<Func<T, bool>> whereExpression) where T : class, new();
    T GetById<T>(object id) where T : class, new();
    List<T> GetAll<T>() where T : class, new();
    List<T> GetList<T>(Expression<Func<T, bool>> whereExpression) where T : class, new();
    List<T> GetPageList<T>(Expression<Func<T, bool>> whereExpression, PageModel pageModel) where T : class, new();
}