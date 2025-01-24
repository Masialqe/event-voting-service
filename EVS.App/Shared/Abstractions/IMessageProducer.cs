using EVS.App.Application.Messaging;

namespace EVS.App.Shared.Abstractions;

public interface IMessageProducer
{
    Task QueueMessage(Message message,
        CancellationToken cancellationToken = default);
}