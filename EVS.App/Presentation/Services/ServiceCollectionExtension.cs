namespace EVS.App.Presentation.Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddScoped<LocalStorageService>();
        return services;
    }
}