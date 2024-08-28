using Microsoft.AspNetCore.Mvc;
using ResultsPatternMinimalApis.Models;

namespace ResultsPatternMinimalApis;

public static class GetWeatherForecastEndpoint
{
    public static IEndpointRouteBuilder MapGetWeatherForecast(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/weather-forecast", ([FromServices] WeatherService weatherService) =>
            {
                var result = weatherService.GetForecast(); // Assuming GetForecast is async
                return result.ToMinimalApiResult();
            })
            .Produces(StatusCodes.Status200OK, typeof(WeatherForecast[]))
            .Produces(StatusCodes.Status404NotFound);

        return endpoints;
    }
}