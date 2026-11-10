namespace CinemaLab6.Exceptions;

// Додаткове завдання: власний тип винятку для порушення правил предметної області кінотеатру
public class DomainRuleViolationException : InvalidOperationException
{
    public string RuleName { get; }
    public int ScreeningId { get; }
    public int? Row { get; }
    public int? Seat { get; }

    public DomainRuleViolationException(
        string ruleName,
        int screeningId,
        string message,
        int? row = null,
        int? seat = null)
        : base(message)
    {
        RuleName = ruleName;
        ScreeningId = screeningId;
        Row = row;
        Seat = seat;
    }
}
