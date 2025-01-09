using EVS.App.Application.Messaging;

namespace EVS.App.Shared.Abstractions;

public interface IMessageService
{
    Task SendAsync(Message message, CancellationToken cancellationToken = default);
}