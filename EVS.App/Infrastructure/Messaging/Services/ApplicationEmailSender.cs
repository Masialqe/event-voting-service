using EVS.App.Application.Abstractions;
using EVS.App.Application.Messaging;
using EVS.App.Shared.Abstractions;

namespace EVS.App.Infrastructure.Messaging.Services;

public class ApplicationEmailSender(
    IMessageProducer messageProducer) : IEmailSender
{
    public async Task SendAsync(Message message, CancellationToken cancellationToken = default)
    {
        await messageProducer.QueueMessage(message, cancellationToken);
    }
}