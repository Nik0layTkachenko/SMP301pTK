using CinemaLab7.Models;

namespace CinemaLab7.Services;

public class ReceiptService
{
    public void Handle(object? sender, Ticket ticket)
    {
        Console.WriteLine($"  [ReceiptService] Чек #{ticket.TicketId}: {ticket.CustomerName} | "{ticket.Screening.MovieTitle}" | Ряд {ticket.Row}, Місце {ticket.Seat} | {ticket.FinalPrice} грн.");
    }
}
