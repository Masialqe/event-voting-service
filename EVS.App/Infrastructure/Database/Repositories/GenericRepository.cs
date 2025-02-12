using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public class GenericRepository<T>(
    ApplicationDbContext context) : IGenericRepository<T>
    where T : class, IEntity
{
    private readonly DbSet<T> _dbSet = context.Set<T>();
    
    public virtual async Task CreateAsync(T entity, 
        CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public virtual IQueryable<T> ById(Guid entityId)
        => _dbSet.Where(x => x.Id == entityId);
    
    public virtual async Task UpdateAsync(T updatedState, 
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(updatedState);
        
        var entityState = _dbSet.Entry(updatedState);
        
        if(entityState.State == EntityState.Detached) _dbSet.Attach(updatedState);
        
        entityState.State = EntityState.Modified;
        
        await context.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(Guid entityId,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == entityId, cancellationToken);
        
        ArgumentNullException.ThrowIfNull(entity);
        _dbSet.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}