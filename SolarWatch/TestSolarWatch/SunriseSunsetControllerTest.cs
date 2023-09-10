using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch;
using SolarWatch.Controllers;
using SolarWatch.Services;

namespace TestSolarWatch;

[TestFixture]
public class SunriseSunsetControllerTest
{
    private Mock<ILogger<SunriseSunsetController>> _loggerMock;
    private Mock<ISunriseSunsetProvider>? _sunriseSunsetProviderMock;
    private Mock<ILatLonProvider>? _latLonProviderMock;
    private Mock<IJsonProcessor> _jsonProcessorMock;
    private SunriseSunsetController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SunriseSunsetController>>();
        _sunriseSunsetProviderMock = new Mock<ISunriseSunsetProvider>();
        _latLonProviderMock = new Mock<ILatLonProvider>();
        _jsonProcessorMock = new Mock<IJsonProcessor>();
        _controller = new SunriseSunsetController(_loggerMock.Object, _jsonProcessorMock.Object, _sunriseSunsetProviderMock.Object, _latLonProviderMock.Object);
    }
    
    [Test]
    public void GetCurrent_ValidInput_ReturnsOkResult()
    {
        // Arrange
        DateTime date = DateTime.Parse("2023-09-10");
        string cityName = "Budapest";
        var latLonData = "[{\"name\":\"Budapest\",\"local_names\":{\"tg\":\"????????\",\"ps\":\"???????\",\"no\":\"Budapest\",\"ca\":\"Budapest\",\"es\":\"Budapest\",\"sr\":\"??????????\",\"sv\":\"Buda\npest\",\"mn\":\"????????\",\"hr\":\"Budimpešta\",\"gr\":\"??????????\",\"uk\":\"????????\",\"bn\":\"?????????\",\"th\":\"?????????\",\"gu\":\"?????????\",\"sh\":\"Budimpešta\",\"my\":\n\"????????????\",\"he\":\"??????\",\"ar\":\"???????\",\"tr\":\"Budapeşte\",\"kv\":\"????????\",\"de\":\"Budapest\",\"bs\":\"Budimpešta\",\"pt\":\"Budapeste\",\"af\":\"Boedapest\",\"bg\n\":\"????????\",\"hy\":\"?????????\",\"os\":\"????????\",\"ru\":\"????????\",\"mk\":\"??????????\",\"be\":\"????????\",\"el\":\"??????????\",\"tt\":\"????????\",\"pl\":\"Budapeszt\",\"\nis\":\"Búdapest\",\"ta\":\"??????????\",\"sk\":\"Budapešť\",\"av\":\"????????\",\"sl\":\"Budimpešta\",\"hu\":\"Budapest\",\"ja\":\"?????\",\"fa\":\"???????\",\"ht\":\"Boudapcs\",\"ka\":\n\"?????????\",\"ku\":\"Budapeşt\",\"ro\":\"Budapesta\",\"ur\":\"???????\",\"kk\":\"????????\",\"mr\":\"?????????\",\"uz\":\"Budapesht\",\"yi\":\"????????\",\"cs\":\"Budapešť\",\"en\":\"\nBudapest\",\"az\":\"Budapeşt\",\"ko\":\"?????\",\"hi\":\"?????????\",\"sq\":\"Budapesti\",\"lv\":\"Budapešta\",\"fy\":\"Boedapest\",\"la\":\"Budapestinum\",\"am\":\"?????\",\"ug\":\"Bu\ndapésht\",\"li\":\"Boedapes\",\"it\":\"Budapest\",\"fr\":\"Budapest\",\"nl\":\"Boedapest\",\"eo\":\"Budapesto\",\"ml\":\"????????????\",\"kn\":\"??????????\",\"cv\":\"????????\",\"ga\":\"Búdaipeist\",\"bo\":\"????????????\",\"zh\":\"????\",\"oc\":\"Budapcst\",\"lt\":\"Budapeštas\"},\"lat\":47.4979937,\"lon\":19.0403594,\"country\":\"HU\"}]"; // Replace with your mock data
        var sunsetSunriseData = "{\"results\":{\"sunrise\":\"4:13:18 AM\",\"sunset\":\"5:08:52 PM\",\"solar_noon\":\"10:41:05 AM\",\"day_length\":\"12:55:34\",\"civil_twilight_begin\":\"3:43:59 AM\n\",\"civil_twilight_end\":\"5:38:11 PM\",\"nautical_twilight_begin\":\"3:07:05 AM\",\"nautical_twilight_end\":\"6:15:05 PM\",\"astronomical_twilight_begin\":\"2:28:17 AM\",\"astronomical_twilight_end\":\"6:53:52 PM\"},\"status\":\"OK\"}";

        _latLonProviderMock?.Setup(mock => mock.GetCurrent(cityName)).Returns(latLonData);
        _jsonProcessorMock.Setup(mock => mock.ProcessLat(latLonData)).Returns(47.4979937);
        _jsonProcessorMock.Setup(mock => mock.ProcessLon(latLonData)).Returns(19.0403594);
        _sunriseSunsetProviderMock?.Setup(mock => mock.GetCurrent(It.IsAny<string>(), 47.4979937, 19.0403594))
            .Returns(sunsetSunriseData);
        _jsonProcessorMock.Setup(mock => mock.ProcessSunriseSunset(sunsetSunriseData))
            .Returns(new SunriseSunset());

        // Act
        ActionResult<SunriseSunset> result = _controller.GetCurrent(date, cityName);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public void GetCurrent_LatLonProviderThrowsException_ReturnsNotFound()
    {
        // Arrange
        DateTime date = DateTime.Parse("2023-09-10");
        string cityName = "TestCity";

        _latLonProviderMock.Setup(mock => mock.GetCurrent(cityName)).Throws<Exception>();

        // Act
        ActionResult<SunriseSunset> result = _controller.GetCurrent(date, cityName);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }

    [Test]
    public void GetCurrent_SunriseSunsetProviderThrowsException_ReturnsNotFound()
    {
        // Arrange
        DateTime date = DateTime.Parse("2024-09-10");
        string cityName = "Budapest";
        var latLonData = "[{\"name\":\"Budapest\",\"local_names\":{\"tg\":\"????????\",\"ps\":\"???????\",\"no\":\"Budapest\",\"ca\":\"Budapest\",\"es\":\"Budapest\",\"sr\":\"??????????\",\"sv\":\"Buda\npest\",\"mn\":\"????????\",\"hr\":\"Budimpešta\",\"gr\":\"??????????\",\"uk\":\"????????\",\"bn\":\"?????????\",\"th\":\"?????????\",\"gu\":\"?????????\",\"sh\":\"Budimpešta\",\"my\":\n\"????????????\",\"he\":\"??????\",\"ar\":\"???????\",\"tr\":\"Budapeşte\",\"kv\":\"????????\",\"de\":\"Budapest\",\"bs\":\"Budimpešta\",\"pt\":\"Budapeste\",\"af\":\"Boedapest\",\"bg\n\":\"????????\",\"hy\":\"?????????\",\"os\":\"????????\",\"ru\":\"????????\",\"mk\":\"??????????\",\"be\":\"????????\",\"el\":\"??????????\",\"tt\":\"????????\",\"pl\":\"Budapeszt\",\"\nis\":\"Búdapest\",\"ta\":\"??????????\",\"sk\":\"Budapešť\",\"av\":\"????????\",\"sl\":\"Budimpešta\",\"hu\":\"Budapest\",\"ja\":\"?????\",\"fa\":\"???????\",\"ht\":\"Boudapcs\",\"ka\":\n\"?????????\",\"ku\":\"Budapeşt\",\"ro\":\"Budapesta\",\"ur\":\"???????\",\"kk\":\"????????\",\"mr\":\"?????????\",\"uz\":\"Budapesht\",\"yi\":\"????????\",\"cs\":\"Budapešť\",\"en\":\"\nBudapest\",\"az\":\"Budapeşt\",\"ko\":\"?????\",\"hi\":\"?????????\",\"sq\":\"Budapesti\",\"lv\":\"Budapešta\",\"fy\":\"Boedapest\",\"la\":\"Budapestinum\",\"am\":\"?????\",\"ug\":\"Bu\ndapésht\",\"li\":\"Boedapes\",\"it\":\"Budapest\",\"fr\":\"Budapest\",\"nl\":\"Boedapest\",\"eo\":\"Budapesto\",\"ml\":\"????????????\",\"kn\":\"??????????\",\"cv\":\"????????\",\"ga\":\"Búdaipeist\",\"bo\":\"????????????\",\"zh\":\"????\",\"oc\":\"Budapcst\",\"lt\":\"Budapeštas\"},\"lat\":47.4979937,\"lon\":19.0403594,\"country\":\"HU\"}]";

        _latLonProviderMock.Setup(mock => mock.GetCurrent(cityName)).Returns(latLonData);
        _jsonProcessorMock.Setup(mock => mock.ProcessLat(latLonData)).Returns(47.4979937);
        _jsonProcessorMock.Setup(mock => mock.ProcessLon(latLonData)).Returns(19.0403594);
        _sunriseSunsetProviderMock.Setup(mock => mock.GetCurrent(It.IsAny<string>(), 47.4979937, 19.0403594))
            .Throws<Exception>();

        // Act
        ActionResult<SunriseSunset> result = _controller.GetCurrent(date, cityName);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
    }
}