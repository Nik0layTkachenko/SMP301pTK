using System.Text;
using CinemaLab6.Exceptions;
using CinemaLab6.Extensions;
using CinemaLab6.Models;
using CinemaLab6.Services;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("============================================================");
Console.WriteLine("Лабораторна робота №6: Обробка винятків та валідація даних");
Console.WriteLine("Виконав: Ткаченко Микола Віталійович | Група: 301-пТК | Варіант 4");
Console.WriteLine("Предметна область: Кінотеатр (.NET 10 / C# 14)");
Console.WriteLine("============================================================
");

Action<string> logger = message =>
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");

var cinemaService = new CinemaService(logger);
var receiptService = new ReceiptService();
var statisticsService = new StatisticsService();

cinemaService.TicketPurchased += receiptService.Handle;
cinemaService.TicketPurchased += statisticsService.Handle;

// Створюємо зал на 3 ряди по 5 місць (усього 15 місць) та сеанс
var hall = new Hall(1, "Зал IMAX", rowsCount: 3, seatsPerRow: 5);
var screening = new Screening(1, "Дюна: Частина друга", hall, new DateTime(2026, 11, 20, 19, 0, 0), 200m);
cinemaService.AddScreening(screening);
Console.WriteLine();

// Допоміжний метод для наочної демонстрації операцій і перевірки цілісності стану
void ExecutePurchaseAttempt(
    string operationTitle,
    string customerName,
    int row,
    int seat,
    string tariffName,
    Func<decimal, decimal> modifier)
{
    Console.WriteLine($"Operation: {operationTitle}");
    int eventsBefore = statisticsService.TicketsSoldCount;

    try
    {
        cinemaService.PurchaseTicket(screening, customerName, row, seat, tariffName, modifier);
        Console.WriteLine("Result: Квиток успішно оформлено.");
    }
    catch (DomainRuleViolationException ex)
    {
        Console.WriteLine($"Error [DomainRuleViolationException - {ex.RuleName}]: {ex.Message}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Error [ArgumentOutOfRangeException]: {ex.Message}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error [ArgumentException]: {ex.Message}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine($"Error [InvalidOperationException]: {ex.Message}");
    }
    finally
    {
        bool eventTriggered = statisticsService.TicketsSoldCount > eventsBefore;
        Console.WriteLine($"Screening status: {screening.Status}");
        Console.WriteLine($"Available seats: {screening.AvailableSeatsCount}/{screening.TotalSeats} | TicketPurchased triggered: {eventTriggered}
");
    }
}

// 1. КОРЕКТНА ОПЕРАЦІЯ: бронювання вільного місця (ряд 2, місце 3)
ExecutePurchaseAttempt(
    "PurchaseTicket(customer = "Олександр Коваль", row = 2, seat = 3, price = Regular)",
    "Олександр Коваль",
    row: 2,
    seat: 3,
    tariffName: "Regular",
    modifier: PricingPolicies.Regular);

// 2. НЕКОРЕКТНА ОПЕРАЦІЯ №1: від'ємний номер ряду або місця (ArgumentOutOfRangeException)
ExecutePurchaseAttempt(
    "PurchaseTicket(customer = "Марія Бондар", row = -1, seat = 4, price = Student)",
    "Марія Бондар",
    row: -1,
    seat: 4,
    tariffName: "Student",
    modifier: PricingPolicies.Student);

// 3. НЕКОРЕКТНА ОПЕРАЦІЯ №2: місце, якого немає у вибраному залі (ArgumentOutOfRangeException)
ExecutePurchaseAttempt(
    "PurchaseTicket(customer = "Ігор Мельник", row = 5, seat = 10, price = Regular)",
    "Ігор Мельник",
    row: 5,
    seat: 10,
    tariffName: "Regular",
    modifier: PricingPolicies.Regular);

// 4. НЕКОРЕКТНА ОПЕРАЦІЯ №3: повторне бронювання вже зайнятого місця (DomainRuleViolationException)
ExecutePurchaseAttempt(
    "PurchaseTicket(customer = "Анна Шевченко", row = 2, seat = 3 [вже зайняте], price = Weekend)",
    "Анна Шевченко",
    row: 2,
    seat: 3,
    tariffName: "Weekend",
    modifier: PricingPolicies.Weekend);

// 5. НЕКОРЕКТНА ОПЕРАЦІЯ №4: некоректна (нульова) ціна квитка (ArgumentOutOfRangeException)
ExecutePurchaseAttempt(
    "PurchaseTicket(customer = "Дмитро Ткач", row = 1, seat = 1, price = 0 грн)",
    "Дмитро Ткач",
    row: 1,
    seat: 1,
    tariffName: "InvalidZero",
    modifier: PricingPolicies.InvalidZeroPrice);

// 6. НЕКОРЕКТНА ОПЕРАЦІЯ №5: продаж квитка після початку сеансу (InvalidOperationException)
Console.WriteLine("--- Розпочинаємо показ фільму (зміна стану сеансу на InProgress) ---");
screening.StartScreening();

ExecutePurchaseAttempt(
    "PurchaseTicket(customer = "Сергій Лисенко", row = 1, seat = 2, after screening started)",
    "Сергій Лисенко",
    row: 1,
    seat: 2,
    tariffName: "Regular",
    modifier: PricingPolicies.Regular);

// 7. Демонстрація безпечного числового введення через int.TryParse
Console.WriteLine("--- Перевірка числового введення (int.TryParse) ---");
string[] sampleInputs = ["abc", "2"];
foreach (string rawInput in sampleInputs)
{
    Console.WriteLine($"Введене значення номера ряду: "{rawInput}"");
    if (!int.TryParse(rawInput, out int parsedRow))
    {
        Console.WriteLine(" -> Помилка введення: введіть коректне ціле число (TryParse повернув false).");
    }
    else
    {
        Console.WriteLine($" -> Число успішно розпізнано: ряд №{parsedRow}.");
    }
}

Console.WriteLine("
--- Підсумковий стан сеансу ---");
Console.WriteLine(screening.GetSummary());
