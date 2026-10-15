namespace CinemaBooking.Pricing;

public sealed class StandardTicketPricing : TicketPricing
{
    public override string Name => "Стандартний";

    public override decimal CalculatePrice(decimal basePrice) => basePrice;
}
