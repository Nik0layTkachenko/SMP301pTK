using CinemaLab5.Models;

namespace CinemaLab5.Services;

// Підписник 2 (Subscriber 2)
public class StatisticsService
{
    public int TicketsSoldCount { get; private set; }
    public decimal TotalRevenue { get; private set; }

    public void Handle(object? sender, Ticket ticket)
    {
        TicketsSoldCount += ticket.Quantity;
        TotalRevenue += ticket.FinalPrice;

        Console.WriteLine($"  [StatisticsService] Статистика кінотеатру: продано квитків = {TicketsSoldCount}, " +
                          $"загальна виручка = {TotalRevenue} грн, заповненість залу "{ticket.Screening.MovieTitle}" = {ticket.Screening.OccupancyPercentage:F1}%.");
    }
}
