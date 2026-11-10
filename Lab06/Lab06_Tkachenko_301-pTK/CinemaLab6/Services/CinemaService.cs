using CinemaLab6.Models;

namespace CinemaLab6.Services;

public class CinemaService
{
    private readonly List<Screening> _screenings = new();
    private readonly Action<string> _logger;
    private int _nextTicketId = 1;

    // Подія успішної купівлі квитка (з Лабораторної №5)
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
        _logger($"Додано сеанс #{screening.Id}: "{screening.MovieTitle}" ({screening.Hall.Name}), місць: {screening.TotalSeats}, базова ціна: {screening.BasePrice} грн.");
    }

    // Купівля квитка з повною валідацією вхідних даних та стану сеансу
    public Ticket PurchaseTicket(
        Screening screening,
        string customerName,
        int row,
        int seat,
        string modifierName,
        Func<decimal, decimal> pricingModifier)
    {
        // 1. Валідація вхідних аргументів перед будь-якими змінами стану
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
                $"Розрахована ціна квитка має бути більшою за нуль (отримано: {finalPrice} грн).");
        }

        // 2. Перевірка доступності місця та стану сеансу і бронювання місця
        // Якщо виникне виняток, кількість вільних місць не зміниться і подія TicketPurchased не виникне
        screening.ReserveSeat(row, seat);

        // 3. Створення квитка після успішного бронювання
        var ticket = new Ticket(_nextTicketId++, screening, customerName, row, seat, modifierName, finalPrice);

        _logger($"Оформлено квиток #{ticket.TicketId} для {customerName}: ряд {row}, місце {seat}, тариф '{modifierName}', ціна {finalPrice} грн.");

        // 4. Виклик події ЛИШЕ після успішного завершення всіх перевірок та оновлення стану
        TicketPurchased?.Invoke(this, ticket);

        return ticket;
    }

    public IReadOnlyList<Screening> GetAllScreenings() => _screenings;
}
