using CinemaBooking.Domain;

namespace CinemaBooking.Services;

public class CinemaAnalyticsService(IEnumerable<Screening> screenings)
{
    public IReadOnlyList<Screening> GetSalesOpenScreenings() =>
        screenings
            .Where(s => s.Status == ScreeningStatus.SalesOpen)
            .OrderBy(s => s.StartTime)
            .ToList();

    public IReadOnlyList<Screening> GetTop5ScreeningsByRevenue() =>
        screenings
            .OrderByDescending(s => s.Revenue)
            .ThenByDescending(s => s.Tickets.Count)
            .Take(5)
            .ToList();

    public decimal GetTotalRevenue() =>
        screenings
            .SelectMany(s => s.Tickets)
            .Sum(t => t.Price);

    public decimal GetAverageTicketPrice()
    {
        var allTickets = screenings.SelectMany(s => s.Tickets).ToList();
        return allTickets.Count == 0 ? 0m : Math.Round(allTickets.Average(t => t.Price), 2);
    }

    public IReadOnlyList<(string MovieTitle, int TicketsCount)> GetTicketCountPerMovie() =>
        screenings
            .GroupBy(s => s.Movie.Title)
            .Select(g => (
                MovieTitle: g.Key,
                TicketsCount: g.SelectMany(s => s.Tickets).Count()
            ))
            .OrderByDescending(x => x.TicketsCount)
            .ToList();

    public IReadOnlyList<(string MovieTitle, string HallName, Seat Seat)> GetAllSoldSeats() =>
        screenings
            .SelectMany(
                s => s.Tickets,
                (screening, ticket) => (screening.Movie.Title, screening.Hall.Name, ticket.Seat))
            .ToList();

    public IReadOnlyList<(string MovieTitle, int TicketsSold, decimal MovieRevenue)> GetTop3MoviesByTicketsSold() =>
        screenings
            .GroupBy(s => s.Movie.Title)
            .Select(g => (
                MovieTitle: g.Key,
                TicketsSold: g.SelectMany(s => s.Tickets).Count(),
                MovieRevenue: g.SelectMany(s => s.Tickets).Sum(t => t.Price)
            ))
            .OrderByDescending(x => x.TicketsSold)
            .ThenByDescending(x => x.MovieRevenue)
            .Take(3)
            .ToList();

    public IReadOnlyList<Screening> GetHighOccupancyScreenings(double thresholdPercent = 80.0) =>
        screenings
            .Where(s => s.OccupancyPercentage > thresholdPercent)
            .OrderByDescending(s => s.OccupancyPercentage)
            .ToList();

    public bool HasAnySoldOutScreening() =>
        screenings.Any(s => s.IsSoldOut);

    public Dictionary<string, int> GetMovieTicketsSoldDictionary() =>
        screenings
            .GroupBy(s => s.Movie.Title)
            .ToDictionary(
                g => g.Key,
                g => g.SelectMany(s => s.Tickets).Count());
}
