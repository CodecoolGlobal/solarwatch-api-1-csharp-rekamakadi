namespace SolarWatch.Services.Repository;

public interface ICityRepository : IGenericRepository<City>
{
    public City? GetByName(string cityName);
}