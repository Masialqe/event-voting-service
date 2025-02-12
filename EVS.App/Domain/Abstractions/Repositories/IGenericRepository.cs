using EVS.App.Domain.Abstractions.Entities;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface IGenericRepository<T> where T : class, IEntity
{
    Task CreateAsync(T entity,
        CancellationToken cancellationToken = default);
    
    IQueryable<T> ById(Guid entityId);
    
    Task UpdateAsync(T updatedState,
        CancellationToken cancellationToken = default);
    

    Task DeleteAsync(Guid entityId,
        CancellationToken cancellationToken = default);
}