using EVS.App.Application.Abstractions;
using EVS.App.Shared.Hub;

namespace EVS.App.Infrastructure.Notifiers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotifiers(this IServiceCollection services)
    {
        services.AddScoped<IEventNotifier, SignalRNotifier>();
        services.AddScoped<SignalRService>();
        
        return services;
    }
}