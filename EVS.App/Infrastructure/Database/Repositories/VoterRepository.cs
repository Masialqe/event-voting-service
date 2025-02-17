using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public class VoterRepository(
    IDbContextFactory<ApplicationDbContext> contextFactory) : GenericRepository<Voter>(contextFactory), IVoterRepository
{
    public async Task<Voter?> GetVoterByNameAsync(string name, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Voters.FirstOrDefaultAsync(x => x.Username == name, cancellationToken), 
            cancellationToken
        );
    }

    public async Task<Voter?> GetVoterByEmailAsync(string email, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Voters.FirstOrDefaultAsync(x => x.Email == email, cancellationToken), 
            cancellationToken
        );
    }

    public async Task<Voter?> GetVoterByUserIdAsync(string userId, 
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Voters.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken), 
            cancellationToken
        );
    }

    public async Task<Voter?> GetIncludingDependencies(Guid voterId,
        CancellationToken cancellationToken = default)
    {
        return await ExecuteAsync(
            context => context.Voters
                .Include(x => x.VoterEvents)
                .FirstOrDefaultAsync(x => x.Id == voterId, cancellationToken), 
            cancellationToken
        );
    }

}