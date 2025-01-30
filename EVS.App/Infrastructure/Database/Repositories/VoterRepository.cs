using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public class VoterRepository(
    ApplicationDbContext context) : IVoterRepository
{
    private readonly DbSet<Voter> _voters = context.Voters;
    public async Task AddVoterAsync(Voter voter, CancellationToken cancellationToken = default)
    {
        await _voters.AddAsync(voter, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
  
    public async Task<Voter?> GetVoterByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var result = await _voters
            .FirstOrDefaultAsync(x => x.Username == name, cancellationToken);

        return result;
    }

    public async Task<Voter?> GetVoterByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var result = await _voters
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        return result;
    }

    public async Task<Voter?> GetVoterByUserIdAsync(string userId, 
        CancellationToken cancellationToken = default)
    {
        var result = await _voters
            .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        
        return result;
    }
}