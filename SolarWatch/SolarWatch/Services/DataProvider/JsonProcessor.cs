using System.Globalization;
using System.Text.Json;


namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public SunriseSunset ProcessSunriseSunset(string data, string formattedDate, int cityId)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        SunriseSunset sunriseSunset = new SunriseSunset
        {
            ActualDate = DateTime.Parse(formattedDate),
            Sunrise = GetDateTimeFromString(results.GetProperty("sunrise").ToString()),
            Sunset = GetDateTimeFromString(results.GetProperty("sunset").ToString()),
            SolarNoon = GetDateTimeFromString(results.GetProperty("solar_noon").ToString()),
            CityId = cityId
        };

        return sunriseSunset;
    }
    public City ProcessCity(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        
        City city = new City
        {
            Name = json.RootElement[0].GetProperty("name").ToString(),
            Latitude = json.RootElement[0].GetProperty("lat").GetDouble(),
            Longitude = json.RootElement[0].GetProperty("lon").GetDouble(),
            State = json.RootElement[0].GetProperty("state").ToString(),
            Country = json.RootElement[0].GetProperty("country").ToString(),
        };

        return city;
    }
    
    private static DateTime GetDateTimeFromString(string dateToConvert)
    {
        string timeFormat = "h:mm:ss tt";
    
        return DateTime.ParseExact(dateToConvert, timeFormat, CultureInfo.InvariantCulture);
    }
    
}