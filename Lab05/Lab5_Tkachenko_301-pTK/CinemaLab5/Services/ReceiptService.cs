using CinemaLab5.Models;

namespace CinemaLab5.Services;

// Підписник 1 (Subscriber 1)
public class ReceiptService
{
    public void Handle(object? sender, Ticket ticket)
    {
        Console.WriteLine($"  [ReceiptService] Квитанція #{ticket.TicketId}: {ticket.CustomerName} | " +
                          $"Сеанс: "{ticket.Screening.MovieTitle}" | Квитків: {ticket.Quantity} | " +
                          $"Тариф: {ticket.ModifierName} | Разом: {ticket.FinalPrice} грн.");
    }
}
