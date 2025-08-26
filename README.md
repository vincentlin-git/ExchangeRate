
**Requirements:** .NET 9.0 SDK

```bash
cd ExchangeRate
cd ExchangeServiceAPI
dotnet restore
dotnet run

# Access API at https://localhost:5000
# Swagger UI at http://localhost:5000/index.html
```

## Usage

```bash
vincentlin@ ~ %  
curl -X 'POST' \
'http://localhost:5000/ExchangeService' \
-H 'accept: text/plain' \
-H 'Content-Type: application/json' \
-d '{
"amount": 5,
"inputCurrency": "AUD",
"outputCurrency": "USD"
}'

# Response:
{"amount":5,"inputCurrency":"AUD","outputCurrency":"USD","value":3.24}%   

```

## Architecture

- **ExchangeServiceAPI**: Web API with controllers and models
- **ExchangeRateProviders**: Pluggable exchange rate provider implementations

## Configuration

```json
{
  "ExchangeRateApi": {
    "ExchangeRateApiUrl": "https://v6.exchangerate-api.com/v6",
    "ApiKey": "Vincent registered a free API key from exchangerate-api.com, configured at appsettings.json",
    "TimeoutSeconds": 10
  }
}
```

## Extending

**Add new exchange rate providers:**
1. Implement `IExchangeRateProvider`
2. Register in `Program.cs` DI container
3. Add configuration to `appsettings.json`


## Production Considerations

- **Resilience**: Add retry policies
- **Monitoring**: Health checks
