using EVS.App.Domain.Voters;

namespace EVS.App.Domain.Abstractions;

public interface IVoterRepository
{
    Task<Result> AddVoterAsync(
        Voter voter,
        CancellationToken cancellationToken = default);
    
    Task<Result<Voter>> GetVoterByNameAsync(
        string name, 
        CancellationToken cancellationToken = default);
    
    Task<Result<Voter>> GetVoterByEmailAsync(
        string email, 
        CancellationToken cancellationToken = default);
}