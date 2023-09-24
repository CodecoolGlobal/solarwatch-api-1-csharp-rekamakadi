namespace SolarWatch.Services.Repository;

public interface ISunriseSunsetRepository : IGenericRepository<SunriseSunset>
{
    public SunriseSunset? GetByDateAndCityId(DateTime date, int cityId);
}