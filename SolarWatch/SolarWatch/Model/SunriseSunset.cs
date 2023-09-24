namespace SolarWatch;

public class SunriseSunset
{
    public int Id { get; init; }
    public DateTime ActualDate { get; init; }
    public DateTime Sunrise { get; init; }

    public DateTime Sunset { get; init; }
    public DateTime SolarNoon { get; init; }
    public int CityId { get; init; }
}