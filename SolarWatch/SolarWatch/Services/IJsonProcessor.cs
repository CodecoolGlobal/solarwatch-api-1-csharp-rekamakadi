using SolarWatch;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    SunriseSunset ProcessSunriseSunset(string data);
    double ProcessLat(string data);
    double ProcessLon(string data);
}