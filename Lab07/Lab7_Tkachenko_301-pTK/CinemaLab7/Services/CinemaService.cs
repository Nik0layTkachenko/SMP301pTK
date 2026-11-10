using CinemaLab7.DTOs;
using CinemaLab7.Models;

namespace CinemaLab7.Services;

public class CinemaService
{
    private readonly List<Screening> _screenings = new();
    private readonly List<Ticket> _tickets = new();
    private readonly Action<string> _logger;
    private int _nextTicketId = 1;

    public event EventHandler<Ticket>? TicketPurchased;

    public CinemaService(Action<string> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public void AddScreening(Screening screening)
    {
        ArgumentNullException.ThrowIfNull(screening);
        _screenings.Add(screening);
        _logger($"Додано сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.Hall.Name}), початок: {screening.StartTime:dd.MM.yyyy HH:mm}, базова ціна: {screening.BasePrice} грн.");
    }

    public Ticket PurchaseTicket(
        Screening screening,
        string customerName,
        int row,
        int seat,
        string modifierName,
        Func<decimal, decimal> pricingModifier)
    {
        ArgumentNullException.ThrowIfNull(screening);
        ArgumentException.ThrowIfNullOrWhiteSpace(customerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(modifierName);
        ArgumentNullException.ThrowIfNull(pricingModifier);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(row);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seat);

        decimal finalPrice = pricingModifier(screening.BasePrice);
        if (finalPrice <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(finalPrice),
                "Ціна квитка повинна бути додатною.");
        }

        screening.ReserveSeat(row, seat);

        var ticket = new Ticket(_nextTicketId++, screening, customerName, row, seat, modifierName, finalPrice);
        _tickets.Add(ticket);

        _logger($"Продано квиток #{ticket.TicketId} ({customerName}): "{screening.MovieTitle}", ряд {row}, місце {seat}, ціна {finalPrice} грн.");

        TicketPurchased?.Invoke(this, ticket);
        return ticket;
    }

    // Формування списку звітних DTO-записів на основі актуальних даних Screening та Ticket
    public List<ScreeningReport> BuildScreeningReports()
    {
        var reports = new List<ScreeningReport>();

        foreach (var screening in _screenings)
        {
            decimal screeningRevenue = _tickets
                .Where(t => t.Screening.Id == screening.Id)
                .Sum(t => t.FinalPrice);

            reports.Add(new ScreeningReport(
                ScreeningId: screening.Id,
                MovieTitle: screening.MovieTitle,
                StartsAt: screening.StartTime,
                TicketsSold: screening.SoldTickets,
                Revenue: screeningRevenue));
        }

        return reports;
    }

    public IReadOnlyList<Screening> GetAllScreenings() => _screenings;
    public IReadOnlyList<Ticket> GetAllTickets() => _tickets;
}
