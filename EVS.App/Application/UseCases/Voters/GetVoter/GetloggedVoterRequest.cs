namespace EVS.App.Application.UseCases.Voters.GetVoter;

public sealed record GetloggedVoterRequest()
{
    public static GetloggedVoterRequest Create() 
        => new GetloggedVoterRequest();
}
