using CinemaLab7.Models;

namespace CinemaLab7.Extensions;

public static class ScreeningExtensions
{
    public static bool IsAlmostFull(this Screening screening)
        => screening.OccupancyPercentage >= 80.0;

    public static string GetSummary(this Screening screening)
        => $"Сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.Hall.Name}) | Продано: {screening.SoldTickets}/{screening.TotalSeats}";

    extension(Screening screening)
    {
        public int RemainingSeats => screening.AvailableSeatsCount;
        public bool IsSoldOut() => screening.SoldTickets >= screening.TotalSeats;
    }
}
