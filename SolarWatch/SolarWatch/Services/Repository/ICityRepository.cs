namespace SolarWatch.Services.Repository;

public interface ICityRepository : IGenericRepository<City>
{
    public Task<City> GetByNameAsync(string cityName);
}