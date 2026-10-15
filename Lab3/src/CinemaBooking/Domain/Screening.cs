using CinemaBooking.Pricing;

namespace CinemaBooking.Domain;

public class Screening
{
    private readonly HashSet<Seat> _bookedSeats = [];
    private readonly List<Ticket> _tickets = [];

    public Guid Id { get; init; } = Guid.NewGuid();

    public required Movie Movie { get; init; }

    public required Hall Hall { get; init; }

    public required DateTime StartTime { get; init; }

    public required decimal BasePrice { get; init; }

    public ScreeningStatus Status { get; private set; } = ScreeningStatus.Scheduled;

    public int AvailableSeats => Hall.TotalCapacity - _bookedSeats.Count;

    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

    public void ChangeStatus(ScreeningStatus newStatus)
    {
        if (Status == ScreeningStatus.Cancelled && newStatus != ScreeningStatus.Cancelled)
            throw new InvalidOperationException("Скасований сеанс не можна перевести в інший статус.");

        if (Status == ScreeningStatus.Finished && newStatus != ScreeningStatus.Finished)
            throw new InvalidOperationException("Завершений сеанс не можна змінити.");

        Status = newStatus;
    }

    public string GetScreeningStatusDescription() => Status switch
    {
        ScreeningStatus.Scheduled => "Сеанс заплановано (продаж квитків ще не відкрито)",
        ScreeningStatus.SalesOpen => "Продаж квитків відкрито",
        ScreeningStatus.Started => "Сеанс уже розпочався",
        ScreeningStatus.Finished => "Сеанс завершено",
        ScreeningStatus.Cancelled => "Сеанс скасовано",
        _ => "Невідомий статус"
    };

    public string GetSalesState() => this switch
    {
        { Status: ScreeningStatus.SalesOpen, AvailableSeats: 0 } => "SoldOut",
        { Status: ScreeningStatus.SalesOpen, AvailableSeats: <= 5 } => "FewSeatsLeft",
        { Status: ScreeningStatus.SalesOpen } => "Available",
        { Status: ScreeningStatus.Scheduled } => "NotOnSaleYet",
        { Status: ScreeningStatus.Cancelled } => "Cancelled",
        _ => "Closed"
    };

    public Ticket BookSeat(Seat seat, TicketPricing pricingStrategy, string? customerNote = null)
    {
        ArgumentNullException.ThrowIfNull(seat);
        ArgumentNullException.ThrowIfNull(pricingStrategy);
        seat.Validate();

        if (Status != ScreeningStatus.SalesOpen)
            throw new InvalidOperationException($"Купівля неможлива. Поточний статус: {GetScreeningStatusDescription()}.");

        if (!Hall.IsSeatValid(seat))
            throw new ArgumentOutOfRangeException(nameof(seat), $"Місце {seat} не існує у {Hall.Name}.");

        if (!_bookedSeats.Add(seat))
            throw new InvalidOperationException($"Місце {seat} вже зайняте.");

        decimal finalPrice = pricingStrategy.CalculatePrice(BasePrice);

        var ticket = new Ticket
        {
            ScreeningId = Id,
            MovieTitle = Movie.Title,
            Seat = seat,
            Price = finalPrice,
            PricingType = pricingStrategy.Name,
            CustomerNote = customerNote
        };

        _tickets.Add(ticket);
        return ticket;
    }
}
