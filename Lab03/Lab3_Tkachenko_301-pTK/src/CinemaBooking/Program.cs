using System.Text;
using CinemaBooking.Domain;
using CinemaBooking.Pricing;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Лабораторна робота №3: Сучасне моделювання даних у C# ===");
Console.WriteLine("Студент: Ткаченко Микола Віталійович | Група: 301-пТК | Варіант: 4 (Кінотеатр)\n");

Console.WriteLine("--- 1. Перевірка record Seat (Value Equality) ---");
var seatA = new Seat(Row: 3, Number: 7);
var seatB = new Seat(Row: 3, Number: 7);
var seatC = seatA with { Number = 8 };

Console.WriteLine($"seatA: {seatA}");
Console.WriteLine($"seatB: {seatB}");
Console.WriteLine($"seatA == seatB (однакові значення): {seatA == seatB}");
Console.WriteLine($"seatC (створено через with): {seatC}, seatA == seatC: {seatA == seatC}\n");

Console.WriteLine("--- 2. Фільми, Nullable Description та Relational Pattern (Категорія тривалості) ---");
var shortMovie = new Movie { Title = "Хроніки старого міста", Duration = 75, Description = null, AgeRating = 12 };
var standardMovie = new Movie { Title = "Тіні забутих предків", Duration = 115, Description = "Класика українського поетичного кіно.", AgeRating = 12 };
var longMovie = new Movie { Title = "Дюна: Частина друга", Duration = 166, Description = "Епічне науково-фантастичне продовження.", AgeRating = 16 };

Movie[] movies = [shortMovie, standardMovie, longMovie];
foreach (var movie in movies)
{
    Console.WriteLine($"- {movie}");
}
Console.WriteLine();

Console.WriteLine("--- 3. Статуси сеансу (switch expression) та Property Pattern (SoldOut) ---");
var chamberHall = new Hall { Name = "Камерний VIP-зал", RowsCount = 1, SeatsPerRow = 2 };
var screening = new Screening { Movie = longMovie, Hall = chamberHall, StartTime = DateTime.Now.AddHours(3), BasePrice = 200m };

Console.WriteLine($"Початковий статус: {screening.GetScreeningStatusDescription()} \vert{} Стан продажу: {screening.GetSalesState()}");

screening.ChangeStatus(ScreeningStatus.SalesOpen);
Console.WriteLine($"Після відкриття:  {screening.GetScreeningStatusDescription()} | Стан продажу: {screening.GetSalesState()} (Вільних місць: {screening.AvailableSeats})");

TicketPricing[] pricings = [new StandardTicketPricing(), new StudentTicketPricing(), new VipTicketPricing()];
var ticket1 = screening.BookSeat(new Seat(1, 1), pricings[1], "Студентський квиток №12345");
var ticket2 = screening.BookSeat(new Seat(1, 2), pricings[2], null);

Console.WriteLine("\nОформлені квитки:");
Console.WriteLine($"  {ticket1}");
Console.WriteLine($"  {ticket2}");

Console.WriteLine($"\nПісля викупу всіх місць:");
Console.WriteLine($"Статус сеансу: {screening.GetScreeningStatusDescription()}");
Console.WriteLine($"Вільних місць: {screening.AvailableSeats}");
Console.WriteLine($"Результат Property Pattern (GetSalesState): {screening.GetSalesState()}");

screening.ChangeStatus(ScreeningStatus.Started);
Console.WriteLine($"Початок показу: {screening.GetScreeningStatusDescription()} \vert{} Стан продажу: {screening.GetSalesState()}");

screening.ChangeStatus(ScreeningStatus.Finished);
Console.WriteLine($"Кінець показу:  {screening.GetScreeningStatusDescription()} | Стан продажу: {screening.GetSalesState()}");
