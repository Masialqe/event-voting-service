using EVS.App.Infrastructure.Identity.Users;
using Microsoft.AspNetCore.Identity;
using EVS.App.Shared.Abstractions;
using EVS.App.Shared.Messaging;

namespace EVS.App.Infrastructure.Identity.Implementations;

public class IdentityEmailSender(
    IMessageProducer messageProducer) : IEmailSender<VoterIdentity>
{
    public async Task SendConfirmationLinkAsync(VoterIdentity user, string email, string confirmationLink)
    {
        if(user == null || string.IsNullOrEmpty(user.UserName) ||
           string.IsNullOrEmpty(user.Email) ||
           string.IsNullOrEmpty(confirmationLink))
            throw new ArgumentNullException();
        
        var message = MessageFactory.EmailConfirmationMessage(email, user.UserName, confirmationLink);
        
        await messageProducer.QueueMessage(message);
    }

    public async Task SendPasswordResetLinkAsync(VoterIdentity user, string email, string resetLink)
    {
        if(user == null || string.IsNullOrEmpty(user.UserName) ||
           string.IsNullOrEmpty(user.Email) ||
           string.IsNullOrEmpty(resetLink))
            throw new ArgumentNullException();
        
        var message = MessageFactory.PasswordResetMessage(email, user.UserName, resetLink);
        
        await messageProducer.QueueMessage(message);
    }

    public Task SendPasswordResetCodeAsync(VoterIdentity user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }
} 