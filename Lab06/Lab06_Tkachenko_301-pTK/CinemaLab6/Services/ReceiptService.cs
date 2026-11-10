using CinemaLab6.Models;

namespace CinemaLab6.Services;

public class ReceiptService
{
    public void Handle(object? sender, Ticket ticket)
    {
        Console.WriteLine($"  [ReceiptService] Квитанція #{ticket.TicketId}: Покупець {ticket.CustomerName} | " +
                          $"Фільм: "{ticket.Screening.MovieTitle}" | Ряд {ticket.Row}, Місце {ticket.Seat} | " +
                          $"Тариф: {ticket.ModifierName} | Сума: {ticket.FinalPrice} грн.");
    }
}
