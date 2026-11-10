using System.Text;
using CinemaLab7.DTOs;
using CinemaLab7.Models;
using CinemaLab7.Services;
using Microsoft.Extensions.Configuration;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("============================================================");
Console.WriteLine("Лабораторна робота №7: JSON, робота з файлами та конфігурація");
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

string relativePath = config["Storage:FilePath"]
    ?? throw new InvalidOperationException("Storage:FilePath is not configured.");

bool writeIndented = !bool.TryParse(config["Storage:WriteIndented"], out bool parsedIndented) || parsedIndented;

string filePath = Path.Combine(AppContext.BaseDirectory, relativePath);

Console.WriteLine("--- Параметри конфігурації (appsettings.json) ---");
Console.WriteLine($"Storage:FilePath      = {relativePath}");
Console.WriteLine($"Storage:WriteIndented = {writeIndented}");
Console.WriteLine($"Повний шлях до файлу  = {filePath}
");

// 2. Ініціалізація сервісів та створення предметних даних (5 сеансів)
var cinemaService = new CinemaService(logger);
var receiptService = new ReceiptService();
var statisticsService = new StatisticsService();
var storageService = new ReportStorageService(logger);

cinemaService.TicketPurchased += receiptService.Handle;
cinemaService.TicketPurchased += statisticsService.Handle;

var imaxHall = new Hall(1, "Зал IMAX", rowsCount: 5, seatsPerRow: 10);
var standardHall = new Hall(2, "Зал Комфорт", rowsCount: 4, seatsPerRow: 8);

var s1 = new Screening(1, "Дюна: Частина друга", imaxHall, new DateTime(2026, 11, 21, 18, 0, 0), 220m);
var s2 = new Screening(2, "Інтерстеллар", imaxHall, new DateTime(2026, 11, 21, 21, 30, 0), 200m);
var s3 = new Screening(3, "Оппенгеймер", standardHall, new DateTime(2026, 11, 22, 17, 0, 0), 180m);
var s4 = new Screening(4, "Бетмен", standardHall, new DateTime(2026, 11, 22, 20, 0, 0), 160m);
var s5 = new Screening(5, "Аватар: Шлях води", imaxHall, new DateTime(2026, 11, 23, 19, 0, 0), 250m);

cinemaService.AddScreening(s1);
cinemaService.AddScreening(s2);
cinemaService.AddScreening(s3);
cinemaService.AddScreening(s4);
cinemaService.AddScreening(s5);
Console.WriteLine();

// Продаж квитків на всі 5 сеансів
Console.WriteLine("--- Оформлення квитків ---");
cinemaService.PurchaseTicket(s1, "Олександр Коваль", 2, 5, "Regular", PricingPolicies.Regular);
cinemaService.PurchaseTicket(s1, "Марія Бондар", 2, 6, "Student", PricingPolicies.Student);
cinemaService.PurchaseTicket(s1, "Ігор Мельник", 3, 4, "Weekend", PricingPolicies.Weekend);

cinemaService.PurchaseTicket(s2, "Анна Шевченко", 1, 1, "Regular", PricingPolicies.Regular);
cinemaService.PurchaseTicket(s2, "Дмитро Ткач", 1, 2, "Student", PricingPolicies.Student);

cinemaService.PurchaseTicket(s3, "Олена Кравченко", 2, 3, "Weekend", PricingPolicies.Weekend);
cinemaService.PurchaseTicket(s3, "Василь Романюк", 2, 4, "Weekend", PricingPolicies.Weekend);

cinemaService.PurchaseTicket(s4, "Наталія Сидоренко", 4, 2, "Student", PricingPolicies.Student);

cinemaService.PurchaseTicket(s5, "Сергій Лисенко", 3, 7, "Regular", PricingPolicies.Regular);
cinemaService.PurchaseTicket(s5, "Юлія Поліщук", 3, 8, "Weekend", PricingPolicies.Weekend);
Console.WriteLine();

// 3. Формування звіту (5 записів ScreeningReport) та збереження у JSON-файл
Console.WriteLine("--- Експорт звіту про сеанси у JSON ---");
List<ScreeningReport> reportsToSave = cinemaService.BuildScreeningReports();
await storageService.SaveReportsAsync(reportsToSave, filePath, writeIndented);

string rawJsonContent = await File.ReadAllTextAsync(filePath);
Console.WriteLine("
Вміст створеного JSON-файлу:");
Console.WriteLine(rawJsonContent);
Console.WriteLine();

// 4. Повторне зчитування з JSON-файлу, десеріалізація та виведення у консоль
Console.WriteLine("--- Імпорт та десеріалізація звіту з JSON-файлу ---");
List<ScreeningReport>? loadedReports = await storageService.LoadReportsAsync(filePath);

if (loadedReports is not null)
{
    foreach (var item in loadedReports)
    {
        Console.WriteLine($"Сеанс #{item.ScreeningId} | Фільм: "{item.MovieTitle}" | " +
                          $"Початок: {item.StartsAt:dd.MM.yyyy HH:mm} | Продано квитків: {item.TicketsSold} | Дохід: {item.Revenue} грн");
    }

    decimal totalRevenue = loadedReports.Sum(r => r.Revenue);
    int totalTickets = loadedReports.Sum(r => r.TicketsSold);
    Console.WriteLine($"------------------------------------------------------------");
    Console.WriteLine($"Загальна кількість проданих квитків зі звіту: {totalTickets}");
    Console.WriteLine($"Загальний дохід від квитків зі звіту:         {totalRevenue} грн
");
}

// 5. Демонстрація коректної обробки помилок (відсутній файл та пошкоджений JSON)
Console.WriteLine("--- Перевірка обробки помилок читання файлів та JSON ---");

string missingFilePath = Path.Combine(AppContext.BaseDirectory, "data", "non-existent-report.json");
Console.WriteLine($"1) Спроба читання відсутнього файлу: {missingFilePath}");
await storageService.LoadReportsAsync(missingFilePath);

string corruptedFilePath = Path.Combine(AppContext.BaseDirectory, "data", "corrupted-report.json");
await File.WriteAllTextAsync(corruptedFilePath, "{ "ScreeningId": 1, "MovieTitle": "Broken JSON" ");
Console.WriteLine($"
2) Спроба читання файлу з некоректним JSON: {corruptedFilePath}");
await storageService.LoadReportsAsync(corruptedFilePath);

// Прибираємо тестовий пошкоджений файл після демонстрації
File.Delete(corruptedFilePath);
Console.WriteLine("
Роботу програми успішно завершено без аварійних збоїв.");
