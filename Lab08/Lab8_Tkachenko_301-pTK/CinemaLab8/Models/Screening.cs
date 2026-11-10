using CinemaLab8.Exceptions;

namespace CinemaLab8.Models;

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

    public void ReserveSeat(int row, int seat)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(row);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seat);

        if (!Hall.HasSeat(row, seat))
        {
            throw new ArgumentOutOfRangeException(
                nameof(seat),
                $"Місце (ряд {row}, місце {seat}) не існує у залі "{Hall.Name}".");
        }

        if (Status != ScreeningStatus.Scheduled)
        {
            throw new InvalidOperationException(
                $"Продаж квитків неможливий: сеанс "{MovieTitle}" має статус {Status}.");
        }

        if (_occupiedSeats.Contains((row, seat)))
        {
            throw new DomainRuleViolationException(
                ruleName: "SeatAlreadyBooked",
                screeningId: Id,
                message: $"Місце (ряд {row}, місце {seat}) на сеанс "{MovieTitle}" вже зайняте.",
                row: row,
                seat: seat);
        }

        _occupiedSeats.Add((row, seat));
    }
}
