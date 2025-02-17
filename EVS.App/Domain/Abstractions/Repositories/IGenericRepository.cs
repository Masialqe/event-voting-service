using EVS.App.Domain.Abstractions.Entities;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface IGenericRepository<T> where T : class, IEntity
{
    Task CreateAsync(T entity,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<T?> GetByIdNoTrackingAsync(Guid id,
        CancellationToken cancellationToken = default);
    
    Task UpdateAsync(T updatedState,
        CancellationToken cancellationToken = default);
    
    Task DeleteAsync(Guid entityId,
        CancellationToken cancellationToken = default);
}