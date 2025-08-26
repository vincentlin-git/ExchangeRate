namespace RateProviders.Interfaces;

public interface IExchangeRateProvider
{
    Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default);
}