namespace DefaultNamespace;

public interface ILatLonProvider
{
    Task<string> GetCurrent(string cityName);
}