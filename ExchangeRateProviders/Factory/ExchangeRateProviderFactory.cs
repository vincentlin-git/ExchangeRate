using Microsoft.Extensions.DependencyInjection;
using RateProviders.Interfaces;
using RateProviders.Provider_ExchangeRateApi;

namespace RateProviders.Factories;

public class ExchangeRateProviderFactory(IServiceProvider serviceProvider) : IExchangeRateProviderFactory
{
    public IExchangeRateProvider CreateProvider(string providerName)
    {
        return providerName.ToLowerInvariant() switch
        {
            "exchangerateapi" => serviceProvider.GetRequiredService<ExchangeRateApiProvider>(),
            _ => throw new ArgumentException($"Unknown provider: {providerName}", nameof(providerName))
        };
    }

    public IExchangeRateProvider CreateDefaultProvider()
    {
        return CreateProvider("exchangerateapi");
    }
}
