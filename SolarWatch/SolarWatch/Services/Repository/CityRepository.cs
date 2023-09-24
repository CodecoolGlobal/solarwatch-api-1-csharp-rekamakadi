using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

public class CityRepository : ICityRepository
{
    public City? GetById(int id)
    {
        using var dbContext = new SolarWatchApiContext();
        return dbContext.Cities.FirstOrDefault(c => c.Id == id);
    }

    public IEnumerable<City> GetAll()
    {
        using var dbContext = new SolarWatchApiContext();
        return dbContext.Cities.ToList();
    }

    public void Add(City city)
    {
        using var dbContext = new SolarWatchApiContext();
        dbContext.Add(city);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        using var dbContext = new SolarWatchApiContext();
        var city = dbContext.Cities.FirstOrDefault(c => c.Id == id);
        if (city == null)
        {
            throw new Exception($"There is no City in DB with id: {id}");
        }
        dbContext.Remove(city);
    }

    public void Update(City city)
    {
        using var dbContext = new SolarWatchApiContext();
        dbContext.Update(city);
        dbContext.SaveChanges();
    }

    public City? GetByName(string cityName)
    {
        using var dbContext = new SolarWatchApiContext();
        return dbContext.Cities.FirstOrDefault(c => c.Name == cityName);
    }
}