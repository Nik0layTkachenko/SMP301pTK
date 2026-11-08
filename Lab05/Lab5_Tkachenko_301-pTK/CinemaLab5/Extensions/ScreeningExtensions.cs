using CinemaLab5.Models;

namespace CinemaLab5.Extensions;

public static class ScreeningExtensions
{
    // Класичний extension method 1
    public static bool IsAlmostFull(this Screening screening)
    {
        return screening.OccupancyPercentage >= 80.0;
    }

    // Класичний extension method 2
    public static string GetSummary(this Screening screening)
    {
        string state = screening.IsAlmostFull() ? "Майже заповнений (>=80%)" : "Є вільні місця";
        return $"Сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.HallName}) | " +
               $"Початок: {screening.StartTime:dd.MM.yyyy HH:mm} | " +
               $"Місця: {screening.SoldTickets}/{screening.TotalSeats} ({screening.OccupancyPercentage:F1}%) | {state}";
    }

    // Додаткове завдання (Розділ 7): C# 14 extension block
    extension(Screening screening)
    {
        public int RemainingSeats => screening.TotalSeats - screening.SoldTickets;

        public bool IsSoldOut()
            => screening.SoldTickets >= screening.TotalSeats;
    }
}
