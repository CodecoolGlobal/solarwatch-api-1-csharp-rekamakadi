namespace SolarWatch.Services.Repository;

public interface ISunriseSunsetRepository : IGenericRepository<SunriseSunset>
{
    public Task<SunriseSunset> GetByDateAndCityAsync(DateTime date, City city);
}