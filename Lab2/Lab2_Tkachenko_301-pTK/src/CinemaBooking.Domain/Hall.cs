namespace CinemaBooking.Domain;
public class Hall
{
    public string Name { get; }
    public int TotalSeats { get; }
    public Hall(string name, int totalSeats)
    {
        Name = name;
        TotalSeats = totalSeats;
    }
}
