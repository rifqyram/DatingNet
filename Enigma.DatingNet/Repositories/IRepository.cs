using System.Linq.Expressions;

namespace Enigma.DatingNet.Repositories;

public interface IRepository<TEntity>
{
    Task<TEntity> SaveAsync(TEntity entity);
    TEntity Attach(TEntity entity);
    Task<IEnumerable<TEntity>> SaveAllAsync(IEnumerable<TEntity> entities);
    Task<TEntity?> FindByIdAsync(Guid id);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria);
    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes);
    Task<IEnumerable<TEntity>> FindAllAsync();
    Task<IEnumerable<TEntity>> FindAllAsync(string[] includes);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int page, int size);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int page, int size, string[] includes);
    Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int? page, int? size, string[]? includes, Expression<Func<TEntity, object>>? orderBy, string direction);
    TEntity Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteAll(IEnumerable<TEntity> entities);
    Task<int> CountAsync();
    Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria);
}