using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public class GenericRepository<T>(
    IDbContextFactory<ApplicationDbContext> contextFactory) : IGenericRepository<T>
    where T : class, IEntity
{
    public virtual async Task CreateAsync(T entity, 
        CancellationToken cancellationToken = default)
            => await SaveAsync(async context => await context.Set<T>().AddAsync(entity, cancellationToken), cancellationToken);
    
    public virtual async Task<T?> GetByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
            => await ExecuteAsync(async context => await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken), cancellationToken);
    
    public virtual async Task<T?> GetByIdNoTrackingAsync(Guid id, 
        CancellationToken cancellationToken = default)
        => await ExecuteAsync(async context => await context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken), cancellationToken);
    
    public virtual Task UpdateAsync(T updatedState, 
        CancellationToken cancellationToken = default) =>
            SaveAsync(context =>
            {
                ArgumentNullException.ThrowIfNull(updatedState);
            
                var entityState = context.Set<T>().Entry(updatedState);

                if (entityState.State == EntityState.Detached)
                    context.Set<T>().Attach(updatedState);

                entityState.State = EntityState.Modified;
            
                return Task.CompletedTask;
            }, cancellationToken);
    
    public virtual async Task DeleteAsync(Guid entityId,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        await context.Set<T>()
            .Where(x => x.Id == entityId).ExecuteDeleteAsync(cancellationToken);
    }

    protected async Task<TResult> ExecuteAsync<TResult>(Func<ApplicationDbContext, Task<TResult>> query,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return await query(context);
    }

    protected async Task SaveAsync(Func<ApplicationDbContext, Task> action,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        await action(context);
        await context.SaveChangesAsync(cancellationToken);
    }
}