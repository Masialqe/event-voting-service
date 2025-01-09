using EVS.App.Application.Messaging;

namespace EVS.App.Shared.Messaging;

public static class MessageFactory
{
    public static Message EmailConfirmationMessage(string address, string username, string confirmationLink) =>
        Message.Create($"Hi {username}! <br/> Confirm your email address. <br/>", 
            $"To confirm your email click here <a href='{confirmationLink}'>here</a>.", 
            address);
}