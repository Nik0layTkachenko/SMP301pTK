namespace CinemaBooking.Domain;
public interface ITicketPricingStrategy
{
    decimal CalculatePrice(decimal basePrice);
}
