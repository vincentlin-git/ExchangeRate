using RateProviders.Models;
using RateProviders.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Configure ExchangeRateApi options
builder.Services.Configure<ExchangeRateApiOptions>(
    builder.Configuration.GetSection(ExchangeRateApiOptions.SectionName));

builder.Services.AddExchangeRateProviders();


// API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Exchange Service API", 
        Version = "v1"
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Service API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger at root
    });
}
else
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.MapControllers();

app.Run();