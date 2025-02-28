﻿using EVS.App.Domain.Abstractions;

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

        await voterRepository.AddVoterAsync(voter, cancellationToken);
        return Result.Success();
    }

    public async Task<bool> IsVoterExistsAsync(Voter voter,
        CancellationToken cancellationToken = default)
    {
        var voterByUsername = await voterRepository.GetVoterByNameAsync(voter.Username, cancellationToken);
        var voterByEmail = await voterRepository.GetVoterByEmailAsync(voter.Email, cancellationToken);

        return voterByUsername.IsSuccess || voterByEmail.IsSuccess;
    }
}