using CinemaLab6.Models;

namespace CinemaLab6.Services;

public class StatisticsService
{
    public int TicketsSoldCount { get; private set; }
    public decimal TotalRevenue { get; private set; }

    public void Handle(object? sender, Ticket ticket)
    {
        TicketsSoldCount++;
        TotalRevenue += ticket.FinalPrice;

        Console.WriteLine($"  [StatisticsService] Статистику оновлено: продано квитків = {TicketsSoldCount}, " +
                          $"виручка = {TotalRevenue} грн, вільних місць = {ticket.Screening.AvailableSeatsCount}.");
    }
}
