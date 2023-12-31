﻿using System.Net;
using DefaultNamespace;

namespace SolarWatch.Services;

public class SunriseSunsetProvider : ISunriseSunsetProvider
{
    private readonly ILogger<SunriseSunsetProvider> _logger;

    public SunriseSunsetProvider(ILogger<SunriseSunsetProvider> logger)
    {
        _logger = logger;
    }

    public async Task<string> GetCurrent(string date, double lat, double lon)
    {
        var url = $"https://api.sunrise-sunset.org/json?lat={lat}&lng={lon}&date={date}";
        var client = new HttpClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);

        var response = await client.GetAsync(url);

        return await response.Content.ReadAsStringAsync();
    }
}