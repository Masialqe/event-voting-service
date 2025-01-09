using EVS.App.Application.Abstractions;
using EVS.App.Application.Messaging;
using EVS.App.Infrastructure.Messaging.Queues;
using EVS.App.Infrastructure.Messaging.Services;
using EVS.App.Shared.Abstractions;

namespace EVS.App.Infrastructure.Messaging.ApplicationServices;

public class EmailMessageService(
    MessageQueueProducer messageQueueProducer) : IMessageService
{
    public async Task SendAsync(Message message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        await messageQueueProducer.QueueMessage(message, cancellationToken);
    }
}