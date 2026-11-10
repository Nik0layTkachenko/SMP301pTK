using CinemaLab8.Models;

namespace CinemaLab8.Extensions;

public static class ScreeningExtensions
{
    public static bool IsAlmostFull(this Screening screening)
        => screening.OccupancyPercentage >= 80.0;

    public static string GetSummary(this Screening screening)
        => $"Сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.Hall.Name}) | Продано квитків: {screening.SoldTickets}/{screening.TotalSeats}";

    extension(Screening screening)
    {
        public int RemainingSeats => screening.AvailableSeatsCount;
        public bool IsSoldOut() => screening.SoldTickets >= screening.TotalSeats;
    }
}
