namespace EVS.App.Application.UseCases.Voters.GetVoterEvent;

public sealed record GetVoterEventByIdRequest(Guid VoterEventId)
{
    public static GetVoterEventByIdRequest Create(Guid voterEventId)
        => new GetVoterEventByIdRequest(voterEventId);
}
