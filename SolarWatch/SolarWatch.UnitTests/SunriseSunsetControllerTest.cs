using Microsoft.AspNetCore.Mvc;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Services.Repository;

namespace SolarWatch.UnitTests;

[TestFixture]
public class SunriseSunsetControllerTest
{
    private SunriseSunsetController _controller;
    private Mock<ICityRepository> _cityRepositoryMock;
    private Mock<ISunriseSunsetRepository> _sunriseSunsetRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _cityRepositoryMock = new Mock<ICityRepository>();
        _sunriseSunsetRepositoryMock = new Mock<ISunriseSunsetRepository>();
        _controller = new SunriseSunsetController(_cityRepositoryMock.Object, _sunriseSunsetRepositoryMock.Object);
    }

    [Test]
    public async Task GetAllCities_ReturnsOkResult()
    {
        // Arrange
        _cityRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<City>());

        // Act
        var result = await _controller.GetAllCities();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public async Task GetOrAddByCityName_ReturnsOkResult()
    {
        // Arrange
        string cityName = "TestCity";
        _cityRepositoryMock.Setup(repo => repo.GetByNameAsync(cityName)).ReturnsAsync(new City());

        // Act
        var result = await _controller.GetOrAddByCityName(cityName);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }
    
    [Test]
    public async Task GetCityById_ReturnsOkResult()
    {
        // Arrange
        int cityId = 1;
        _cityRepositoryMock.Setup(repo => repo.GetByIdAsync(cityId)).ReturnsAsync(new City());

        // Act
        var result = await _controller.GetCityById(cityId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public void AddSunriseSunset_IsNotNull()
    {
        // Arrange
        var newSunriseSunset = new SunriseSunset();

        // Act
        var result = _controller.AddSunriseSunset(newSunriseSunset);

        // Assert
        Assert.IsNotNull(result);
    }

    [Test]
    public void UpdateCity_ReturnsOkResult()
    {
        // Arrange
        int cityId = 1;
        var request = new City();

        // Act
        var result = _controller.UpdateCity(cityId, request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Value, Is.EqualTo($"The current city on id: {cityId} is successfully updated to {request}."));
    }

    [Test]
    public void DeleteCity_ReturnsOkResult()
    {
        // Arrange
        int cityId = 1;

        // Act
        var result = _controller.DeleteCity(cityId) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Value, Is.EqualTo($"The current city on id: {cityId} is successfully deleted."));
    }

    [Test]
    public async Task GetAllSunsetSunrises_ReturnsOkResult()
    {
        // Arrange
        _sunriseSunsetRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<SunriseSunset>());

        // Act
        var result = await _controller.GetAllSunsetSunrises();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public async Task GetOrAddByDateAndCityName_ReturnsOkResult()
    {
        // Arrange
        DateTime date = DateTime.Now;
        string cityName = "TestCity";
        _cityRepositoryMock.Setup(repo => repo.GetByNameAsync(cityName)).ReturnsAsync(new City());
        _sunriseSunsetRepositoryMock.Setup(repo => repo.GetByDateAndCityAsync(date, It.IsAny<City>())).ReturnsAsync(new SunriseSunset());

        // Act
        var result = await _controller.GetOrAddByDateAndCityName(date, cityName);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public async Task GetSsById_ReturnsOkResult()
    {
        // Arrange
        int sunriseSunsetId = 1;
        _sunriseSunsetRepositoryMock.Setup(repo => repo.GetByIdAsync(sunriseSunsetId)).ReturnsAsync(new SunriseSunset());

        // Act
        var result = await _controller.GetSsById(sunriseSunsetId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
    }

    [Test]
    public void UpdateSunriseSunset_ReturnsOkResult()
    {
        // Arrange
        int sunriseSunsetId = 1;
        var request = new SunriseSunset();

        // Act
        var result = _controller.UpdateSunriseSunset(sunriseSunsetId, request) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Value, Is.EqualTo($"The current sunriseSunset on id: {sunriseSunsetId} is successfully updated to {request}."));
    }

    [Test]
    public void DeleteSunriseSunset_ReturnsOkResult()
    {
        // Arrange
        int sunriseSunsetId = 1;

        // Act
        var result = _controller.DeleteSunriseSunset(sunriseSunsetId) as OkObjectResult;

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Value, Is.EqualTo($"The current sunriseSunset on id: {sunriseSunsetId} is successfully deleted."));
    }
}