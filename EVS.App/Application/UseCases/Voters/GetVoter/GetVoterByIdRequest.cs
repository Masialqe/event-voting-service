namespace EVS.App.Application.UseCases.Voters.GetVoter;

public sealed record GetVoterByIdRequest(Guid VoterId)
{
    public static GetVoterByIdRequest Create(Guid voterId)
        => new GetVoterByIdRequest(voterId);
};
