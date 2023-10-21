using System.Globalization;
using SolarWatch.Services;

namespace TestSolarWatch;

[TestFixture]
public class JsonProcessorTests
{
    private JsonProcessor _jsonProcessor;

    [SetUp]
    public void SetUp()
    {
        _jsonProcessor = new JsonProcessor();
    }

    [Test]
    public void ProcessSunriseSunset_ReturnsValidSunriseSunset()
    {
        // Arrange
        string jsonData = "{\"results\":{\"sunrise\":\"4:13:18 AM\",\"sunset\":\"5:08:52 PM\",\"solar_noon\":\"10:41:05 AM\",\"day_length\":\"12:55:34\"}}";
        string formattedDate = "2023-09-10";
        int cityId = 1;
        
        // Act
        var sunriseSunset = _jsonProcessor.ProcessSunriseSunset(jsonData, formattedDate, cityId);

        // Assert
        Assert.IsNotNull(sunriseSunset);
        Assert.That(sunriseSunset.ActualDate, Is.EqualTo(DateTime.ParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture)));
        Assert.That(sunriseSunset.Sunrise, Is.EqualTo(DateTime.ParseExact("4:13:18 AM", "h:mm:ss tt", CultureInfo.InvariantCulture)));
        Assert.That(sunriseSunset.Sunset, Is.EqualTo(DateTime.ParseExact("5:08:52 PM", "h:mm:ss tt", CultureInfo.InvariantCulture)));
        Assert.That(sunriseSunset.SolarNoon, Is.EqualTo(DateTime.ParseExact("10:41:05 AM", "h:mm:ss tt", CultureInfo.InvariantCulture)));
        Assert.That(sunriseSunset.CityId, Is.EqualTo(cityId));
    }

    [Test]
    public void ProcessCity_ReturnsValidCity()
    {
        // Arrange
        string data = "[{\"name\": \"TestCity\", \"lat\": 123.456, \"lon\": 789.012, \"state\": \"TestState\", \"country\": \"TestCountry\"}]";

        // Act
        var city = _jsonProcessor.ProcessCity(data);

        // Assert
        Assert.That(city.Name, Is.EqualTo("TestCity"));
        Assert.That(city.Latitude, Is.EqualTo(123.456));
        Assert.That(city.Longitude, Is.EqualTo(789.012));
        Assert.That(city.State, Is.EqualTo("TestState"));
        Assert.That(city.Country, Is.EqualTo("TestCountry"));
    }

    [Test]
    public void GetDateTimeFromString_ParsesTimeCorrectly()
    {
        // Arrange
        string timeToConvert = "6:30:00 AM";
        var expectedResult = DateTime.ParseExact("6:30:00 AM", "h:mm:ss tt", CultureInfo.InvariantCulture);
    
        // Act
        var dateTime = _jsonProcessor.GetDateTimeFromString(timeToConvert);
        
    
        // Assert
        Assert.That(dateTime.ToString("h:mm:ss tt"), Is.EqualTo(expectedResult.ToString("h:mm:ss tt")));
    }
}