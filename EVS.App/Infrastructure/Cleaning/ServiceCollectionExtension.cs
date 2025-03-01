namespace EVS.App.Infrastructure.Cleaning;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCleaningServices(this IServiceCollection services)
    {
        services.AddHostedService<DeletedEventsCleanerService>();
        
        return services;
    }
}