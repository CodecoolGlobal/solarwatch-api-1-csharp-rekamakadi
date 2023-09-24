using System.ComponentModel.DataAnnotations;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Data;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly ICityProvider _cityProvider;

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider, ICityProvider cityProvider)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _cityProvider = cityProvider;
    }

    [HttpGet("GetCurrent")]
    public async Task<ActionResult<SunriseSunset>> GetCurrent([Required]DateTime date, [Required]string cityName)
    {
        await using var dbContext = new SolarWatchApiContext();
        string formattedDate = date.ToString("yyyy'-'M'-'d");
        var city = dbContext.Cities.FirstOrDefault(c => c.Name == cityName);
        if (city == null)
        {
            return NotFound($"City {cityName}  not found");
        }
        
        try
        {
            var cityData = await _cityProvider.GetCurrent(cityName);
            var lat = _jsonProcessor.ProcessCity(cityData).Latitude;
            var lon = _jsonProcessor.ProcessCity(cityData).Longitude;
            
            try
            {
                var sunsetSunriseData = await _sunriseSunsetProvider.GetCurrent(formattedDate, lat, lon);
                return  Ok( _jsonProcessor.ProcessSunriseSunset(sunsetSunriseData, formattedDate));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting sunsetSunrise data");
                return NotFound("Error getting sunsetSunrise data");
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lat, lon data");
            return NotFound("Error getting lat, lon data");
        }
    }
}