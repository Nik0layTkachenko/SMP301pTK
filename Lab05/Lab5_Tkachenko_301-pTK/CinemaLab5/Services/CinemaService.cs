using CinemaLab5.Models;

namespace CinemaLab5.Services;

public class CinemaService
{
    private readonly List<Screening> _screenings = new();
    private readonly Action<string> _logger;
    private int _nextTicketId = 1;

    // Стандартний .NET event pattern
    public event EventHandler<Ticket>? TicketPurchased;

    public CinemaService(Action<string> logger)
    {
        _logger = logger;
    }

    // Доменна операція 1: реєстрація нового сеансу
    public void AddScreening(Screening screening)
    {
        _screenings.Add(screening);
        _logger($"Додано сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.HallName}), місць: {screening.TotalSeats}, базова ціна: {screening.BasePrice} грн.");
    }

    // Доменна операція 2: продаж квитка з передачею Func<decimal, decimal>
    public Ticket? PurchaseTicket(Screening screening, string customerName, string modifierName, Func<decimal, decimal> pricingModifier, int quantity = 1)
    {
        if (screening.SoldTickets + quantity > screening.TotalSeats)
        {
            _logger($"Відмова у продажі: недостатньо місць на сеанс "{screening.MovieTitle}" для {customerName}.");
            return null;
        }

        decimal pricePerTicket = pricingModifier(screening.BasePrice);
        decimal totalCost = pricePerTicket * quantity;
        screening.SoldTickets += quantity;

        var ticket = new Ticket(_nextTicketId++, screening, customerName, modifierName, quantity, totalCost);

        _logger($"Оформлено квиток #{ticket.TicketId} ({quantity} шт.) для {customerName} за тарифом '{modifierName}'. Сума: {totalCost} грн.");

        // Виклик події TicketPurchased
        TicketPurchased?.Invoke(this, ticket);

        return ticket;
    }

    // Доменна операція 3: відбір сеансів за Predicate<Screening>
    public List<Screening> GetScreeningsByCondition(Predicate<Screening> predicate)
    {
        _logger("Виконується перевірка сеансів за заданим предикатом...");
        return _screenings.FindAll(predicate);
    }

    // Доменна операція 4: зміна базової ціни сеансу
    public void ChangeBasePrice(Screening screening, decimal newBasePrice)
    {
        decimal oldPrice = screening.BasePrice;
        screening.BasePrice = newBasePrice;
        _logger($"Оновлено базову ціну сеансу "{screening.MovieTitle}": {oldPrice} грн -> {newBasePrice} грн.");
    }

    public IReadOnlyList<Screening> GetAllScreenings() => _screenings;
}
