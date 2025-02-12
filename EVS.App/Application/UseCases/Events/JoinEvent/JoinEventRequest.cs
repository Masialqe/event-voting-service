using EVS.App.Domain.Voters;

namespace EVS.App.Application.UseCases.Events.JoinEvent;

public sealed record JoinEventRequest(Voter Voter, Guid EventId);