namespace SolarWatch.Services.Repository;

public interface ISunriseSunsetRepository : IGenericRepository<SunriseSunset>
{
    public Task<SunriseSunset> GetByDateAndCityIdAsync(DateTime date, int cityId);
}