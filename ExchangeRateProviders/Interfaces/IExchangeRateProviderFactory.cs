namespace RateProviders.Interfaces;

public interface IExchangeRateProviderFactory
{
    IExchangeRateProvider CreateProvider(string providerName);
    IExchangeRateProvider CreateDefaultProvider();
}
