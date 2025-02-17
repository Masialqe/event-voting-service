using EVS.App.Application.UseCases.Events.EventHubs;
using EVS.App.Infrastructure.Notifiers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;


namespace EVS.App.Shared.Hub;

public sealed class SignalRService : IAsyncDisposable
{
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<SignalRService> _logger;
    
    public HubConnection HubConnection { get; private set; }
    public readonly Guid InstanceId;

    public SignalRService(NavigationManager navigationManager, ILogger<SignalRService> logger)
    {
        _navigationManager = navigationManager;
        _logger = logger;
        
        InstanceId = Guid.NewGuid();
        
        HubConnection = CreateHubConnection();
    }

    private HubConnection CreateHubConnection()
    {
        try
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri(HubConfig.EVENT_HUB_URL))
                .WithAutomaticReconnect()
                .Build();
            
            return hubConnection;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to establish hub connection. {ExceptionMessage} {Exception}", ex.Message, ex);
            throw;
        }
    }
    
    public async Task StartAsync()
    {
        if(HubConnection is null) HubConnection = CreateHubConnection();
        
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }
    
    public async Task StopAsync()
    {
        if (HubConnection.State == HubConnectionState.Connected)
        {
            await HubConnection.StopAsync();
        }
    }
    
    
    public async ValueTask DisposeAsync()
    {
        await HubConnection.DisposeAsync();
    }
}