using System.Net.Http.Json;
using CinemaLab8.DTOs;

namespace CinemaLab8.Services;

// Сервіс завантаження демонстраційного довідника контактів відвідувачів через HTTP
public class VisitorContactsService
{
    private readonly HttpClient _httpClient;
    private readonly Action<string> _logger;

    public VisitorContactsService(HttpClient httpClient, Action<string> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
    }

    // Отримання одного контакту користувача за ID із підтримкою CancellationToken
    public async Task<ExternalUser?> GetUserAsync(int id, CancellationToken token)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        return await _httpClient.GetFromJsonAsync<ExternalUser>(
            $"users/{id}",
            token);
    }

    // Одночасне завантаження декількох контактів за допомогою Task.WhenAll
    public async Task<IReadOnlyList<ExternalUser>> LoadUsersConcurrentlyAsync(
        IEnumerable<int> userIds,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userIds);

        var tasks = userIds.Select(id => GetUserAsync(id, token));
        ExternalUser?[] results = await Task.WhenAll(tasks);

        var users = results
            .Where(u => u is not null)
            .Select(u => u!)
            .ToList();

        _logger($"Одночасно завантажено контактів (Task.WhenAll): {users.Count}");
        return users;
    }

    // Послідовне завантаження контактів (для порівняння часу виконання у додатковому завданні)
    public async Task<IReadOnlyList<ExternalUser>> LoadUsersSequentiallyAsync(
        IEnumerable<int> userIds,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userIds);

        var users = new List<ExternalUser>();
        foreach (int id in userIds)
        {
            ExternalUser? user = await GetUserAsync(id, token);
            if (user is not null)
            {
                users.Add(user);
            }
        }

        _logger($"Послідовно завантажено контактів (foreach + await): {users.Count}");
        return users;
    }
}
