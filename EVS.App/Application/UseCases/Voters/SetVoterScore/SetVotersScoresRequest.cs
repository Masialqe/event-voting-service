using EVS.App.Domain.VoterEvents;

namespace EVS.App.Application.UseCases.Voters.SetVoterScore;

public sealed record SetVotersScoresRequest(SaveVoterScoreRequest[] VotersScores, Guid EventId, Guid VoteMadeById)
{
    public static SetVotersScoresRequest Create(SaveVoterScoreRequest[] votersScores, Guid eventId, Guid voterMadeById) 
        => new SetVotersScoresRequest(votersScores, eventId, voterMadeById);
};