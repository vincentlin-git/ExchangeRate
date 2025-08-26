namespace ExchangeServiceAPI.Models;

public class ExchangeResponse
{
    public decimal Amount { get; init; }
    
    public string InputCurrency { get; init; } = string.Empty;
    
    public string OutputCurrency { get; init; } = string.Empty;
    
    public decimal Value { get; init; }
}