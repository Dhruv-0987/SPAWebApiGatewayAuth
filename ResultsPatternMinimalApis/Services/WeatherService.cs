using ResultsPatternMinimalApis.Models;
using ResultsPatternMinimalApis.Models.ResultsTypes;

namespace ResultsPatternMinimalApis;

public class WeatherService
{
    public Result<WeatherForecast[]> GetForecast()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        
        var result = new Result<WeatherForecast[]>(true, Error.None, forecast);
        return result;
    }
}