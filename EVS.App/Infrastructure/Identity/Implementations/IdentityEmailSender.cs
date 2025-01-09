using EVS.App.Infrastructure.Identity.Users;
using EVS.App.Shared.Abstractions;
using EVS.App.Shared.Messaging;
using Microsoft.AspNetCore.Identity;

namespace EVS.App.Infrastructure.Identity.Implementations;

public class IdentityEmailSender(
    IMessageProducer messageProducer) : IEmailSender<VoterIdentity>
{
    public async Task SendConfirmationLinkAsync(VoterIdentity user, string email, string confirmationLink)
    {
        ArgumentNullException.ThrowIfNull(user.UserName);
        
        var message = MessageFactory.EmailConfirmationMessage(email, user.UserName, confirmationLink);
        
        await messageProducer.QueueMessage(message);
    }

    public Task SendPasswordResetLinkAsync(VoterIdentity user, string email, string resetLink)
    {
        throw new NotImplementedException();
    }

    public Task SendPasswordResetCodeAsync(VoterIdentity user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }
} 