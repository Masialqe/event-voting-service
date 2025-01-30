using EVS.App.Application.Abstractions;

namespace EVS.App.Infrastructure.Notifiers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotifiers(this IServiceCollection services)
    {
        services.AddScoped<IEventNotifier, SignalRNotifier>();
        return services;
    }
}