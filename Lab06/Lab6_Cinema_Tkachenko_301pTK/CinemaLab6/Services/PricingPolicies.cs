using CinemaLab6.Models;

namespace CinemaLab6.Services;

public static class PricingPolicies
{
    // Func<decimal, decimal> - модифікатори ціни з Лабораторної №5
    public static readonly Func<decimal, decimal> Regular =
        price => price;

    public static readonly Func<decimal, decimal> Student =
        price => Math.Round(price * 0.80m, 2);

    public static readonly Func<decimal, decimal> Weekend =
        price => Math.Round(price * 1.20m, 2);

    // Спеціальний некоректний модифікатор для демонстрації валідації додатної ціни квитка
    public static readonly Func<decimal, decimal> InvalidZeroPrice =
        _ => 0m;

    // Predicate<Screening> - перевірка заповненості залу >= 80%
    public static readonly Predicate<Screening> OccupancyAtLeast80Percent =
        screening => screening.OccupancyPercentage >= 80.0;
}
