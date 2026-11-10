namespace CinemaLab7.DTOs;

// DTO-модель звіту про сеанси (Варіант 4) без порушення інкапсуляції доменних класів
public sealed record ScreeningReport(
    int ScreeningId,
    string MovieTitle,
    DateTime StartsAt,
    int TicketsSold,
    decimal Revenue);
