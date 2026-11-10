namespace CinemaLab8.Services;

public static class PricingPolicies
{
    public static readonly Func<decimal, decimal> Regular =
        price => price;

    public static readonly Func<decimal, decimal> Student =
        price => Math.Round(price * 0.80m, 2);

    public static readonly Func<decimal, decimal> Weekend =
        price => Math.Round(price * 1.20m, 2);
}
