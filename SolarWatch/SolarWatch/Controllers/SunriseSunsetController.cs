using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Services.Repository;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class SunriseSunsetController : ControllerBase
{
    private readonly ICityRepository _cityRepository;
    private readonly ISunriseSunsetRepository _sunriseSunsetRepository;

    public SunriseSunsetController(ICityRepository cityRepository, ISunriseSunsetRepository sunriseSunsetRepository)
    {
        _cityRepository = cityRepository;
        _sunriseSunsetRepository = sunriseSunsetRepository;
    }

    
    [HttpGet("/GetAllCities")]
    public async Task<ActionResult<List<City>>> GetAllCities()
    {
        return Ok(await _cityRepository.GetAllAsync());
    }
    
    [HttpGet("/GetOrAddByCityName")]
    public async Task<ActionResult<SunriseSunset>> GetOrAddByCityName([Required]string cityName)
    {
        return Ok(await _cityRepository.GetByNameAsync(cityName));    
    }
    
    [HttpGet("/GetCity/{id}")]
    public async Task<ActionResult<City>> GetCityById(int id)
    {
        return Ok(await _cityRepository.GetByIdAsync(id));
    }
    
    [HttpPost("/AddCity")]
    public ActionResult<City> AddCity(City city)
    {
        _cityRepository.AddAsync(city);
        return CreatedAtAction(nameof(AddCity), new { id = city.Id }, city);
    }
    
    [HttpPost("/UpdateCity/{id}")]
    public IActionResult UpdateCity(int id, [FromBody] City request)
    {
        try
        {
            _cityRepository.UpdateAsync(id, request);
            return Ok($"The current city on id: {id} is successfully updated to {request}.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"{e}");        }
        
    }
    
    [HttpDelete("/DeleteCityById/{id}")]
    public IActionResult DeleteCity(int id)
    {
        try
        {
            _cityRepository.DeleteAsync(id);
            return Ok($"The current city on id: {id} is successfully deleted.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"{e}");
        }
        
    }
    
    
    [HttpGet("/GetAllSunsetSunrises")]
    public async Task<ActionResult<List<SunriseSunset>>> GetAllSunsetSunrises()
    {
        return Ok(await _sunriseSunsetRepository.GetAllAsync());
    }
    
    [HttpGet("GetOrAddByDateAndCityName")]
    public async Task<ActionResult<SunriseSunset>> GetOrAddByDateAndCityName([Required]DateTime date, [Required]string cityName)
    {
        var city = await _cityRepository.GetByNameAsync(cityName);
        var sunriseSunset = await _sunriseSunsetRepository.GetByDateAndCityAsync(date, city);

        return Ok(sunriseSunset);    
    }
    
    [HttpGet("/GetSsById/{id}")]
    public async Task<ActionResult<City>> GetSsById(int id)
    {
        return Ok(await _sunriseSunsetRepository.GetByIdAsync(id));
    }
    
    [HttpPost("/AddSunriseSunset")]
    public ActionResult<SunriseSunset> AddSunriseSunset(SunriseSunset sunriseSunset)
    {
        _sunriseSunsetRepository.AddAsync(sunriseSunset);
        return CreatedAtAction(nameof(AddSunriseSunset), new { id = sunriseSunset.Id }, sunriseSunset);
    }

    [HttpPost("/UpdateSs/{id}")]
    public IActionResult UpdateSunriseSunset(int id, [FromBody] SunriseSunset request)
    {
        try
        {
            _sunriseSunsetRepository.UpdateAsync(id, request);
            return Ok($"The current sunriseSunset on id: {id} is successfully updated to {request}.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"{e}");
        }
    }

    [HttpDelete("/DeleteSsById/{id}")]
    public IActionResult DeleteSunriseSunset(int id)
    {
        try
        {
            _sunriseSunsetRepository.DeleteAsync(id);
            return Ok($"The current sunriseSunset on id: {id} is successfully deleted.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception($"{e}");
        }
        
    }
}