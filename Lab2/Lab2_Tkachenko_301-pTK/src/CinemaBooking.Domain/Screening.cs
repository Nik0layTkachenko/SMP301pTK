using System;
using System.Collections.Generic;
using System.Linq;

namespace CinemaBooking.Domain;

public class Screening
{
    public Movie Movie { get; }
    public Hall Hall { get; }
    public DateTime StartTime { get; }
    public ScreeningStatus Status { get; private set; }

    private readonly List<Ticket> _tickets = new();
    public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

    public Screening(Movie movie, Hall hall, DateTime startTime)
    {
        Movie = movie;
        Hall = hall;
        StartTime = startTime;
        Status = ScreeningStatus.Scheduled;
    }

    public void ReserveSeat(int seatNumber, decimal price)
    {
        if (Status != ScreeningStatus.Scheduled)
            throw new InvalidOperationException("Бронювання можливе лише до початку сеансу.");
        
        if (seatNumber <= 0 || seatNumber > Hall.TotalSeats)
            throw new ArgumentOutOfRangeException(nameof(seatNumber), "Некоректний номер місця.");

        if (_tickets.Any(t => t.SeatNumber == seatNumber))
            throw new InvalidOperationException("Це місце вже заброньовано.");

        _tickets.Add(new Ticket(seatNumber, price));
    }

    public void CancelReservation(int seatNumber)
    {
        if (Status != ScreeningStatus.Scheduled)
            throw new InvalidOperationException("Після початку сеансу квитки не повертаються.");

        var ticket = _tickets.FirstOrDefault(t => t.SeatNumber == seatNumber);
        if (ticket == null)
            throw new InvalidOperationException("Квиток на це місце не знайдено.");

        _tickets.Remove(ticket);
    }

    public void Start()
    {
        if (Status != ScreeningStatus.Scheduled)
            throw new InvalidOperationException("Сеанс уже розпочато або завершено.");
        Status = ScreeningStatus.Started;
    }

    public void Finish()
    {
        if (Status != ScreeningStatus.Started)
            throw new InvalidOperationException("Можна завершити лише розпочатий сеанс.");
        Status = ScreeningStatus.Finished;
    }
}
