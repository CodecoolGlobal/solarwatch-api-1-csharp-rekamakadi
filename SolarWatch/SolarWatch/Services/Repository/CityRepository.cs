using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

public class InClassName
{
    public InClassName(int id)
    {
        Id = id;
    }

    public int Id { get; private set; }
}

public class CityRepository : ICityRepository
{
    public async Task<City> GetByIdAsync(InClassName inClassName)
    {
        await using var dbContext = new SolarWatchApiContext();
        return await dbContext.Cities.FirstOrDefaultAsync(c => c.Id == inClassName.Id);
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
            throw new Exception($"There is no City in DB with id: {id}");
        }
        dbContext.Remove(city);
    }

    public async void UpdateAsync(City city)
    {
        await using var dbContext = new SolarWatchApiContext();
        dbContext.Update(city);
        await dbContext.SaveChangesAsync();
    }

    public async Task<City> GetByNameAsync(string cityName)
    {
        await using var dbContext = new SolarWatchApiContext();
        return await dbContext.Cities.FirstOrDefaultAsync(c => c.Name == cityName);
    }
}