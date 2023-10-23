using SolarWatch.Contracts;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SolarWatch.IntegrationTests;

public class SunriseSunsetController_IntegrationTest
{
    private HttpClient _client;

        [SetUp]
        public void Setup()
        {
            var factory = new WebApplicationFactory<Program>();
            string connectionString = "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=Kiskutyafüle32!;TrustServerCertificate=true;";
            Environment.SetEnvironmentVariable("CONNECTION_STRING", connectionString);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

           
            _client = factory.CreateClient();

            AuthRequest authRequest = new AuthRequest("admin@admin.com", "admin123");
            var jsonString = JsonSerializer.Serialize(authRequest);
            var jsonStringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync("/Auth/Login", jsonStringContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var desContent = JsonSerializer.Deserialize<AuthResponse>(content,options);
            var token = desContent.Token;
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        [Test]
        public async Task GetSunriseSunsetAsync_ReturnsSunriseSunsetData()
        {
            // Arrange
            
            var cityName = "Budapest";
            var date = DateTime.Today;

            // Act
            var response = await _client.GetAsync($"/SunriseSunset/GetOrAddByDateAndCityName?cityName={cityName}&date={date}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            
            var responseContent = await response.Content.ReadAsStringAsync();
            var sunriseSunsetData = JsonConvert.DeserializeObject<SunriseSunset>(responseContent);
            
            Assert.NotNull(sunriseSunsetData);
            Assert.That(date, Is.EqualTo(sunriseSunsetData.ActualDate)); 
        }
        
        [Test]
        public async Task GetSunriseSunsetAsync_ReturnsNotFoundForInvalidCity()
        {
            // Arrange
            var cityName = "NonExistentCity";
            var date = "2022-01-01";

            // Act
            var response = await _client.GetAsync($"/SunriseSunset/GetOrAddByDateAndCityName?cityName={cityName}&date={date}");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }
        
        [Test]
        public async Task CreateCity_ReturnsCreatedResponse()
        {
            // Arrange
            var newCity = new City
            {
                Name = "NewCity",
                Country = "Country",
                State = "State",
                Latitude = 42.1234,
                Longitude = -78.5678
            };
    
            var jsonString = JsonSerializer.Serialize(newCity);
            var jsonStringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/AddCity", jsonStringContent);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
        
}