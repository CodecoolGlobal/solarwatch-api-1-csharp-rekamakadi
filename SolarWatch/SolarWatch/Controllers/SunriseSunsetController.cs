using System.ComponentModel.DataAnnotations;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly ILatLonProvider _latLonProvider;

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider, ILatLonProvider latLonProvider)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _latLonProvider = latLonProvider;
    }

    [HttpGet("GetCurrent")]
    public ActionResult<SunriseSunset> GetCurrent([Required]DateTime date, [Required]string cityName)
    {
        string formattedDate = date.ToString("yyyy'-'M'-'d");
        try
        {
            var latLonData = _latLonProvider.GetCurrent(cityName);
            var lat = _jsonProcessor.ProcessLat(latLonData);
            var lon = _jsonProcessor.ProcessLon(latLonData);
            
            try
            {
                var sunsetSunriseData = _sunriseSunsetProvider.GetCurrent(formattedDate, lat, lon);
                return Ok(_jsonProcessor.ProcessSunriseSunset(sunsetSunriseData));
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