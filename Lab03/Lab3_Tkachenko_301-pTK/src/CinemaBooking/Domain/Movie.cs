namespace CinemaBooking.Domain;

public class Movie
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Title { get; init; }

    public required int Duration { get; init; }

    public string? Description { get; init; }

    public int AgeRating { get; init; }

    public string GetMovieCategory() => Duration switch
    {
        <= 0 => throw new InvalidOperationException("Тривалість фільму має бути додатною."),
        < 90 => "Short",
        >= 90 and <= 150 => "Standard",
        > 150 => "Long"
    };

    public string GetFormattedDescription() =>
        string.IsNullOrWhiteSpace(Description)
            ? "Опис відсутній"
            : Description;

    public override string ToString() =>
        $"{Title} ({Duration} хв, Категорія: {GetMovieCategory()}, {AgeRating}+) — {GetFormattedDescription()}";
}
