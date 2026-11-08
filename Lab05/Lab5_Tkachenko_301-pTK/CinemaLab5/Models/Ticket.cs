namespace CinemaLab5.Models;

public class Ticket
{
    public int TicketId { get; set; }
    public Screening Screening { get; set; }
    public string CustomerName { get; set; }
    public string ModifierName { get; set; }
    public int Quantity { get; set; }
    public decimal FinalPrice { get; set; }
    public DateTime PurchaseTime { get; set; }

    public Ticket(int ticketId, Screening screening, string customerName, string modifierName, int quantity, decimal finalPrice)
    {
        TicketId = ticketId;
        Screening = screening;
        CustomerName = customerName;
        ModifierName = modifierName;
        Quantity = quantity;
        FinalPrice = finalPrice;
        PurchaseTime = DateTime.Now;
    }
}
