namespace DefaultNamespace;

public interface ISunriseSunsetProvider
{
    public Task<string> GetCurrent(string date, double lat, double lon);
}