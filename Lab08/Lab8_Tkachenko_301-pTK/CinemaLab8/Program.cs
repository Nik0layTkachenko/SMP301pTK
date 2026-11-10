using System.Diagnostics;
using System.Text;
using System.Text.Json;
using CinemaLab8.DTOs;
using CinemaLab8.Extensions;
using CinemaLab8.Models;
using CinemaLab8.Services;
using Microsoft.Extensions.Configuration;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("============================================================");
Console.WriteLine("Лабораторна робота №8: Асинхронне програмування та HTTP");
Console.WriteLine("Виконав: Ткаченко Микола Віталійович | Група: 301-пТК | Варіант 4");
Console.WriteLine("Предметна область: Кінотеатр (.NET 10 / C# 14)");
Console.WriteLine("============================================================
");

Action<string> logger = message =>
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");

// 1. Зчитування конфігурації з appsettings.json
string configFile = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
var config = new ConfigurationBuilder()
    .AddJsonFile(configFile, optional: false)
    .Build();

string baseUrl = config["ExternalApi:BaseUrl"]
    ?? throw new InvalidOperationException("ExternalApi:BaseUrl is not configured.");

int timeoutSeconds = int.TryParse(config["ExternalApi:TimeoutSeconds"], out int parsedTimeout)
    ? parsedTimeout
    : 10;

Console.WriteLine("--- Налаштування ExternalApi (appsettings.json) ---");
Console.WriteLine($"BaseUrl        = {baseUrl}");
Console.WriteLine($"TimeoutSeconds = {timeoutSeconds} с
");

// 2. Створення основних доменних сутностей кінотеатру
var cinemaService = new CinemaService(logger);
var imaxHall = new Hall(1, "Зал IMAX", rowsCount: 5, seatsPerRow: 10);
var screening = new Screening(1, "Дюна: Частина друга", imaxHall, new DateTime(2026, 11, 25, 19, 0, 0), 220m);
cinemaService.AddScreening(screening);
Console.WriteLine();

// 3. Використання одного екземпляра HttpClient для завантаження контактів відвідувачів
using var httpClient = new HttpClient
{
    BaseAddress = new Uri(baseUrl)
};

var contactsService = new VisitorContactsService(httpClient, logger);
int[] targetUserIds = [1, 2, 3];

Console.WriteLine("--- Завантаження довідника контактів відвідувачів (Task.WhenAll) ---");
Console.WriteLine("Loading external contacts...");

try
{
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
    var swConcurrent = Stopwatch.StartNew();

    IReadOnlyList<ExternalUser> contacts = await contactsService.LoadUsersConcurrentlyAsync(targetUserIds, cts.Token);

    swConcurrent.Stop();
    Console.WriteLine($"Received: {contacts.Count} (час виконання Task.WhenAll: {swConcurrent.ElapsedMilliseconds} мс)");

    Console.WriteLine("
Список контактів відвідувачів для майбутніх повідомлень про сеанси:");
    foreach (var contact in contacts)
    {
        Console.WriteLine($"{contact.Id} | {contact.Name} | {contact.Email}");
    }
}
catch (OperationCanceledException)
{
    Console.WriteLine("[Помилка скасування - OperationCanceledException]: Час очікування відповіді від сервера вичерпано.");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"[Мережева помилка - HttpRequestException]: Не вдалося завантажити контакти. Деталі: {ex.Message}");
}
catch (JsonException ex)
{
    Console.WriteLine($"[Помилка формату даних - JsonException]: Отримано некоректний JSON. Деталі: {ex.Message}");
}

// Перевірка, що завантаження зовнішніх контактів не створило Ticket і не забронювало Seat автоматично
Console.WriteLine("
--- Перевірка незмінності стану доменних сутностей після імпорту контактів ---");
Console.WriteLine(screening.GetSummary());
Console.WriteLine($"Кількість створених квитків (Ticket): {cinemaService.GetAllTickets().Count}");
Console.WriteLine($"Вільних місць у залі (Seat):          {screening.RemainingSeats}/{screening.TotalSeats}
");

// 4. Додаткове завдання: порівняння тривалості 3 послідовних запитів і 3 одночасних запитів через Task.WhenAll
Console.WriteLine("--- Додаткове завдання: Послідовні запити vs Task.WhenAll ---");
try
{
    using var benchmarkCts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));

    var swSeq = Stopwatch.StartNew();
    await contactsService.LoadUsersSequentiallyAsync(targetUserIds, benchmarkCts.Token);
    swSeq.Stop();

    var swPar = Stopwatch.StartNew();
    await contactsService.LoadUsersConcurrentlyAsync(targetUserIds, benchmarkCts.Token);
    swPar.Stop();

    Console.WriteLine($"Тривалість 3 послідовних запитів (foreach + await): {swSeq.ElapsedMilliseconds} мс");
    Console.WriteLine($"Тривалість 3 одночасних запитів (Task.WhenAll):     {swPar.ElapsedMilliseconds} мс
");
}
catch (Exception ex) when (ex is OperationCanceledException or HttpRequestException or JsonException)
{
    Console.WriteLine($"Помилка під час порівняння швидкодії: {ex.Message}
");
}

// 5. Демонстрація обробки помилки мережі та скасування за тайм-аутом
Console.WriteLine("--- Демонстрація обробки помилок HTTP та CancellationToken ---");

// 5.1. Перевірка HttpRequestException на неіснуючій адресі сервера
Console.WriteLine("1) Запит до неіснуючого хоста (перевірка HttpRequestException):");
using var invalidClient = new HttpClient
{
    BaseAddress = new Uri("https://invalid-cinema-contacts.typicode.invalid/")
};
var invalidContactsService = new VisitorContactsService(invalidClient, logger);

try
{
    using var errorCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
    await invalidContactsService.GetUserAsync(1, errorCts.Token);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($" -> Перехоплено HttpRequestException: {ex.Message}");
}
catch (OperationCanceledException)
{
    Console.WriteLine(" -> Перехоплено OperationCanceledException під час звернення до неіснуючого сервера.");
}

// 5.2. Перевірка OperationCanceledException при миттєвому скасуванні токена
Console.WriteLine("
2) Запит зі скасованим CancellationToken (перевірка OperationCanceledException):");
try
{
    using var canceledCts = new CancellationTokenSource();
    canceledCts.Cancel();
    await contactsService.LoadUsersConcurrentlyAsync(targetUserIds, canceledCts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine(" -> Перехоплено OperationCanceledException: операцію коректно скасовано через CancellationToken.");
}

Console.WriteLine("
Роботу застосунку завершено штатно.");
