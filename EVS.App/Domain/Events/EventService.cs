using EVS.App.Domain.Abstractions;
using EVS.App.Domain.VoterEvents;
using MassTransit;

namespace EVS.App.Domain.Events;

public sealed class EventService(
    IEventRepository eventRepository)
{
    //todo: remove code duplications
    public async Task<Result> CreateEventAsync(string eventName, string eventDescription,
        Guid voterId, CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventDescription))
            throw new ArgumentNullException();
        
        if (await IsEventNameAlreadyTaken(eventName, cancellationToken))
            return EventErrors.EventAlreadyExistError;
        
        var newEvent = Event.Create(eventName, eventDescription, voterId);
        
        await eventRepository.CreateAsync(newEvent, cancellationToken);

        return Result.Success();
    }

    public async Task<Result> StartEventAsync(Guid eventId,
        CancellationToken cancellationToken = default)
    {
        if(eventId == Guid.Empty) throw new ArgumentNullException(nameof(eventId));
        
        var eventState = await eventRepository.GetByIdAsync(eventId: eventId, cancellationToken: cancellationToken);
        
        if(eventState == null) return EventErrors.EventNotFoundError;
        
        eventState.Start();
        await eventRepository.UpdateAsync(eventState, cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> EndEventAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        if(eventId == Guid.Empty) throw new ArgumentNullException(nameof(eventId));
        
        var eventState = await eventRepository.GetByIdAsync(eventId: eventId, cancellationToken: cancellationToken);
        
        if(eventState == null) return EventErrors.EventNotFoundError;
        
        eventState.End();
        await eventRepository.UpdateAsync(eventState, cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result<VoterEvent>> AddVoterToEventAsync(Guid eventId, Guid voterId,
        CancellationToken cancellationToken = default)
    {
        if(eventId == Guid.Empty || voterId == Guid.Empty) throw new ArgumentNullException(nameof(eventId));
        
        var voterEventState = VoterEvent.Create(eventId, voterId);
        
        var eventState = await eventRepository.GetByIdAsync(eventId: eventId, cancellationToken: cancellationToken);
        
        if(eventState == null) return EventErrors.EventNotFoundError;
        
        eventState.AddVoter(voterEventState);
        await eventRepository.UpdateAsync(eventState, cancellationToken);

        return voterEventState;
    }

    private async Task<bool> IsEventNameAlreadyTaken(string eventName,
        CancellationToken cancellationToken = default)
    {
        var result = await eventRepository.GetByNameAsync(eventName, cancellationToken);

        return result is not null;
    }
}