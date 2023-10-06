namespace DefaultNamespace;

public interface ICityProvider
{
    Task<string> GetCurrent(string cityName);
}