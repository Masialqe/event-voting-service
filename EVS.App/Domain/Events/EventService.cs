using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Repositories;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;
using EVS.App.Domain.Voters;
using EVS.App.Infrastructure.Database.Repositories;

namespace EVS.App.Domain.Events;

public sealed class EventService(
    IEventRepository eventRepository,
    IVoterEventRepository voterEventRepository)
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

    public async Task<Result<Event?>> GetEventByIdAsync(Guid eventId,
        CancellationToken cancellationToken = default)
    {
        //var result = await eventRepository.GetByIdAsync(eventId, 
            //true, true, cancellationToken);

        var result = await eventRepository.GetIncludingDependencies(eventId, cancellationToken);
        
        if(result == null)
            return EventErrors.EventNotFoundError;
        
        return result;
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
        //1) even doesnt exists
        // 2) event is already sighned
        if(eventId == Guid.Empty || voterId == Guid.Empty) throw new ArgumentNullException(nameof(eventId));
        
        var voterEventState = VoterEvent.Create(eventId, voterId);
        
        try
        {
            await voterEventRepository.LinkEventAndVoterAsync(voterEventState, cancellationToken);
        }
        catch (EventNotFoundException)
        {
            return EventErrors.EventNotFoundError;
        }
        catch (VoterAlreadySignedException)
        {
            return EventErrors.EventAlreadyCointainsVoterError;
        }

        return voterEventState;
    }

    private async Task<bool> IsEventNameAlreadyTaken(string eventName,
        CancellationToken cancellationToken = default)
    {
        var result = await eventRepository.GetByNameAsync(eventName, cancellationToken);

        return result is not null;
    }
}