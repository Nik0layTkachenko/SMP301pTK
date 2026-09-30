namespace CinemaBooking.Domain;
public class RegularPricingStrategy : ITicketPricingStrategy
{
    public decimal CalculatePrice(decimal basePrice) => basePrice;
}
