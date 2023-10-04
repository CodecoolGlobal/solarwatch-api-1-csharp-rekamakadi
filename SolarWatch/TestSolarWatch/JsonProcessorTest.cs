// using System.Globalization;
// using System.Text.Json;
// using Moq;
// using SolarWatch;
// using SolarWatch.Services;
//
// namespace TestSolarWatch;
//
// [TestFixture]
// public class JsonProcessorTest
// {
//     private JsonProcessor _jsonProcessor;
//     private Mock<SunriseSunsetProvider> _sunriseSunsetProviderMock;
//
//     [SetUp]
//     public void SetUp()
//     {
//         _jsonProcessor = new JsonProcessor();
//         _sunriseSunsetProviderMock = new Mock<SunriseSunsetProvider>();
//     }
//
//     [Test]
//      public void ProcessSunriseSunset_ValidData_ReturnsSunriseSunsetObject()
//         {
//             // Arrange
//             string jsonData = "{\"results\":{\"sunrise\":\"4:13:18 AM\",\"sunset\":\"5:08:52 PM\",\"solar_noon\":\"10:41:05 AM\",\"day_length\":\"12:55:34\"}}";
//             string formattedDate = "2023-09-10";
//             int cityId = 1;
//
//             var jsonDocumentMock = new Mock<JsonDocument>();
//             _sunriseSunsetProviderMock.Setup(mock => mock.GetCurrent(It.IsAny<string>(), It.IsAny<double>(), It.IsAny<double>()))
//                 .ReturnsAsync(sunsetSunriseData);
//             // Act
//             var sunriseSunset = _jsonProcessor.ProcessSunriseSunset(jsonData, formattedDate, cityId);
//
//             // Assert
//             Assert.IsNotNull(sunriseSunset);
//             Assert.AreEqual(DateTime.ParseExact(formattedDate, "yyyy-MM-dd", CultureInfo.InvariantCulture), sunriseSunset.ActualDate);
//             Assert.AreEqual(DateTime.ParseExact("4:13:18 AM", "h:mm:ss tt", CultureInfo.InvariantCulture), sunriseSunset.Sunrise);
//             Assert.AreEqual(DateTime.ParseExact("5:08:52 PM", "h:mm:ss tt", CultureInfo.InvariantCulture), sunriseSunset.Sunset);
//             Assert.AreEqual(DateTime.ParseExact("10:41:05 AM", "h:mm:ss tt", CultureInfo.InvariantCulture), sunriseSunset.SolarNoon);
//             Assert.AreEqual(cityId, sunriseSunset.CityId);
//         }
//
//         [Test]
//         public void ProcessCity_ValidData_ReturnsCityObject()
//         {
//             // Arrange
//             string jsonData = "[{\"name\":\"Budapest\",\"lat\":47.4979937,\"lon\":19.0403594,\"state\":\"SomeState\",\"country\":\"HU\"}]";
//
//             var jsonDocumentMock = new Mock<JsonDocument>();
//             var jsonElementMock = new Mock<JsonElement>();
//
//             jsonDocumentMock.Setup(m => m.RootElement).Returns(jsonElementMock.Object);
//             jsonElementMock.Setup(m => m[0].GetProperty("name").ToString()).Returns("Budapest");
//             jsonElementMock.Setup(m => m[0].GetProperty("lat").GetDouble()).Returns(47.4979937);
//             jsonElementMock.Setup(m => m[0].GetProperty("lon").GetDouble()).Returns(19.0403594);
//             jsonElementMock.Setup(m => m[0].GetProperty("state").ToString()).Returns("SomeState");
//             jsonElementMock.Setup(m => m[0].GetProperty("country").ToString()).Returns("HU");
//
//             City city = new City
//             {
//                 Name = "Budapest",
//                 Latitude = 47.4979937,
//                 Longitude = 19.0403594,
//                 State = "Some State",
//                 Country = "Hungary"
//             };
//             // Act
//             
//             var result = _jsonProcessor.ProcessCity(jsonData);
//
//             // Assert
//             Assert.IsNotNull(result);
//             Assert.That(result, Is.EqualTo(city));
//             }
//     // public void ProcessSunriseSunset_ValidData_ReturnsSunriseSunset()
//     // {
//     //     // Arrange
//     //     string jsonData = "{\"results\":{\"sunrise\":\"6:00 AM\",\"sunset\":\"6:00 PM\",\"solar_noon\":\"12:00 PM\"}}";
//     //
//     //     // Act
//     //     SunriseSunset sunriseSunset = _jsonProcessor.ProcessSunriseSunset(jsonData, DateTime.Today.ToString());
//     //
//     //     // Assert
//     //     Assert.IsNotNull(sunriseSunset);
//     //     Assert.AreEqual("6:00 AM", sunriseSunset.Sunrise);
//     //     Assert.AreEqual("6:00 AM", sunriseSunset.Sunrise);
//     //     Assert.AreEqual("6:00 AM", sunriseSunset.Sunrise);
//     //     Assert.AreEqual("6:00 AM", sunriseSunset.Sunrise);
//     //     Assert.AreEqual("6:00 PM", sunriseSunset.Sunset);
//     //     Assert.AreEqual("12:00 PM", sunriseSunset.SolarNoon);
//     // }
//     //
//     // [Test]
//     // public void ProcessCity_ValidData_ReturnsLat_Lon()
//     // {
//     //     // Arrange
//     //     string jsonData = "[{\"lat\": 123.456, \"lon\": 789.012}]";
//     //
//     //     // Act
//     //     double lat = _jsonProcessor.ProcessCity(jsonData).Latitude;
//     //     double lon = _jsonProcessor.ProcessCity(jsonData).Longitude;
//     //
//     //     // Assert
//     //     Assert.AreEqual(123.456, lat);
//     //     Assert.AreEqual(789.012, lon);
//     // }
// }