using System.Text.Json.Serialization;

namespace RateProviders.Models;

public class ExchangeRateApiResponse
{
    [JsonPropertyName("result")]
    public string Result { get; init; } = string.Empty;
    
    [JsonPropertyName("base_code")]
    public string BaseCode { get; init; } = string.Empty;
    
    [JsonPropertyName("conversion_rates")]
    public Dictionary<string, decimal> ConversionRates { get; init; } = new();
    
    [JsonPropertyName("error-type")]
    public string? ErrorType { get; init; }
}