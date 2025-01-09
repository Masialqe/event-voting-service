using EVS.App.Application.Messaging;

namespace EVS.App.Application.Abstractions;

public interface IEmailSender
{
    Task SendAsync(Message message, CancellationToken cancellationToken = default);
}