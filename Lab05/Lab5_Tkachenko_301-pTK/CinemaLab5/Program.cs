using System.Text;
using CinemaLab5.Extensions;
using CinemaLab5.Models;
using CinemaLab5.Services;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("============================================================");
Console.WriteLine("Лабораторна робота №5: Delegates, Lambdas, Events, Extensions");
Console.WriteLine("Виконав: Ткаченко Микола Віталійович | Група: 301-пТК | Варіант 4");
Console.WriteLine("Предметна область: Кінотеатр (.NET 10 / C# 14)");
Console.WriteLine("============================================================
");

// 1. Action<string> logger через лямбда-вираз
Action<string> logger =
    message => Console.WriteLine(
        $"[{DateTime.Now:HH:mm:ss}] {message}");

// 2. Створення сервісу кінотеатру (Publisher) та підписників (Subscribers)
var service = new CinemaService(logger);
var receiptService = new ReceiptService();
var statisticsService = new StatisticsService();

// Підписка двох subscribers на подію TicketPurchased
service.TicketPurchased += receiptService.Handle;
service.TicketPurchased += statisticsService.Handle;

// 3. Додавання сеансів (Доменна операція 1)
var screening1 = new Screening(1, "Дюна: Частина друга", "Зал 1 (IMAX)", new DateTime(2026, 11, 14, 19, 0, 0), 200m, 10);
var screening2 = new Screening(2, "Інтерстеллар", "Зал 2", new DateTime(2026, 11, 15, 20, 30, 0), 150m, 20);

service.AddScreening(screening1);
service.AddScreening(screening2);
Console.WriteLine();

// 4. Купівля квитків із використанням Func<decimal, decimal> (Доменна операція 2)
Console.WriteLine("--- Продаж квитків (Func + Event + Subscribers) ---");
service.PurchaseTicket(screening1, "Олександр Коваль", "Regular", PricingPolicies.Regular, quantity: 3);
service.PurchaseTicket(screening1, "Марія Бондар", "Student (-20%)", PricingPolicies.Student, quantity: 3);
service.PurchaseTicket(screening1, "Ігор Мельник", "Weekend (+20%)", PricingPolicies.Weekend, quantity: 2);

service.PurchaseTicket(screening2, "Анна Шевченко", "Student (-20%)", PricingPolicies.Student, quantity: 4);
Console.WriteLine();

// 5. Оновлення базової ціни сеансу (Доменна операція 4)
service.ChangeBasePrice(screening2, 170m);
Console.WriteLine();

// 6. Використання Predicate<Screening> для пошуку сеансів із заповненістю >= 80% (Доменна операція 3)
Console.WriteLine("--- Фільтрація сеансів із заповненістю >= 80% (Predicate) ---");
var busyScreenings = service.GetScreeningsByCondition(PricingPolicies.OccupancyAtLeast80Percent);
foreach (var s in busyScreenings)
{
    Console.WriteLine($" -> Сеанс із високою заповненістю: "{s.MovieTitle}" ({s.OccupancyPercentage:F1}%)");
}
Console.WriteLine();

// 7. Використання Extension Methods та C# 14 extension block
Console.WriteLine("--- Підсумкове зведення по сеансах (Extension Methods) ---");
foreach (var s in service.GetAllScreenings())
{
    Console.WriteLine(s.GetSummary());
    Console.WriteLine($"   Перевірка розширень: IsAlmostFull = {s.IsAlmostFull()} | Вільних місць (C# 14) = {s.RemainingSeats} | IsSoldOut (C# 14) = {s.IsSoldOut()}");
}
