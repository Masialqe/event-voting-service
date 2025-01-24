namespace EVS.App.Application.Messaging;

public record Message
{
    public required string Subject { get; init; }
    public required string Content { get; init; }
    public required string Receiver { get; init; }
    
    public static Message Create(string subject, string content, string receiver)
        => new Message{ Subject = subject, Content = content, Receiver = receiver };
}