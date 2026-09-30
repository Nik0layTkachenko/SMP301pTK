using System;
namespace CinemaBooking.Domain;
public class Ticket
{
    public int SeatNumber { get; }
    public decimal Price { get; }
    public Ticket(int seatNumber, decimal price)
    {
        if (price <= 0) throw new ArgumentException("Ціна повинна бути більшою за 0.", nameof(price));
        SeatNumber = seatNumber;
        Price = price;
    }
}
