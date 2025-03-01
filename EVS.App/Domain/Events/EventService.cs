using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Events;

public sealed class EventService(
    IEventRepository eventRepository,
    IVoterEventRepository voterEventRepository,
    ILogger<EventService> logger)
{
    //todo: remove code duplications
    public async Task<Result> CreateEventAsync(CreateEventDto createEventDto, 
        CancellationToken cancellationToken = default)
    {
        if(string.IsNullOrEmpty(createEventDto.EventName) || string.IsNullOrEmpty(createEventDto.EventDescription))
            throw new ArgumentNullException();
        
        if (await IsEventNameAlreadyTaken(createEventDto.EventName, cancellationToken))
            return EventErrors.EventAlreadyExistError;

        var newEvent = createEventDto.ToDomain();
        
        await eventRepository.CreateAsync(newEvent, cancellationToken);

        return Result.Success();
    }

    public async Task<Result<Event?>> GetEventByIdAsync(Guid eventId, CancellationToken cancellationToken = default)
    => (await eventRepository.GetIncludingDependencies(eventId, cancellationToken)) is { } result
            ? result
            : EventErrors.EventNotFoundError;
    
    public async Task<Result> StartEventAsync(Guid eventId,
        CancellationToken cancellationToken = default)
    {
        if(eventId == Guid.Empty) return EventErrors.InvalidEventDataError;
        
        var eventState = await eventRepository.GetByIdAsync(eventId, cancellationToken);
        
        if(eventState == null) return EventErrors.EventNotFoundError;
        
        eventState.Start();
        await eventRepository.UpdateAsync(eventState, cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result> EndEventAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        if (eventId == Guid.Empty) return EventErrors.InvalidEventDataError;
        
        var eventState = await eventRepository.GetByIdAsync(eventId, cancellationToken);
        
        if(eventState == null) return EventErrors.EventNotFoundError;
        
        eventState.End();
        await eventRepository.UpdateAsync(eventState, cancellationToken);
        
        return Result.Success();
    }

    public async Task<Result<Guid>> AddVoterToEventAsync(Guid eventId, Guid voterId,
        CancellationToken cancellationToken = default)
    {
        if(eventId == Guid.Empty || voterId == Guid.Empty) return EventErrors.InvalidEventDataError;
        
        var voterEventState = VoterEvent.Create(eventId, voterId);

        try
        {
            logger.LogError("Entry data eventId {EventId} voterId {VoterId}", eventId, voterId);
            await voterEventRepository.LinkEventAndVoterAsync(voterEventState, cancellationToken);
        }
        catch (Exception ex) when (ex is DomainException)
        {
            var errorResult = ex switch
            {
                EventNotFoundException => EventErrors.EventNotFoundError,
                VoterAlreadySignedException => EventErrors.EventAlreadyCointainsVoterError,
                VoterLimitReachedException => EventErrors.VoterLimitReachedError,
                _ => EventErrors.EventOperationCannotBeExecutedError
            };

            return errorResult;
        }

        return voterEventState.Id;
    }
    
    //todo: create repository method to just check if event exists
    private async Task<bool> IsEventNameAlreadyTaken(string eventName,
        CancellationToken cancellationToken = default)
    {
        var result = await eventRepository.GetByNameAsync(eventName, cancellationToken);

        return result is not null;
    }
}