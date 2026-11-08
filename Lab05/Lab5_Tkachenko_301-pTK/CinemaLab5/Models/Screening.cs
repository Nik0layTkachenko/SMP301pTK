namespace CinemaLab5.Models;

public class Screening
{
    public int Id { get; set; }
    public string MovieTitle { get; set; }
    public string HallName { get; set; }
    public DateTime StartTime { get; set; }
    public decimal BasePrice { get; set; }
    public int TotalSeats { get; set; }
    public int SoldTickets { get; set; }

    public Screening(int id, string movieTitle, string hallName, DateTime startTime, decimal basePrice, int totalSeats)
    {
        Id = id;
        MovieTitle = movieTitle;
        HallName = hallName;
        StartTime = startTime;
        BasePrice = basePrice;
        TotalSeats = totalSeats;
        SoldTickets = 0;
    }

    public double OccupancyPercentage =>
        TotalSeats == 0 ? 0 : (double)SoldTickets / TotalSeats * 100.0;
}
