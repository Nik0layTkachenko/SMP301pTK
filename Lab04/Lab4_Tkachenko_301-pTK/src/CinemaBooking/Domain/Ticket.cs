namespace CinemaBooking.Domain;

public class Ticket
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required Guid ScreeningId { get; init; }

    public required string MovieTitle { get; init; }

    public required Seat Seat { get; init; }

    public required decimal Price { get; init; }

    public required string PricingType { get; init; }

    public string? CustomerNote { get; init; }

    public DateTime PurchasedAt { get; init; } = DateTime.Now;

    public override string ToString()
    {
        string noteInfo = string.IsNullOrWhiteSpace(CustomerNote) ? "" : $" [Примітка: {CustomerNote}]";
        return $"Квиток #{Id.ToString()[..8]} | Фільм: \"{MovieTitle}\" | {Seat} | Тариф: {PricingType} | Ціна: {Price:F2} грн{noteInfo}";
    }
}
