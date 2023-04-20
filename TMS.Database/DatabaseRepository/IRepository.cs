namespace TMS.Database.DatabaseRepository;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetById(int id);
    IQueryable<TEntity> GetAll();
    Task<TEntity> Add(TEntity entity);
    Task<TEntity> Update(TEntity entity);
    Task Delete(TEntity entity);
}