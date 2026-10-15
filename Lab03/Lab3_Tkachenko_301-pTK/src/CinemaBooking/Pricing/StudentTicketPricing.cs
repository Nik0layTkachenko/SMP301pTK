namespace CinemaBooking.Pricing;

public sealed class StudentTicketPricing : TicketPricing
{
    public override string Name => "Студентський (-25%)";

    public override decimal CalculatePrice(decimal basePrice) => Math.Round(basePrice * 0.75m, 2);
}
