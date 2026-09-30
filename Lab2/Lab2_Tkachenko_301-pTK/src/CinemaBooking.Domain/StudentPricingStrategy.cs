namespace CinemaBooking.Domain;
public class StudentPricingStrategy : ITicketPricingStrategy
{
    public decimal CalculatePrice(decimal basePrice) => basePrice * 0.5m;
}
