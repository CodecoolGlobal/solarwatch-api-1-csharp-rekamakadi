using System.Net;
using DefaultNamespace;

namespace SolarWatch.Services;

public class LatLonProvider : ILatLonProvider
{
    private readonly ILogger<LatLonProvider> _logger;

    public LatLonProvider(ILogger<LatLonProvider> logger)
    {
        _logger = logger;
    }

    public string GetCurrent(string cityName)
    {
        var apiKey = "1688e0e4037c768ddf90516fb972cbb9";
        var url = $"https://api.openweathermap.org/geo/1.0/direct?q={cityName}&limit=1&appid={apiKey}";
        var client = new WebClient();
        
        _logger.LogInformation("Calling Geocoding API with url: {url}", url);
        return client.DownloadString(url);
    }
}