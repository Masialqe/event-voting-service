using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Events;
using EVS.App.Domain.VoterEvents;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Services;

public class TransactionService(
    IDbContextFactory<ApplicationDbContext> contextFactory,
    ILogger<TransactionService> logger) : ITransactionService
{
    public async Task RegisterVoteAsync(SaveVoterScoreRequest[] request, Event relatedEventState, 
        VoterEvent voteOwnerState, CancellationToken cancellationToken = default)
    {
        await SaveTransactionAsync(async context =>
        {
            var voterEventsId = request.Select(x => x.VoterId).ToArray();
            var voterEventStates = await context.Set<VoterEvent>()
                .Where(x => voterEventsId.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);
            
            foreach (var saveVoterScoreRequest in request)
            {
                var currentState = voterEventStates.GetValueOrDefault(saveVoterScoreRequest.VoterId) 
                                   ?? throw new KeyNotFoundException($"Voter event not found {saveVoterScoreRequest.VoterId}");

                currentState.AddScore(saveVoterScoreRequest.Score);
            }
            
            //Update owner of vote state
            await UpdateAsync(context, voteOwnerState, cancellationToken);
            //Update event state 
            await UpdateAsync(context, relatedEventState, cancellationToken);
            
        }, cancellationToken);
    }
    private async Task SaveTransactionAsync(Func<ApplicationDbContext, Task> action,
        CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await action(context);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to save transaction due to {ExceptionMessage} with exception {Exception}", 
                ex.Message, ex);
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private Task UpdateAsync<TEntity>(ApplicationDbContext context,TEntity updatedState,
        CancellationToken cancellationToken = default) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(updatedState);
            
        var entityState = context.Set<TEntity>().Entry(updatedState);

        if (entityState.State == EntityState.Detached)
            context.Set<TEntity>().Attach(updatedState);

        entityState.State = EntityState.Modified;
            
        return Task.CompletedTask;
    }
        
}