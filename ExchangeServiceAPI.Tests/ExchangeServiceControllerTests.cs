using ExchangeServiceAPI.Controllers;
using ExchangeServiceAPI.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RateProviders.Exceptions;
using RateProviders.Interfaces;

namespace ExchangeServiceAPI.Tests;

[TestFixture]
public class ExchangeServiceControllerTests
{
    private Mock<IExchangeRateProvider> _mockExchangeRateProvider = null!;
    private Mock<ILogger<ExchangeServiceController>> _mockLogger = null!;
    private ExchangeServiceController _controller = null!;

    [SetUp]
    public void SetUp()
    {
        _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();
        _mockLogger = new Mock<ILogger<ExchangeServiceController>>();
        _controller = new ExchangeServiceController(_mockExchangeRateProvider.Object, _mockLogger.Object);
    }

    [Test]
    public async Task ConvertCurrency_ValidRequest_ReturnsOkWithExchangeResponse()
    {
        // Arrange
        var request = new ExchangeRequest
        {
            Amount = 100m,
            InputCurrency = "AUD",
            OutputCurrency = "USD"
        };
        
        var expectedRate = 0.65m;
        _mockExchangeRateProvider
            .Setup(x => x.GetExchangeRateAsync("AUD", "USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRate);

        // Act
        var result = await _controller.ConvertCurrency(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.Value.Should().BeOfType<ExchangeResponse>();
        
        var response = (ExchangeResponse)okResult.Value!;
        response.Amount.Should().Be(100m);
        response.InputCurrency.Should().Be("AUD");
        response.OutputCurrency.Should().Be("USD");
        response.Value.Should().Be(65m);
    }

    [Test]
    public async Task ConvertCurrency_RateProviderException_ReturnsProblemDetails()
    {
        // Arrange
        var request = new ExchangeRequest
        {
            Amount = 100m,
            InputCurrency = "USD",
            OutputCurrency = "XYZ"
        };
        
        _mockExchangeRateProvider
            .Setup(x => x.GetExchangeRateAsync("USD", "XYZ", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RateProviderException("Currency XYZ not found"));

        // Act
        var result = await _controller.ConvertCurrency(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500);
        objectResult.Value.Should().BeOfType<ProblemDetails>();
    }
}
