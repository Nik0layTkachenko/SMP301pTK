namespace CinemaBooking.Repositories;

public class InMemoryRepository<T> where T : class
{
    private readonly List<T> _items = [];

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        _items.Add(item);
    }

    public bool Remove(T item)
    {
        ArgumentNullException.ThrowIfNull(item);
        return _items.Remove(item);
    }

    public IReadOnlyCollection<T> GetAll() => _items.AsReadOnly();
}
