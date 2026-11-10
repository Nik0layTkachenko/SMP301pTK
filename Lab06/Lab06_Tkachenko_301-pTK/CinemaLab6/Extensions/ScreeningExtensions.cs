using CinemaLab6.Models;

namespace CinemaLab6.Extensions;

public static class ScreeningExtensions
{
    public static bool IsAlmostFull(this Screening screening)
    {
        return screening.OccupancyPercentage >= 80.0;
    }

    public static string GetSummary(this Screening screening)
    {
        return $"Сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.Hall.Name}) | " +
               $"Статус: {screening.Status} | Вільно місць: {screening.AvailableSeatsCount}/{screening.TotalSeats} " +
               $"({screening.OccupancyPercentage:F1}%)";
    }

    extension(Screening screening)
    {
        public int RemainingSeats => screening.AvailableSeatsCount;

        public bool IsSoldOut()
            => screening.SoldTickets >= screening.TotalSeats;
    }
}
