using CinemaLab5.Models;

namespace CinemaLab5.Services;

public static class PricingPolicies
{
    // Func<decimal, decimal> - модифікатори ціни через лямбда-вирази
    public static readonly Func<decimal, decimal> Regular =
        price => price;

    public static readonly Func<decimal, decimal> Student =
        price => Math.Round(price * 0.80m, 2);

    public static readonly Func<decimal, decimal> Weekend =
        price => Math.Round(price * 1.20m, 2);

    // Predicate<Screening> - перевірка чи заповненість залу >= 80%
    public static readonly Predicate<Screening> OccupancyAtLeast80Percent =
        screening => screening.OccupancyPercentage >= 80.0;
}
