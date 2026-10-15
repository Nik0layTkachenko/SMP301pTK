namespace CinemaBooking.Pricing;

public abstract class TicketPricing
{
    public abstract string Name { get; }

    public abstract decimal CalculatePrice(decimal basePrice);
}
