using System.Text.Json;
using Microsoft.Extensions.Options;
using RateProviders.Exceptions;
using RateProviders.Interfaces;
using RateProviders.Models;

namespace RateProviders.Provider_ExchangeRateApi;

public class ExchangeRateApiProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ExchangeRateApiOptions _options;

    public ExchangeRateApiProvider(HttpClient httpClient, IOptions<ExchangeRateApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
    }

    public async Task<decimal> GetExchangeRateAsync(string fromCurrency, string toCurrency, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{_options.ExchangeRateApiUrl}/{_options.ApiKey}/latest/{fromCurrency.ToUpperInvariant()}";
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new RateProviderException($"External API returned status code: {response.StatusCode}");
            }

            var jsonContent = await response.Content.ReadAsStringAsync(cancellationToken);
            var exchangeRateResponse = JsonSerializer.Deserialize<ExchangeRateApiResponse>(jsonContent);

            if (exchangeRateResponse == null)
            {
                throw new RateProviderException("Failed to deserialize exchange rate response");
            }

            if (exchangeRateResponse.Result != "success")
            {
                throw new RateProviderException($"Exchange rate API error: {exchangeRateResponse.ErrorType ?? "Unknown error"}");
            }

            if (!exchangeRateResponse.ConversionRates.TryGetValue(toCurrency.ToUpperInvariant(), out var rate))
            {
                throw new RateProviderException($"Exchange rate not found for currency pair {fromCurrency} to {toCurrency}");
            }

            return rate;
        }
        catch (HttpRequestException ex)
        {
            throw new RateProviderException("Failed to retrieve exchange rate due to network error", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new RateProviderException("Exchange rate request timed out", ex);
        }
        catch (JsonException ex)
        {
            throw new RateProviderException("Failed to parse exchange rate response", ex);
        }
    }
}