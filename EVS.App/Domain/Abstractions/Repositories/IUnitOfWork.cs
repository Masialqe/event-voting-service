namespace EVS.App.Domain.Abstractions.Repositories;

public interface IUnitOfWork
{
    Task SaveTransactionAsync(Func<Task> action,
        CancellationToken cancellationToken = default);
}