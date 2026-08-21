namespace Prism.Domain.Interfaces;

public interface IRepositoryBase<TEntity> where TEntity : class
{
    Task AddAsync(TEntity obj);
    void Update(TEntity obj);
    void Remove(TEntity obj);
    void RemoveRange(IEnumerable<TEntity> entities);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAll();
    IQueryable<TEntity> Query();
}
