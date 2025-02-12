using EVS.App.Domain.Voters;

namespace EVS.App.Domain.Abstractions.Repositories;

public interface IVoterRepository : IGenericRepository<Voter>
{
    Task<Voter?> GetVoterByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);
    
    Task<Voter?> GetVoterByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default);
    
    Task<Voter?> GetVoterByUserIdAsync(
        string userId, 
        CancellationToken cancellationToken = default);
}