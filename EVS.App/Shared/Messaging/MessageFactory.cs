using EVS.App.Application.Messaging;

namespace EVS.App.Shared.Messaging;

public static class MessageFactory
{
    public static Message EmailConfirmationMessage(string address, string username, string confirmationLink) =>
        Message.Create("Confirm your email address.", 
            $" Hi {username}! <br/> To confirm your email click here <a href='{confirmationLink}'>here</a>.", 
            address);

    public static Message PasswordResetMessage(string address, string username, string resetPasswordLink) =>
        Message.Create("Reset your password",
            $"Hi {username}! <br/> To reset your password click here <a href='{resetPasswordLink}'>here</a>.",
            address);

}