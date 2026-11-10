using CinemaLab6.Exceptions;

namespace CinemaLab6.Models;

public enum ScreeningStatus
{
    Scheduled,
    InProgress,
    Finished
}

public class Screening
{
    private readonly HashSet<(int Row, int Seat)> _occupiedSeats = new();

    public int Id { get; }
    public string MovieTitle { get; }
    public Hall Hall { get; }
    public DateTime StartTime { get; }
    public decimal BasePrice { get; private set; }
    public ScreeningStatus Status { get; private set; }

    public int TotalSeats => Hall.TotalSeats;
    public int SoldTickets => _occupiedSeats.Count;
    public int AvailableSeatsCount => TotalSeats - SoldTickets;

    public double OccupancyPercentage =>
        TotalSeats == 0 ? 0 : (double)SoldTickets / TotalSeats * 100.0;

    public Screening(int id, string movieTitle, Hall hall, DateTime startTime, decimal basePrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(movieTitle);
        ArgumentNullException.ThrowIfNull(hall);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(basePrice);

        Id = id;
        MovieTitle = movieTitle;
        Hall = hall;
        StartTime = startTime;
        BasePrice = basePrice;
        Status = ScreeningStatus.Scheduled;
    }

    public bool IsSeatOccupied(int row, int seat) => _occupiedSeats.Contains((row, seat));

    // Перевірка аргументів та стану об'єкта перед бронюванням місця
    public void ReserveSeat(int row, int seat)
    {
        // 1. Перевірка вхідних даних: додатні номери ряду й місця
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(row);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seat);

        // 2. Перевірка чи місце існує у вибраному залі
        if (!Hall.HasSeat(row, seat))
        {
            throw new ArgumentOutOfRangeException(
                nameof(seat),
                $"Місце (ряд {row}, місце {seat}) не існує у залі "{Hall.Name}" (рядів: {Hall.RowsCount}, місць у ряду: {Hall.SeatsPerRow}).");
        }

        // 3. Перевірка стану сеансу: заборона продажу після початку або завершення сеансу
        if (Status != ScreeningStatus.Scheduled)
        {
            throw new InvalidOperationException(
                $"Продаж квитків неможливий: сеанс "{MovieTitle}" уже розпочався або завершився (поточний статус: {Status}).");
        }

        // 4. Перевірка правила предметної області: заборона повторного бронювання вже зайнятого місця
        if (_occupiedSeats.Contains((row, seat)))
        {
            throw new DomainRuleViolationException(
                ruleName: "SeatAlreadyBooked",
                screeningId: Id,
                message: $"Місце (ряд {row}, місце {seat}) на сеанс "{MovieTitle}" вже зайняте.",
                row: row,
                seat: seat);
        }

        // Зміна стану об'єкта виконується лише після успішного проходження всіх перевірок
        _occupiedSeats.Add((row, seat));
    }

    public void StartScreening()
    {
        if (Status != ScreeningStatus.Scheduled)
        {
            throw new InvalidOperationException(
                $"Неможливо розпочати сеанс "{MovieTitle}", оскільки його поточний статус: {Status}.");
        }

        Status = ScreeningStatus.InProgress;
    }

    public void UpdateBasePrice(decimal newBasePrice)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(newBasePrice);

        if (Status != ScreeningStatus.Scheduled)
        {
            throw new InvalidOperationException(
                "Не можна змінювати базову ціну після початку або завершення сеансу.");
        }

        BasePrice = newBasePrice;
    }
}
