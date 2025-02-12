using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace EVS.App.Infrastructure.Database.Repositories;

public class VoterRepository(
    ApplicationDbContext context) : GenericRepository<Voter>(context), IVoterRepository
{
    private readonly DbSet<Voter> _voters = context.Voters;
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Voter?> GetVoterByNameAsync(string name, 
        CancellationToken cancellationToken = default)
        => await _voters.FirstOrDefaultAsync(x => x.Username == name, cancellationToken);

    public async Task<Voter?> GetVoterByEmailAsync(string email, 
        CancellationToken cancellationToken = default)
        => await _voters.FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    
    public async Task<Voter?> GetVoterByUserIdAsync(string userId, 
        CancellationToken cancellationToken = default)
        => await _voters.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
}