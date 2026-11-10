namespace CinemaLab8.DTOs;

public sealed record ScreeningReport(
    int ScreeningId,
    string MovieTitle,
    DateTime StartsAt,
    int TicketsSold,
    decimal Revenue);
