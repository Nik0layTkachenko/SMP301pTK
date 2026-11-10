namespace CinemaLab7.Models;

public class Ticket
{
    public int TicketId { get; }
    public Screening Screening { get; }
    public string CustomerName { get; }
    public int Row { get; }
    public int Seat { get; }
    public string ModifierName { get; }
    public decimal FinalPrice { get; }
    public DateTime PurchaseTime { get; }

    public Ticket(
        int ticketId,
        Screening screening,
        string customerName,
        int row,
        int seat,
        string modifierName,
        decimal finalPrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketId);
        ArgumentNullException.ThrowIfNull(screening);
        ArgumentException.ThrowIfNullOrWhiteSpace(customerName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(row);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seat);
        ArgumentException.ThrowIfNullOrWhiteSpace(modifierName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(finalPrice);

        TicketId = ticketId;
        Screening = screening;
        CustomerName = customerName;
        Row = row;
        Seat = seat;
        ModifierName = modifierName;
        FinalPrice = finalPrice;
        PurchaseTime = DateTime.Now;
    }
}
