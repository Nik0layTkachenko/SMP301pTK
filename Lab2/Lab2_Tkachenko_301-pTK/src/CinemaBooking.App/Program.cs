using System;
using CinemaBooking.Domain;

Console.WriteLine("Modern Programming Course - Lab 2");
Console.WriteLine("Student: Ткаченко Микола Віталійович");
Console.WriteLine("Group: 301-пТК");
Console.WriteLine("Variant: 4");
Console.WriteLine("Domain: Cinema Booking");
Console.WriteLine(".NET: 10\n");

var movie = new Movie("Dune: Part Two", 166);
var hall = new Hall("IMAX", 100);
var screening = new Screening(movie, hall, DateTime.Now.AddHours(2));

decimal basePrice = 200m;

// Демонстрація поліморфізму
ITicketPricingStrategy regularPricing = new RegularPricingStrategy();
ITicketPricingStrategy studentPricing = new StudentPricingStrategy();

Console.WriteLine("--- Бронювання квитків ---");
screening.ReserveSeat(10, regularPricing.CalculatePrice(basePrice));
Console.WriteLine($"Заброньовано місце 10 (Звичайний). Ціна: {regularPricing.CalculatePrice(basePrice)} грн.");

screening.ReserveSeat(11, studentPricing.CalculatePrice(basePrice));
Console.WriteLine($"Заброньовано місце 11 (Студентський). Ціна: {studentPricing.CalculatePrice(basePrice)} грн.");

try
{
    screening.ReserveSeat(10, regularPricing.CalculatePrice(basePrice));
}
catch (Exception ex)
{
    Console.WriteLine($"\nСпроба подвійного бронювання: {ex.Message}");
}

Console.WriteLine("\n--- Скасування бронювання ---");
screening.CancelReservation(11);
Console.WriteLine("Бронювання місця 11 скасовано.");

Console.WriteLine("\n--- Початок сеансу ---");
screening.Start();
Console.WriteLine($"Статус сеансу: {screening.Status}");

try
{
    screening.CancelReservation(10);
}
catch (Exception ex)
{
    Console.WriteLine($"Спроба скасування після початку сеансу: {ex.Message}");
}

screening.Finish();
Console.WriteLine($"\nСтатус сеансу після завершення: {screening.Status}");
