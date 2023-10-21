namespace SolarWatch;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public override string ToString()
    {
        return $"{Id} - {Name} - {Latitude} - {Longitude} - {State} - {Country}";
    }

    public void Update(City request)
    {
        request.Id = Id;
        Name = request.Name;
        Longitude = request.Longitude;
        Latitude = request.Latitude;
        Country = request.Country;
        State = request.State;
    }
}