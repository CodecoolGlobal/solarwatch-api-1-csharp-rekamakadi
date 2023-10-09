using System.ComponentModel.DataAnnotations;
using DefaultNamespace;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SolarWatch.Data;
using SolarWatch.Services;
using SolarWatch.Services.Repository;

namespace SolarWatch.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;
    private readonly ICityProvider _cityProvider;
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

    public SunriseSunsetController(ILogger<SunriseSunsetController> logger, IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider, ICityProvider cityProvider, ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
        _cityProvider = cityProvider;
        _cityRepository = cityRepository;
        _sunriseSunsetRepository = sunriseSunsetRepository;
    }

    [HttpGet("GetCurrent")]
    public async Task<ActionResult<SunriseSunset>> GetCurrent([Required]DateTime date, [Required]string cityName)
    {
        string formattedDate = date.ToString("yyyy'-'M'-'d");
        var city = await _cityRepository.GetByNameAsync(cityName);
        if (city == null)
        {
            try
            {
                var cityData = await _cityProvider.GetCurrent(cityName);
                city = _jsonProcessor.ProcessCity(cityData);
                _cityRepository.AddAsync(city);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting city data");
                return NotFound("Error getting city data");
            }
        }

        var sunriseSunset = await _sunriseSunsetRepository.GetByDateAndCityIdAsync(date, city.Id);

        if (sunriseSunset == null)
        {
            try
            {
                var sunsetSunriseData = await _sunriseSunsetProvider.GetCurrent(formattedDate, city.Latitude, city.Longitude);
                sunriseSunset = _jsonProcessor.ProcessSunriseSunset(sunsetSunriseData, formattedDate, city.Id);
                _sunriseSunsetRepository.AddAsync(sunriseSunset);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting sunsetSunrise data");
                return NotFound("Error getting sunsetSunrise data");
            }
        }

        return Ok(sunriseSunset);
    }
}