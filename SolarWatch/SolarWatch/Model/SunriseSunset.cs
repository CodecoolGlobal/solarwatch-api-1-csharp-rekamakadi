namespace SolarWatch;

public class SunriseSunset
{
    public int Id { get; set; }
    public DateTime ActualDate { get; set; }
    public DateTime Sunrise { get; set; }

    public DateTime Sunset { get; set; }
    public DateTime SolarNoon { get; set; }
    public int CityId { get; set; }

    public override string ToString()
    {
        return $"{Id} - {ActualDate} - {Sunrise} - {Sunset} - {SolarNoon} - {CityId}";
    }

    public void Update(SunriseSunset request)
    {
        request.Id = Id;
        ActualDate = request.ActualDate;
        Sunrise = request.Sunrise;
        Sunset = request.Sunset;
        SolarNoon = request.SolarNoon;
        CityId = request.CityId;
    }
}