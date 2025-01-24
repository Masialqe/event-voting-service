using EVS.App.Application.Messaging;
using EVS.App.Shared.Abstractions;
using MassTransit;

namespace EVS.App.Infrastructure.Messaging.Queues;

public sealed class EmailMessageQueueConsumer(
    IMessageService messageService,
    ILogger<EmailMessageQueueConsumer> logger) : IConsumer<Message>
{
    public async Task Consume(ConsumeContext<Message> context)
    {
        try
        {
            await messageService.SendAsync(context.Message, context.CancellationToken);
            logger.LogInformation("Message {Message} has been send.", context.Message);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to process element from queue - {Message} with {CorrelationId}. {ExceptionMessage} {Exception}",
                context.Message ,context.CorrelationId, ex.Message, ex);
            throw;
        }
    }
}