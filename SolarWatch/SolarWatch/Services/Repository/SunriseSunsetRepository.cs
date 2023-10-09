using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    public async Task<SunriseSunset> GetByIdAsync(InClassName inClassName)
    {
        await using var dbContext = new SolarWatchApiContext();
        return await dbContext.SunriseSunsets.FirstOrDefaultAsync(s => s.Id == inClassName.Id);
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
        var sunriseSunset = await dbContext.SunriseSunsets.FirstOrDefaultAsync(s => s.Id == id);
        dbContext.SunriseSunsets.Remove(sunriseSunset);
        await dbContext.SaveChangesAsync();
    }

    public async void UpdateAsync(SunriseSunset sunriseSunset)
    {
        await using var dbContext = new SolarWatchApiContext();
        dbContext.Update(sunriseSunset);
        await dbContext.SaveChangesAsync();
    }

    public async Task<SunriseSunset> GetByDateAndCityIdAsync(DateTime date, int cityId)
    {
        await using var dbContext = new SolarWatchApiContext();
        return dbContext.SunriseSunsets.FirstOrDefault(s => s.ActualDate == date && s.CityId == cityId);
    }
}