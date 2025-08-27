using Microsoft.Extensions.DependencyInjection;
using RateProviders.Factories;
using RateProviders.Interfaces;
using RateProviders.Provider_ExchangeRateApi;

namespace RateProviders.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExchangeRateProviders(this IServiceCollection services)
    {
        // Configure named HTTP Client for exchange rate providers
        services.AddHttpClient<ExchangeRateApiProvider>(client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "ExchangeService/1.0");
        });
        
        // Register the actual provider
        services.AddScoped<ExchangeRateApiProvider>();
        
        // Register factory
        services.AddScoped<IExchangeRateProviderFactory, ExchangeRateProviderFactory>();
        
        // Register default provider through factory
        services.AddScoped<IExchangeRateProvider>(provider => 
            provider.GetRequiredService<IExchangeRateProviderFactory>().CreateDefaultProvider());
        
        return services;
    }
}
