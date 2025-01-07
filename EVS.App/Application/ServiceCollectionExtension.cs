using EVS.App.Application.UseCases.Voters.CreateVoter;
using EVS.App.Domain.Voters;

namespace EVS.App.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<CreateVoterHandler>();
        
        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<VoterService>();
        
        return services;
    }
}