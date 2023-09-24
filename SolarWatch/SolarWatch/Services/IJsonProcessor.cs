using SolarWatch;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    SunriseSunset ProcessSunriseSunset(string data, string formattedDate);
    City ProcessCity(string data);
}