namespace DefaultNamespace;

public interface ISunriseSunsetProvider
{
    public string GetCurrent(string date, double lat, double lon);
}