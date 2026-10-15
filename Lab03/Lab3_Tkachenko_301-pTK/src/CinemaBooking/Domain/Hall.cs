namespace CinemaBooking.Domain;

public class Hall
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; init; }

    public required int RowsCount { get; init; }

    public required int SeatsPerRow { get; init; }

    public int TotalCapacity => RowsCount * SeatsPerRow;

    public bool IsSeatValid(Seat seat)
    {
        ArgumentNullException.ThrowIfNull(seat);
        return seat.Row >= 1 && seat.Row <= RowsCount &&
               seat.Number >= 1 && seat.Number <= SeatsPerRow;
    }

    public override string ToString() =>
        $"Зал \"{Name}\" ({RowsCount} рядів x {SeatsPerRow} місць, усього: {TotalCapacity})";
}
