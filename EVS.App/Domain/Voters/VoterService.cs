using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;

namespace EVS.App.Domain.Voters;

public class VoterService(
    IVoterRepository voterRepository)
{
    public async Task<Result> CreateVoterAsync(
        Voter voter,
        CancellationToken cancellationToken = default)
    {
        if (await IsVoterExistsAsync(voter, cancellationToken))
            return VoterErrors.VoterAlreadyExistsError;

        await voterRepository.CreateAsync(voter, cancellationToken);
        return Result.Success();
    }

    public async Task<Result<Voter>> GetVoterAsync(Guid voterId, 
        CancellationToken cancellationToken = default)
    {
        var result = await voterRepository.GetByIdNoTrackingAsync(voterId, cancellationToken);

        return result is null
            ? VoterErrors.VoterNotFoundError
            : result;
    }

    private async Task<bool> IsVoterExistsAsync(Voter voter,
        CancellationToken cancellationToken = default)
    {
        var voterByUsername = await voterRepository.GetVoterByNameAsync(voter.Username, cancellationToken);
        var voterByEmail = await voterRepository.GetVoterByEmailAsync(voter.Email, cancellationToken);
        
        return voterByUsername is not null || voterByEmail is not null;
    }
}