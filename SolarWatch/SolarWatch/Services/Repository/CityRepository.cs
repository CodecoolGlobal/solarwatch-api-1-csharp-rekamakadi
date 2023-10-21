using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Controllers;
using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

public class CityRepository : ICityRepository
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ICityProvider _cityProvider;

    public CityRepository(ILogger<SunriseSunsetController> logger, IJsonProcessor jsonProcessor, ICityProvider cityProvider)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _cityProvider = cityProvider;
    }

    public async Task<City> GetByIdAsync(int id)
    {
        await using var dbContext = new SolarWatchApiContext();
        var city = await dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
        if (city == null)
        {
            _logger.LogError($"There is no City in DB with id: {id}");
            throw new Exception($"There is no City in DB with id: {id}");
        }

        return city;
    }

    public async Task<IEnumerable<City>> GetAllAsync()
    {
        await using var dbContext = new SolarWatchApiContext();
        return await dbContext.Cities.ToListAsync();
    }

    public async void AddAsync(City city)
    {
        await using var dbContext = new SolarWatchApiContext();
        await dbContext.AddAsync(city);
        await dbContext.SaveChangesAsync();
    }

    public async void DeleteAsync(int id)
    {
        await using var dbContext = new SolarWatchApiContext();
        var city = await dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
        if (city == null)
        {
            _logger.LogError($"There is no City in DB with id: {id}");
            throw new Exception($"There is no City in DB with id: {id}");
        }
        dbContext.Remove(city);
        await dbContext.SaveChangesAsync();
    }

    public async void UpdateAsync(int id, City request)
    {
        await using var dbContext = new SolarWatchApiContext();
        var city = await dbContext.Cities.FirstOrDefaultAsync(c => c.Id == id);
        if (city == null)
        {
            _logger.LogError($"There is no City in DB with id: {id}");
            throw new Exception($"There is no City in DB with id: {id}");
        }

        city.Update(request);
        dbContext.Update(city);
        await dbContext.SaveChangesAsync();
    }

    public async Task<City> GetByNameAsync(string cityName)
    {
        await using var dbContext = new SolarWatchApiContext();
        var city = await dbContext.Cities.FirstOrDefaultAsync(c => c.Name == cityName);
        if (city is null)
        {try
            {
                var cityData = await _cityProvider.GetCurrent(cityName);
                city = _jsonProcessor.ProcessCity(cityData);
                AddAsync(city);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting city data");
                throw new Exception($"Error getting city data");
            }
        }

        return city;
    }
}