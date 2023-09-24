using SolarWatch;

namespace SolarWatch.Services;

public interface IJsonProcessor
{
    SunriseSunset ProcessSunriseSunset(string data, string formattedDate, int cityId);
    City ProcessCity(string data);
}