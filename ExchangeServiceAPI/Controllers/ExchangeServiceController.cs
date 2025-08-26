using RateProviders.Exceptions;
using RateProviders.Interfaces;
using ExchangeServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeServiceAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeServiceController(IExchangeRateProvider exchangeRateProvider, ILogger<ExchangeServiceController> logger) : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ExchangeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConvertCurrency([FromBody] ExchangeRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var fromCurrency = request.InputCurrency.ToUpperInvariant();
        var toCurrency = request.OutputCurrency.ToUpperInvariant();

        try
        {
            logger.LogInformation("Processing exchange request: {FromCurrency} to {ToCurrency} for amount {Amount}", 
                fromCurrency, toCurrency, request.Amount);

            var exchangeRate = await exchangeRateProvider.GetExchangeRateAsync(
                fromCurrency, 
                toCurrency, 
                cancellationToken);

            var convertedValue = Math.Round(request.Amount * exchangeRate, 2);

            var response = new ExchangeResponse
            {
                Amount = request.Amount,
                InputCurrency = fromCurrency,
                OutputCurrency = toCurrency,
                Value = convertedValue
            };

            logger.LogInformation("Exchange completed successfully. Rate: {Rate}, Converted Value: {Value}", 
                exchangeRate, convertedValue);

            return Ok(response);
        }
        catch (RateProviderException ex)
        {
            logger.LogError(ex, "Exchange rate service error occurred");
            return Problem(
                title: "Exchange Rate Service Error",
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred during currency conversion");
            return Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
}