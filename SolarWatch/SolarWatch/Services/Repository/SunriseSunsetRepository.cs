using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Controllers;
using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    private readonly ILogger<SunriseSunsetController> _logger;
    private readonly IJsonProcessor _jsonProcessor;
    private readonly ISunriseSunsetProvider _sunriseSunsetProvider;

    public SunriseSunsetRepository(ILogger<SunriseSunsetController> logger, IJsonProcessor jsonProcessor, ISunriseSunsetProvider sunriseSunsetProvider)
    {
        _logger = logger;
        _jsonProcessor = jsonProcessor;
        _sunriseSunsetProvider = sunriseSunsetProvider;
    }

    public async Task<SunriseSunset> GetByIdAsync(int id)
    {
        await using var dbContext = new SolarWatchApiContext();
        var sunriseSunset = await dbContext.SunriseSunsets.FindAsync(id);
        if (sunriseSunset is null)
        {
            _logger.LogError($"There is no SunriseSunset in DB with id: {id}");
            throw new Exception($"There is no SunriseSunset in DB with id: {id}");
        }

        return sunriseSunset;
    }

    public async Task<IEnumerable<SunriseSunset>> GetAllAsync()
    {
        await using var dbContext = new SolarWatchApiContext();
        return await dbContext.SunriseSunsets.ToListAsync();
    }

    public async void AddAsync(SunriseSunset sunriseSunset)
    {
        await using var dbContext = new SolarWatchApiContext();
        await dbContext.SunriseSunsets.AddAsync(sunriseSunset);
        await dbContext.SaveChangesAsync();
    }

    public async void DeleteAsync(int id)
    {
        await using var dbContext = new SolarWatchApiContext();
        var sunriseSunset = await dbContext.SunriseSunsets.FindAsync(id);
        if (sunriseSunset is null)
        {
            _logger.LogError($"There is no SunriseSunset in DB with id: {id}");
            throw new Exception($"There is no SunriseSunset in DB with id: {id}");
        }
        dbContext.SunriseSunsets.Remove(sunriseSunset);
        await dbContext.SaveChangesAsync();
    }

    public async void UpdateAsync(int id, SunriseSunset request)
    {
        await using var dbContext = new SolarWatchApiContext();
        var sunriseSunset = await dbContext.SunriseSunsets.FindAsync(id);
        if (sunriseSunset is null)
        {
            _logger.LogError($"There is no SunriseSunset in DB with id: {id}");
            throw new Exception($"There is no SunriseSunset in DB with id: {id}");
        }

        sunriseSunset.Update(request);
        dbContext.Update(sunriseSunset);
        await dbContext.SaveChangesAsync();
    }

    public async Task<SunriseSunset> GetByDateAndCityAsync(DateTime date, City city)
    {
        await using var dbContext = new SolarWatchApiContext();
        string formattedDate = date.ToString("yyyy'-'M'-'d");
        var sunriseSunset = await (dbContext.SunriseSunsets.FirstOrDefaultAsync(s => s.ActualDate == date && s.CityId == city.Id));
        
        if (sunriseSunset is null)
        {
            try
            {
                var sunsetSunriseData = await _sunriseSunsetProvider.GetCurrent(formattedDate, city.Latitude, city.Longitude);
                sunriseSunset = _jsonProcessor.ProcessSunriseSunset(sunsetSunriseData, formattedDate, city.Id);
                AddAsync(sunriseSunset);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting sunsetSunrise data");
                throw new Exception("Error getting sunsetSunrise data");
            }
        }

        return sunriseSunset;
    }
}