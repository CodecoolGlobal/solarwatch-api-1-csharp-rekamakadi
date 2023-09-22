using System.Text.Json;


namespace SolarWatch.Services;

public class JsonProcessor : IJsonProcessor
{
    public SunriseSunset ProcessSunriseSunset(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement results = json.RootElement.GetProperty("results");

        SunriseSunset sunriseSunset = new SunriseSunset
        {
            Sunrise = results.GetProperty("sunrise").ToString(),
            Sunset = results.GetProperty("sunset").ToString(),
            SolarNoon = results.GetProperty("solar_noon").ToString(),
        };

        return sunriseSunset;
    }

    public double ProcessLat(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        return json.RootElement[0].GetProperty("lat").GetDouble();
    }
    
    public double ProcessLon(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);

        return json.RootElement[0].GetProperty("lon").GetDouble();
    }

    // private static DateTime GetDateTimeFromUnixTimeStamp(long timeStamp)
    // {
    //     DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
    //     DateTime dateTime = dateTimeOffset.UtcDateTime;
    //
    //     return dateTime;
    // }
    
}