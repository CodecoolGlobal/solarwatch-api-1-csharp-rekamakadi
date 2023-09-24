using System.Net;
using DefaultNamespace;

namespace SolarWatch.Services;

public class CityProvider : ICityProvider
{
    private readonly ILogger<CityProvider> _logger;

    public CityProvider(ILogger<CityProvider> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetCurrent(string cityName)
    {
        var apiKey = "1688e0e4037c768ddf90516fb972cbb9";
        var url = $"https://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";
        var client = new HttpClient();
        
        _logger.LogInformation("Calling Geocoding API with url: {url}", url);

        var response = await client.GetAsync(url);
        
        return await response.Content.ReadAsStringAsync();
    }
}