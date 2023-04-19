namespace TMS.Database.DatabaseRepository;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity GetById(int id);
    IEnumerable<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
    Task<TEntity> Add(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task Delete(TEntity entity);
}