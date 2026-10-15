namespace CinemaBooking.Domain;

public record Seat(int Row, int Number)
{
    public void Validate()
    {
        if (Row <= 0)
            throw new ArgumentOutOfRangeException(nameof(Row), "Номер ряду має бути більшим за 0.");

        if (Number <= 0)
            throw new ArgumentOutOfRangeException(nameof(Number), "Номер місця має бути більшим за 0.");
    }

    public override string ToString() => $"Ряд {Row}, Місце {Number}";
}
