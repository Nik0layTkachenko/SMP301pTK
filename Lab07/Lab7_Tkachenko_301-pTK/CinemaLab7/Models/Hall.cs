namespace CinemaLab7.Models;

public class Hall
{
    public int Id { get; }
    public string Name { get; }
    public int RowsCount { get; }
    public int SeatsPerRow { get; }

    public int TotalSeats => RowsCount * SeatsPerRow;

    public Hall(int id, string name, int rowsCount, int seatsPerRow)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowsCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seatsPerRow);

        Id = id;
        Name = name;
        RowsCount = rowsCount;
        SeatsPerRow = seatsPerRow;
    }

    public bool HasSeat(int row, int seat)
    {
        return row >= 1 && row <= RowsCount && seat >= 1 && seat <= SeatsPerRow;
    }
}
