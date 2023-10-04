// using DefaultNamespace;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.Extensions.Logging;
// using Moq;
// using SolarWatch;
// using SolarWatch.Controllers;
// using SolarWatch.Services;
// using SolarWatch.Services.Repository;
//
// namespace TestSolarWatch;
//
// [TestFixture]
// public class SunriseSunsetControllerTest
// {
//     private Mock<ILogger<SunriseSunsetController>> _loggerMock;
//     private Mock<ISunriseSunsetProvider>? _sunriseSunsetProviderMock;
//     private Mock<ICityProvider>? _cityProviderMock;
//     private Mock<IJsonProcessor> _jsonProcessorMock;
//     private Mock<ICityRepository> _cityRepositoryMock;
//     private Mock<ISunriseSunsetRepository> _sunriseSunsetRepositoryMock;
//     private SunriseSunsetController _controller;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _loggerMock = new Mock<ILogger<SunriseSunsetController>>();
//         _sunriseSunsetProviderMock = new Mock<ISunriseSunsetProvider>();
//         _cityProviderMock = new Mock<ICityProvider>();
//         _jsonProcessorMock = new Mock<IJsonProcessor>();
//         _cityRepositoryMock = new Mock<ICityRepository>();
//         _sunriseSunsetProviderMock = new Mock<ISunriseSunsetProvider>();
//         _controller = new SunriseSunsetController(
//             _loggerMock.Object, 
//             _jsonProcessorMock.Object, 
//             _sunriseSunsetProviderMock.Object, 
//             _cityProviderMock.Object,
//             _cityRepositoryMock.Object,
//             _sunriseSunsetRepositoryMock.Object
//             );
//     }
//     
//     [Test]
//     public async Task GetCurrent_ValidInput_ReturnsOkResult()
//     {
//         // Arrange
//         DateTime date = DateTime.Parse("2023-09-10");
//         string formattedDate = date.ToString("yyyy'-'M'-'d"); 
//         string cityName = "Budapest";
//         var cityData = "[{\"name\":\"Budapest\",\"local_names\":{\"tg\":\"????????\",\"ps\":\"???????\",\"no\":\"Budapest\",\"ca\":\"Budapest\",\"es\":\"Budapest\",\"sr\":\"??????????\",\"sv\":\"Buda\npest\",\"mn\":\"????????\",\"hr\":\"Budimpešta\",\"gr\":\"??????????\",\"uk\":\"????????\",\"bn\":\"?????????\",\"th\":\"?????????\",\"gu\":\"?????????\",\"sh\":\"Budimpešta\",\"my\":\n\"????????????\",\"he\":\"??????\",\"ar\":\"???????\",\"tr\":\"Budapeşte\",\"kv\":\"????????\",\"de\":\"Budapest\",\"bs\":\"Budimpešta\",\"pt\":\"Budapeste\",\"af\":\"Boedapest\",\"bg\n\":\"????????\",\"hy\":\"?????????\",\"os\":\"????????\",\"ru\":\"????????\",\"mk\":\"??????????\",\"be\":\"????????\",\"el\":\"??????????\",\"tt\":\"????????\",\"pl\":\"Budapeszt\",\"\nis\":\"Búdapest\",\"ta\":\"??????????\",\"sk\":\"Budapešť\",\"av\":\"????????\",\"sl\":\"Budimpešta\",\"hu\":\"Budapest\",\"ja\":\"?????\",\"fa\":\"???????\",\"ht\":\"Boudapcs\",\"ka\":\n\"?????????\",\"ku\":\"Budapeşt\",\"ro\":\"Budapesta\",\"ur\":\"???????\",\"kk\":\"????????\",\"mr\":\"?????????\",\"uz\":\"Budapesht\",\"yi\":\"????????\",\"cs\":\"Budapešť\",\"en\":\"\nBudapest\",\"az\":\"Budapeşt\",\"ko\":\"?????\",\"hi\":\"?????????\",\"sq\":\"Budapesti\",\"lv\":\"Budapešta\",\"fy\":\"Boedapest\",\"la\":\"Budapestinum\",\"am\":\"?????\",\"ug\":\"Bu\ndapésht\",\"li\":\"Boedapes\",\"it\":\"Budapest\",\"fr\":\"Budapest\",\"nl\":\"Boedapest\",\"eo\":\"Budapesto\",\"ml\":\"????????????\",\"kn\":\"??????????\",\"cv\":\"????????\",\"ga\":\"Búdaipeist\",\"bo\":\"????????????\",\"zh\":\"????\",\"oc\":\"Budapcst\",\"lt\":\"Budapeštas\"},\"lat\":47.4979937,\"lon\":19.0403594,\"country\":\"HU\"}]";
//         var sunsetSunriseData = "{\"results\":{\"sunrise\":\"4:13:18 AM\",\"sunset\":\"5:08:52 PM\",\"solar_noon\":\"10:41:05 AM\",\"day_length\":\"12:55:34\",\"civil_twilight_begin\":\"3:43:59 AM\n\",\"civil_twilight_end\":\"5:38:11 PM\",\"nautical_twilight_begin\":\"3:07:05 AM\",\"nautical_twilight_end\":\"6:15:05 PM\",\"astronomical_twilight_begin\":\"2:28:17 AM\",\"astronomical_twilight_end\":\"6:53:52 PM\"},\"status\":\"OK\"}";
//
//         _cityRepositoryMock.Setup(mock => mock.GetByName(cityName)).Returns((City)null);
//         _cityProviderMock.Setup(mock => mock.GetCurrent(cityName)).ReturnsAsync(cityData);
//         _jsonProcessorMock.Setup(mock => mock.ProcessCity(cityData)).Returns(new City());
//         _sunriseSunsetRepositoryMock.Setup(mock => mock.GetByDateAndCityId(date, It.IsAny<int>())).Returns((SunriseSunset)null);
//         _sunriseSunsetProviderMock.Setup(mock => mock.GetCurrent(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
//             .ReturnsAsync(sunsetSunriseData);
//         _jsonProcessorMock.Setup(mock => mock.ProcessSunriseSunset(sunsetSunriseData, formattedDate, 1))
//             .Returns(new SunriseSunset());
//
//         // Act
//         var result = await _controller.GetCurrent(date, cityName);
//
//         // Assert
//         Assert.IsInstanceOf<OkObjectResult>(result.Result);
//     }
//
//     [Test]
//     public async Task GetCurrent_CityProviderThrowsException_ReturnsNotFound()
//     {
//         // Arrange
//         DateTime date = DateTime.Parse("2023-09-10");
//         string cityName = "TestCity";
//
//         _cityProviderMock.Setup(mock => mock.GetCurrent(cityName)).Throws<Exception>();
//
//         // Act
//         var result = await _controller.GetCurrent(date, cityName);
//
//         // Assert
//         Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
//     }
//
//     [Test]
//     public async Task GetCurrent_SunriseSunsetProviderThrowsException_ReturnsNotFound()
//     {
//         // Arrange
//         DateTime date = DateTime.Parse("2024-09-10");
//         string cityName = "Budapest";
//         var cityData = "[{\"name\":\"Budapest\",\"local_names\":{\"tg\":\"????????\",\"ps\":\"???????\",\"no\":\"Budapest\",\"ca\":\"Budapest\",\"es\":\"Budapest\",\"sr\":\"??????????\",\"sv\":\"Buda\npest\",\"mn\":\"????????\",\"hr\":\"Budimpešta\",\"gr\":\"??????????\",\"uk\":\"????????\",\"bn\":\"?????????\",\"th\":\"?????????\",\"gu\":\"?????????\",\"sh\":\"Budimpešta\",\"my\":\n\"????????????\",\"he\":\"??????\",\"ar\":\"???????\",\"tr\":\"Budapeşte\",\"kv\":\"????????\",\"de\":\"Budapest\",\"bs\":\"Budimpešta\",\"pt\":\"Budapeste\",\"af\":\"Boedapest\",\"bg\n\":\"????????\",\"hy\":\"?????????\",\"os\":\"????????\",\"ru\":\"????????\",\"mk\":\"??????????\",\"be\":\"????????\",\"el\":\"??????????\",\"tt\":\"????????\",\"pl\":\"Budapeszt\",\"\nis\":\"Búdapest\",\"ta\":\"??????????\",\"sk\":\"Budapešť\",\"av\":\"????????\",\"sl\":\"Budimpešta\",\"hu\":\"Budapest\",\"ja\":\"?????\",\"fa\":\"???????\",\"ht\":\"Boudapcs\",\"ka\":\n\"?????????\",\"ku\":\"Budapeşt\",\"ro\":\"Budapesta\",\"ur\":\"???????\",\"kk\":\"????????\",\"mr\":\"?????????\",\"uz\":\"Budapesht\",\"yi\":\"????????\",\"cs\":\"Budapešť\",\"en\":\"\nBudapest\",\"az\":\"Budapeşt\",\"ko\":\"?????\",\"hi\":\"?????????\",\"sq\":\"Budapesti\",\"lv\":\"Budapešta\",\"fy\":\"Boedapest\",\"la\":\"Budapestinum\",\"am\":\"?????\",\"ug\":\"Bu\ndapésht\",\"li\":\"Boedapes\",\"it\":\"Budapest\",\"fr\":\"Budapest\",\"nl\":\"Boedapest\",\"eo\":\"Budapesto\",\"ml\":\"????????????\",\"kn\":\"??????????\",\"cv\":\"????????\",\"ga\":\"Búdaipeist\",\"bo\":\"????????????\",\"zh\":\"????\",\"oc\":\"Budapcst\",\"lt\":\"Budapeštas\"},\"lat\":47.4979937,\"lon\":19.0403594,\"country\":\"HU\"}]";
//
//         _cityProviderMock.Setup(mock => mock.GetCurrent(cityName)).ReturnsAsync((string cityName) => cityData);
//         _jsonProcessorMock.Setup(mock => mock.ProcessCity(cityData)).Returns(new City{1, "Budapest", 47.4979937, 19.0403594, null, "HU"});
//         _sunriseSunsetProviderMock.Setup(mock => mock.GetCurrent(It.IsAny<string>(), 47.4979937, 19.0403594))
//             .Throws<Exception>();
//
//         // Act
//         var result = await _controller.GetCurrent(date, cityName);
//
//         // Assert
//         Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
//     }
// }