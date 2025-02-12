using EVS.App.Domain.Abstractions;
using EVS.App.Domain.Abstractions.Entities;
using EVS.App.Domain.Events;
using EVS.App.Domain.Exceptions;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Voters;

public class Voter : IVoterEventEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Username { get; private set; } = string.Empty;
    public string UserId { get; private set; } = string.Empty;
    
    public string Email { get; private set; } = string.Empty;
    
    public ICollection<VoterEvent> VoterEvents { get; set; } = [];
    public ICollection<Event> CreatedEvents { get; set; } = [];

    public static Voter Create(string nickname, string userId, string email)
        => new Voter(nickname, userId, email);

    private Voter(string nickname, string userId, string email)
    {
        Username = nickname;
        UserId = userId;
        Email = email;
    }
    
    public Voter(){}

    public void AddVoterEvent(VoterEvent @event)
    {
        var eventId = @event.EventId;
        
        if (VoterEvents.Any(x => x.EventId == eventId))
            throw new VoterAlreadySignedException($"Voter is already signed to event {eventId}.");

        VoterEvents.Add(@event);
    }
}