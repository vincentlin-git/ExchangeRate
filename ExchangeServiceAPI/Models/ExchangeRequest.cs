using System.ComponentModel.DataAnnotations;

namespace ExchangeServiceAPI.Models;

public class ExchangeRequest
{
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Input currency is required")]
    [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Currency code must be 3 letters")]
    public required string InputCurrency { get; set; }

    [Required(ErrorMessage = "Output currency is required")]
    [RegularExpression(@"^[A-Za-z]{3}$", ErrorMessage = "Currency code must be 3 letters")]
    public required string OutputCurrency { get; set; }
}
