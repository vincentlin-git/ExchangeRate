using System.ComponentModel.DataAnnotations;

namespace RateProviders.Models;

public class ExchangeRateApiOptions
{
    public const string SectionName = "RateProviders";
    
    [Required]
    [Url]
    public string ExchangeRateApiUrl { get; init; } = string.Empty;
    
    [Required]
    [MinLength(1)]
    public string ApiKey { get; init; } = string.Empty;
    
    [Range(1, 300)]
    public int TimeoutSeconds { get; init; } = 10;
}