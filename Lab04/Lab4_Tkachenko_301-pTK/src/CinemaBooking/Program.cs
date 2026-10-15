using System.Text;
using CinemaBooking.Domain;
using CinemaBooking.Repositories;
using CinemaBooking.Services;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("=== Лабораторна робота №4: Collections, Generics та LINQ ===");
Console.WriteLine("Студент: Ткаченко Микола Віталійович | Група: 301-пТК | Варіант: 4 (Кінотеатр)\n");

var movieRepository = new InMemoryRepository<Movie>();
var hallRepository = new InMemoryRepository<Hall>();
var screeningRepository = new InMemoryRepository<Screening>();

CinemaDataSeeder.Seed(movieRepository, hallRepository, screeningRepository);

var tempMovie = new Movie { Title = "Тестовий фільм", Duration = 90, AgeRating = 12 };
movieRepository.Add(tempMovie);
movieRepository.Remove(tempMovie);

Console.WriteLine($"Завантажено у репозиторії: Фільмів = {movieRepository.GetAll().Count}, Залів = {hallRepository.GetAll().Count}, Сеансів = {screeningRepository.GetAll().Count}\n");

var analytics = new CinemaAnalyticsService(screeningRepository.GetAll());

Console.WriteLine("--- 1. Сеанси з відкритим продажем квитків (SalesOpen) ---");
foreach (var s in analytics.GetSalesOpenScreenings())
{
    Console.WriteLine($"  [{s.StartTime:dd.MM HH:mm}] \"{s.Movie.Title}\" ({s.Hall.Name}) — Продано: {s.Tickets.Count}/{s.Hall.TotalCapacity}, Статус: {s.GetSalesState()}");
}
Console.WriteLine();

Console.WriteLine("--- 2. Топ-5 сеансів за виручкою (Revenue) ---");
int rank = 1;
foreach (var s in analytics.GetTop5ScreeningsByRevenue())
{
    Console.WriteLine($"  #{rank++}: \"{s.Movie.Title}\" ({s.Hall.Name}, {s.StartTime:dd.MM HH:mm}) — Виручка: {s.Revenue:F2} грн (Квитків: {s.Tickets.Count})");
}
Console.WriteLine();

Console.WriteLine("--- 3 & 4. Загальна виручка та середня вартість квитка ---");
Console.WriteLine($"  Загальний Revenue кінотеатру: {analytics.GetTotalRevenue():F2} грн");
Console.WriteLine($"  Середня ціна квитка (Average): {analytics.GetAverageTicketPrice():F2} грн\n");

Console.WriteLine("--- 5. Кількість проданих квитків для кожного фільму (GroupBy) ---");
foreach (var item in analytics.GetTicketCountPerMovie())
{
    Console.WriteLine($"  - \"{item.MovieTitle}\": {item.TicketsCount} квитків");
}
Console.WriteLine();

var allSoldSeats = analytics.GetAllSoldSeats();
Console.WriteLine($"--- 6. Усі продані місця через SelectMany (Усього продано місць: {allSoldSeats.Count}) ---");
foreach (var seatInfo in allSoldSeats.Take(8))
{
    Console.WriteLine($"  Фільм: \"{seatInfo.MovieTitle}\" \vert{} {seatInfo.HallName} \vert{} {seatInfo.Seat}");
}
Console.WriteLine($"  ... та ще {allSoldSeats.Count - 8} проданих місць.\n");

Console.WriteLine("--- 7. Топ-3 фільми за кількістю проданих квитків ---");
int movieRank = 1;
foreach (var m in analytics.GetTop3MoviesByTicketsSold())
{
    Console.WriteLine($"  #{movieRank++}: \"{m.MovieTitle}\" — {m.TicketsSold} квитків (Сума: {m.MovieRevenue:F2} грн)");
}
Console.WriteLine();

Console.WriteLine("--- 8. Сеанси із заповненістю залу понад 80\% ---");
foreach (var s in analytics.GetHighOccupancyScreenings(80.0))
{
    Console.WriteLine($"  - \"{s.Movie.Title}\" ({s.Hall.Name}) — Заповненість: {s.OccupancyPercentage:F1}% ({s.Tickets.Count}/{s.Hall.TotalCapacity})");
}
Console.WriteLine();

Console.WriteLine("--- 9. Перевірка наявності Sold-Out сеансів через Any() ---");
Console.WriteLine($"  Чи існує хоча б один повністю розпроданий сеанс (Sold-Out): {analytics.HasAnySoldOutScreening()}\n");

Console.WriteLine("--- 10. Словник Dictionary<string, int> (MovieTitle -> TicketsSold) ---");
Dictionary<string, int> ticketsDictionary = analytics.GetMovieTicketsSoldDictionary();
foreach (var (movieTitle, ticketsSold) in ticketsDictionary)
{
    Console.WriteLine($"  [\"{movieTitle}\"] => {ticketsSold}");
}
