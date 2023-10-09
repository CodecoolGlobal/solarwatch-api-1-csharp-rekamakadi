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

        var sunriseSunset = await _sunriseSunsetRepository.GetByDateAndCityIdAsync(date, city.Id);

        return Ok(sunriseSunset);
    }
    
    [HttpPost("/AddCity"), Authorize(Roles="Admin")]
    public ActionResult<City> AddCity(City city)
    {
        _cityRepository.AddAsync(city);
        return CreatedAtAction(nameof(AddCity), new { id = city.Id }, city);
    }

    [HttpPost("/AddSunriseSunset"), Authorize(Roles="Admin")]
    public ActionResult<SunriseSunset> AddSunriseSunset(SunriseSunset sunriseSunset)
    {
        _sunriseSunsetRepository.AddAsync(sunriseSunset);
        return CreatedAtAction(nameof(AddSunriseSunset), new { id = sunriseSunset.Id }, sunriseSunset);
    }

    [HttpGet("{id}"), Authorize(Roles="User, Admin")]
    public ActionResult<City> GetCityById(int id)
    {
        var city = _cityRepository.GetByIdAsync(id);
       
        return Ok(city);
    }

    [HttpGet("{cityName}/{date}"), Authorize(Roles="User, Admin")]
    public ActionResult<SunriseSunset> GetSunriseSunset(string cityName, DateTime date)
    {
        var cityId =  _cityRepository.GetByNameAsync(cityName).Id;
        var sunriseSunset = _sunriseSunsetRepository.GetByDateAndCityIdAsync(date, cityId);
        return Ok(sunriseSunset);
    }

    [HttpPut("{id}"), Authorize(Roles="Admin")]
    public IActionResult UpdateCity(City city)
    {
        _cityRepository.UpdateAsync(city);

        return NoContent();
    }
[HttpPut("{cityName}/{date}"), Authorize(Roles="Admin")]
    public IActionResult UpdateSunriseSunset(SunriseSunset sunriseSunset)
    {
        _sunriseSunsetRepository.UpdateAsync(sunriseSunset);

        return Ok(sunriseSunset);
    }

    [HttpDelete("{id}"), Authorize(Roles="Admin")]
    public IActionResult DeleteCity(int cityId)
    {
        var city = _cityRepository.GetByIdAsync(cityId);
        
        _cityRepository.DeleteAsync(cityId);

        return NoContent();
    }

    [HttpDelete("{cityName}/{date}"), Authorize(Roles="Admin")]
    public IActionResult DeleteSunriseSunset(string cityName, DateTime date)
    {
        var cityId =  _cityRepository.GetByNameAsync(cityName).Id;
        var sunriseSunset = _sunriseSunsetRepository.GetByDateAndCityIdAsync(date, cityId);
        
        _sunriseSunsetRepository.DeleteAsync(sunriseSunset.Id);

        return Ok($"The current sunrise-sunset data on id: {sunriseSunset.Id}, city: {cityName}, date: {date} is successfully deleted.");
    }
}