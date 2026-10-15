namespace CinemaBooking.Pricing;

public sealed class VipTicketPricing : TicketPricing
{
    public override string Name => "VIP (+40%)";

    public override decimal CalculatePrice(decimal basePrice) => Math.Round(basePrice * 1.40m, 2);
}
