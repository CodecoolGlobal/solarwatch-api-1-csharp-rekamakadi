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

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
    }

    [HttpGet("GetCurrent")]
    public ActionResult<SunriseSunset> GetCurrent()
    {
        // TODO: Ide egy városnév alapú api hívás alapján kellene a lat, lon
        //Budapest lat & lon
        var lat = 47.497913;
        var lon = 19.040236;
        // string formattedDate = date.ToString("yyyy'-'M'-'d");

        try
        {
            var sunsetSunriseData = _sunriseSunsetProvider.GetCurrent("2023-09-07", lat, lon);
            return Ok(_jsonProcessor.ProcessSunriseSunset(sunsetSunriseData));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunsetSunrise data");
            return NotFound("Error getting sunsetSunrise data");
        }
    }
}