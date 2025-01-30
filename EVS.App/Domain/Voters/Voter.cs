using EVS.App.Domain.Events;
using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Voters;

public class Voter
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
    
}