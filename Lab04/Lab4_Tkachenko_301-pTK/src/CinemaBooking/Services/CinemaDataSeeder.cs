using CinemaBooking.Domain;
using CinemaBooking.Pricing;
using CinemaBooking.Repositories;

namespace CinemaBooking.Services;

public static class CinemaDataSeeder
{
    public static void Seed(
        InMemoryRepository<Movie> movieRepo,
        InMemoryRepository<Hall> hallRepo,
        InMemoryRepository<Screening> screeningRepo)
    {
        var dune2 = new Movie { Title = "Дюна: Частина друга", Duration = 166, AgeRating = 16, Description = "Фантастичний епос Дені Вільнева." };
        var shadows = new Movie { Title = "Тіні забутих предків", Duration = 115, AgeRating = 12, Description = "Класика українського кінематографа." };
        var oppenheimer = new Movie { Title = "Оппенгеймер", Duration = 180, AgeRating = 16, Description = "Біографічна драма Крістофера Нолана." };
        var mavka = new Movie { Title = "Мавка. Лісова пісня", Duration = 99, AgeRating = 6, Description = "Український анімаційний фентезі-фільм." };
        var shortChronicles = new Movie { Title = "Хроніки старого міста", Duration = 78, AgeRating = 12, Description = null };
        var interstellar = new Movie { Title = "Інтерстеллар", Duration = 169, AgeRating = 12, Description = "Космічна одіссея у пошуках нового дому." };

        Movie[] movies = [dune2, shadows, oppenheimer, mavka, shortChronicles, interstellar];
        foreach (var movie in movies)
            movieRepo.Add(movie);

        var vipHall = new Hall { Name = "VIP Камерний", RowsCount = 2, SeatsPerRow = 4 };
        var redHall = new Hall { Name = "Червоний зал", RowsCount = 3, SeatsPerRow = 5 };
        var blueHall = new Hall { Name = "Синій зал", RowsCount = 2, SeatsPerRow = 5 };
        var imaxHall = new Hall { Name = "IMAX Зал", RowsCount = 4, SeatsPerRow = 5 };

        Hall[] halls = [vipHall, redHall, blueHall, imaxHall];
        foreach (var hall in halls)
            hallRepo.Add(hall);

        TicketPricing[] pricings = [new StandardTicketPricing(), new StudentTicketPricing(), new VipTicketPricing()];
        var baseDate = DateTime.Today.AddHours(10);

        (Movie Movie, Hall Hall, int HourOffset, decimal BasePrice, int TicketsToSell, ScreeningStatus FinalStatus)[] configs =
        [
            (dune2, imaxHall, 0, 250m, 18, ScreeningStatus.SalesOpen),
            (dune2, vipHall, 2, 300m, 8, ScreeningStatus.SalesOpen),
            (dune2, redHall, 5, 220m, 12, ScreeningStatus.Finished),
            (interstellar, imaxHall, 8, 260m, 19, ScreeningStatus.SalesOpen),
            (interstellar, vipHall, 11, 320m, 8, ScreeningStatus.Finished),
            (interstellar, blueHall, 14, 210m, 7, ScreeningStatus.SalesOpen),
            (oppenheimer, imaxHall, 24, 240m, 17, ScreeningStatus.SalesOpen),
            (oppenheimer, redHall, 27, 200m, 10, ScreeningStatus.SalesOpen),
            (oppenheimer, blueHall, 30, 190m, 5, ScreeningStatus.Started),
            (mavka, redHall, 33, 160m, 14, ScreeningStatus.SalesOpen),
            (mavka, blueHall, 36, 150m, 9, ScreeningStatus.SalesOpen),
            (mavka, vipHall, 39, 220m, 4, ScreeningStatus.Scheduled),
            (shadows, blueHall, 48, 170m, 8, ScreeningStatus.SalesOpen),
            (shadows, vipHall, 51, 240m, 7, ScreeningStatus.SalesOpen),
            (shadows, redHall, 54, 180m, 0, ScreeningStatus.Cancelled),
            (shortChronicles, blueHall, 60, 140m, 6, ScreeningStatus.SalesOpen),
            (shortChronicles, vipHall, 63, 190m, 3, ScreeningStatus.Finished),
            (shortChronicles, redHall, 66, 150m, 0, ScreeningStatus.Scheduled)
        ];

        foreach (var cfg in configs)
        {
            var screening = new Screening
            {
                Movie = cfg.Movie,
                Hall = cfg.Hall,
                StartTime = baseDate.AddHours(cfg.HourOffset),
                BasePrice = cfg.BasePrice
            };

            if (cfg.TicketsToSell > 0 && cfg.FinalStatus != ScreeningStatus.Scheduled && cfg.FinalStatus != ScreeningStatus.Cancelled)
            {
                screening.ChangeStatus(ScreeningStatus.SalesOpen);

                int sold = 0;
                for (int r = 1; r <= cfg.Hall.RowsCount && sold < cfg.TicketsToSell; r++)
                {
                    for (int n = 1; n <= cfg.Hall.SeatsPerRow && sold < cfg.TicketsToSell; n++)
                    {
                        var pricing = pricings[(r + n) % pricings.Length];
                        screening.BookSeat(new Seat(r, n), pricing);
                        sold++;
                    }
                }
            }

            if (cfg.FinalStatus != screening.Status)
            {
                screening.ChangeStatus(cfg.FinalStatus);
            }

            screeningRepo.Add(screening);
        }
    }
}
