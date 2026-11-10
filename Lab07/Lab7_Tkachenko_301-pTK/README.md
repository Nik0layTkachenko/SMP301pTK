# Лабораторна робота №7
**Тема:** JSON, робота з файлами та конфігурація  
**Студент:** Ткаченко Микола Віталійович  
**Група:** 301-пТК  
**Варіант:** 4 (Предметна область: Кінотеатр)  
**Версія .NET:** .NET 10 (C# 14)

## Виконані вимоги (Варіант 4)
1. **Звітна DTO-модель `ScreeningReport`** (`DTOs/ScreeningReport.cs`):
   - Створено `public sealed record ScreeningReport(int ScreeningId, string MovieTitle, DateTime StartsAt, int TicketsSold, decimal Revenue);` без ослаблення інкапсуляції доменних класів `Hall`, `Screening` та `Ticket`.
2. **Формування звіту та робота з JSON (`System.Text.Json`)**:
   - На основі 5 сеансів та оформлених квитків формується список із 5 записів `ScreeningReport`.
   - Реалізовано асинхронне збереження (`File.WriteAllTextAsync`) та зчитування (`File.ReadAllTextAsync`) у `ReportStorageService`.
   - Після десеріалізації виводиться інформація по кожному сеансу та загальний дохід від квитків.
3. **Конфігурація `appsettings.json` + Додаткове завдання**:
   - Використано пакет `Microsoft.Extensions.Configuration.Json`.
   - Відносний шлях `Storage:FilePath` (`data/screening-report.json`) та параметр форматування `Storage:WriteIndented` (`true`/`false`) зчитуються з `appsettings.json`.
4. **Обробка помилок файлів та JSON**:
   - Продемонстровано безпечне перехоплення `FileNotFoundException` (відсутній файл) та `JsonException` (некоректний синтаксис JSON) без аварійного завершення програми.

## Запуск проєкту
```bash
dotnet run --project CinemaLab7
```
