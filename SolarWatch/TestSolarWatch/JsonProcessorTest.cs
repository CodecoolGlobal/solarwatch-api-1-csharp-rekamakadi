using SolarWatch;
using SolarWatch.Services;

namespace TestSolarWatch;

[TestFixture]
public class JsonProcessorTest
{
    private JsonProcessor _jsonProcessor;

    [SetUp]
    public void SetUp()
    {
        _jsonProcessor = new JsonProcessor();
    }

    [Test]
    public void ProcessSunriseSunset_ValidData_ReturnsSunriseSunset()
    {
        // Arrange
        string jsonData = "{\"results\":{\"sunrise\":\"6:00 AM\",\"sunset\":\"6:00 PM\",\"solar_noon\":\"12:00 PM\"}}";

        // Act
        SunriseSunset sunriseSunset = _jsonProcessor.ProcessSunriseSunset(jsonData);

        // Assert
        Assert.IsNotNull(sunriseSunset);
        Assert.AreEqual("6:00 AM", sunriseSunset.Sunrise);
        Assert.AreEqual("6:00 PM", sunriseSunset.Sunset);
        Assert.AreEqual("12:00 PM", sunriseSunset.SolarNoon);
    }

    [Test]
    public void ProcessLat_ValidData_ReturnsLatitude()
    {
        // Arrange
        string jsonData = "[{\"lat\": 123.456, \"lon\": 789.012}]";

        // Act
        double lat = _jsonProcessor.ProcessLat(jsonData);

        // Assert
        Assert.AreEqual(123.456, lat);
    }

    [Test]
    public void ProcessLon_ValidData_ReturnsLongitude()
    {
        // Arrange
        string jsonData = "[{\"lat\": 123.456, \"lon\": 789.012}]";

        // Act
        double lon = _jsonProcessor.ProcessLon(jsonData);

        // Assert
        Assert.AreEqual(789.012, lon);
    }
}