using EVS.App.Domain.Abstractions.Repositories;

namespace EVS.App.Infrastructure.Cleaning;

public sealed class DeletedEventsCleanerService(
    IEventRepository eventRepository,
    ILogger<DeletedEventsCleanerService> logger) : BackgroundService
{
    private Timer? _timer;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(_ => _ = DoWorkAsync(stoppingToken), null, TimeSpan.Zero, TimeSpan.FromHours(12));

        return Task.CompletedTask;
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken = default)
    {
        if (!await _semaphore.WaitAsync(0, stoppingToken))
            return;

        try
        {
            await eventRepository.DeleteEndedOlderThanDay(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to execute background operation due to exception {ExceptionMessage} {Exception}",
                ex.Message, ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return base.StopAsync(cancellationToken);
    }

    
    public override void Dispose()
    {
        _timer?.Dispose();
        _semaphore.Dispose();
        base.Dispose();
    }
}