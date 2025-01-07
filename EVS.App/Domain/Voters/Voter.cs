using EVS.App.Domain.VoterEvents;

namespace EVS.App.Domain.Voters;

public class Voter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public ICollection<VoterEvent> VoterEvents { get; set; } = [];

    public static Voter Create(string nickname, string userId, string email)
        => new Voter().SetName(nickname).SetUserId(userId).SetEmail(email);

    public Voter SetName(string nickname)
    {
        Username = nickname;
        return this;
    }

    public Voter SetUserId(string userId)
    {
        UserId = userId;
        return this;
    }

    public Voter SetEmail(string email)
    {
        Email = email;
        return this;
    }
}