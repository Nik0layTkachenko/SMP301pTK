using CinemaLab7.Models;

namespace CinemaLab7.Services;

public class StatisticsService
{
    public int TicketsSoldCount { get; private set; }
    public decimal TotalRevenue { get; private set; }

    public void Handle(object? sender, Ticket ticket)
    {
        TicketsSoldCount++;
        TotalRevenue += ticket.FinalPrice;
    }
}
