using SolarWatch.Data;

namespace SolarWatch.Services.Repository;

public class SunriseSunsetRepository : ISunriseSunsetRepository
{
    public SunriseSunset? GetById(int id)
    {
        using var dbContext = new SolarWatchApiContext();
        return dbContext.SunriseSunsets.FirstOrDefault(s => s.Id == id);
    }

    public IEnumerable<SunriseSunset> GetAll()
    {
        using var dbContext = new SolarWatchApiContext();
        return dbContext.SunriseSunsets.ToList();
    }

    public void Add(SunriseSunset sunriseSunset)
    {
        using var dbContext = new SolarWatchApiContext();
        dbContext.SunriseSunsets.Add(sunriseSunset);
        dbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        using var dbContext = new SolarWatchApiContext();
        var sunriseSunset = dbContext.SunriseSunsets.FirstOrDefault(s => s.Id == id);
        dbContext.SunriseSunsets.Remove(sunriseSunset);
        dbContext.SaveChanges();
    }

    public void Update(SunriseSunset sunriseSunset)
    {
        using var dbContext = new SolarWatchApiContext();
        dbContext.Update(sunriseSunset);
        dbContext.SaveChanges();
    }

    public SunriseSunset? GetByDateAndCityId(DateTime date, int cityId)
    {
        using var dbContext = new SolarWatchApiContext();
        return dbContext.SunriseSunsets.FirstOrDefault(s => s.ActualDate == date && s.CityId == cityId);
    }
}