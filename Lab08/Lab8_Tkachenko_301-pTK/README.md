# Лабораторна робота №8
**Тема:** Асинхронне програмування та HTTP  
**Студент:** Ткаченко Микола Віталійович  
**Група:** 301-пТК  
**Варіант:** 4 (Предметна область: Кінотеатр)  
**Версія .NET:** .NET 10 (C# 14)

## Виконані вимоги (Варіант 4)
1. **Модель `ExternalUser` (`DTOs/ExternalUser.cs`)**:
   - `public sealed record ExternalUser(int Id, string Name, string Email);`
2. **Сервіс `VisitorContactsService` (`Services/VisitorContactsService.cs`)**:
   - Реалізовано `GetUserAsync(int id, CancellationToken token)` через `HttpClient.GetFromJsonAsync<ExternalUser>`.
   - Використовується один екземпляр `HttpClient` із базовою адресою з `appsettings.json` (`ExternalApi:BaseUrl`).
   - Завантаження трьох тестових контактів (`users/1`, `users/2`, `users/3`) виконується паралельно за допомогою `Task.WhenAll`.
   - Демонстраційний довідник контактів відвідувачів виводиться для майбутніх повідомлень про сеанси без автоматичного створення `Ticket` чи бронювання `Seat`.
3. **Додаткове завдання та обробка помилок**:
   - Порівняно час виконання 3 послідовних запитів і 3 одночасних запитів через `Task.WhenAll` за допомогою `Stopwatch`.
   - Продемонстровано перехоплення `HttpRequestException`, `OperationCanceledException` та `JsonException`.

## Запуск проєкту
```bash
dotnet run --project CinemaLab8
```
