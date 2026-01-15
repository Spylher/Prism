using Microsoft.EntityFrameworkCore;
using Prism.Domain.Interfaces;
using Prism.Infrastructure.Data;
namespace Prism.Infrastructure.Repositories;

public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        await _dbSet.AddAsync(obj);
    }

    public void Update(TEntity obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        _dbSet.Attach(obj);
        _context.Entry(obj).State = EntityState.Modified;
    }

    public void Remove(TEntity obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        _dbSet.Remove(obj);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<List<TEntity>> GetAll()
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }
}

