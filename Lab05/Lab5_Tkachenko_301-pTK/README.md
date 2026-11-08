# Лабораторна робота №5
**Тема:** Delegates, Lambda Expressions, Events та Extension Methods  
**Студент:** Ткаченко Микола Віталійович  
**Група:** 301-пТК  
**Варіант:** 4 (Предметна область: Кінотеатр)  
**Версія .NET:** .NET 10 (C# 14)

## Виконані вимоги
- **Func<decimal, decimal>** (`PricingPolicies.cs`): тарифи `Regular`, `Student`, `Weekend` через лямбда-вирази.
- **Predicate<Screening>** (`PricingPolicies.cs`): перевірка заповненості залу `>= 80%`.
- **Action<string> logger** (`Program.cs`, `CinemaService.cs`): логування з поточним часом у 4 доменних операціях.
- **Event `TicketPurchased`**: реалізовано через `EventHandler<Ticket>?` та підписано 2 обробники (`ReceiptService`, `StatisticsService`).
- **Extension Methods** (`ScreeningExtensions.cs`): реалізовано `screening.IsAlmostFull()`, `screening.GetSummary()`, а також додаткове завдання через C# 14 `extension(Screening screening)`.

## Запуск
```bash
dotnet run --project CinemaLab5
```
