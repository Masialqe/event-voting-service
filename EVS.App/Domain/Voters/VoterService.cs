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

    private async Task<bool> IsVoterExistsAsync(Voter voter,
        CancellationToken cancellationToken = default)
    {
        var voterByUsername = await voterRepository.GetVoterByNameAsync(voter.Username, cancellationToken);
        var voterByEmail = await voterRepository.GetVoterByEmailAsync(voter.Email, cancellationToken);
        
        return voterByUsername is not null || voterByEmail is not null;
    }
}