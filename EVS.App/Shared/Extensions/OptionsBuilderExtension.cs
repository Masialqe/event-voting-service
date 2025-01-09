using Microsoft.Extensions.Options;

namespace EVS.App.Shared.Extensions;

public static class OptionsBuilderExtension
{
    /// <summary>
    /// Registers options with validation and configuration binding.
    /// </summary>
    /// <typeparam name="TValue">The type of options to register.</typeparam>
    /// <param name="services">The service collection to register the options with.</param>
    /// <param name="sectionName">The configuration section name to bind the options to.</param>
    /// <returns>The <see cref="OptionsBuilder{TValue}"/> instance.</returns>
    public static OptionsBuilder<TValue> AddConfiguredOptions<TValue>(this IServiceCollection services,
        string sectionName) where TValue : class, IAppOptions
    {
        return services.AddOptions<TValue>()
            .BindConfiguration(sectionName)    
            .ValidateDataAnnotations()         
            .ValidateOnStart();                
    }
}
