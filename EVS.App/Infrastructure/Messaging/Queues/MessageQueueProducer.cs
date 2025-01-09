using EVS.App.Application.Messaging;
using EVS.App.Shared.Abstractions;
using MassTransit;

namespace EVS.App.Infrastructure.Messaging.Queues;

public sealed class MessageQueueProducer(
    IBus messageBus,
    ILogger<MessageQueueProducer> logger) : IMessageProducer
{
    /// <summary>
    /// Queue given message type.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="cancellationToken"></param>
    public async Task QueueMessage(Message message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        try
        {
            await messageBus.Publish(message, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to queue message - {Message}. {ExceptionMessage} {Exception}", 
                message, ex.Message, ex);
            throw;
        }
    }
}