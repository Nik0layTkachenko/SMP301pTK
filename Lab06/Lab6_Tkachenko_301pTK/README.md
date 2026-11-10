# Лабораторна робота №6
**Тема:** Обробка винятків та валідація даних  
**Студент:** Ткаченко Микола Віталійович  
**Група:** 301-пТК  
**Варіант:** 4 (Предметна область: Кінотеатр)  
**Версія .NET:** .NET 10 (C# 14)

## Виконані вимоги (Варіант 4)
1. **Допрацьовано класи `Hall`, `Screening` та `Ticket`**:
   - Додано перевірки вхідних даних: непорожні назви фільму, залу й покупця (`ArgumentException.ThrowIfNullOrWhiteSpace`), додатні номери ряду й місця, додатна базова та фінальна ціна квитка (`ArgumentOutOfRangeException.ThrowIfNegativeOrZero`).
   - Перевірка наявності місця у вибраному залі (`Hall.HasSeat(row, seat)`).
2. **Захист стану об'єкта (`Screening`)**:
   - Заборонено повторне бронювання вже зайнятого місця (`DomainRuleViolationException`).
   - Заборонено продаж квитків або зміну базової ціни після початку сеансу (`InvalidOperationException`).
3. **Цілісність даних та події (`TicketPurchased`)**:
   - Усі перевірки виконуються до зміни стану. За будь-якої помилки кількість вільних місць (`AvailableSeatsCount`) залишається незмінною, а подія `TicketPurchased` не виникає.
4. **Демонстрація в консольному застосунку (`Program.cs`)**:
   - Продемонстровано 1 коректну та 5 некоректних операцій із перехопленням `DomainRuleViolationException`, `ArgumentOutOfRangeException`, `ArgumentException` та `InvalidOperationException` у блоках `try / catch / finally`.
   - Застосовано `int.TryParse` для безпечного розбору числового введення.

## Запуск проєкту
```bash
dotnet run --project CinemaLab6
```
