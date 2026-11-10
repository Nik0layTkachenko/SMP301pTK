using System.Text.Json;
using CinemaLab7.DTOs;

namespace CinemaLab7.Services;

public class ReportStorageService
{
    private readonly Action<string> _logger;

    public ReportStorageService(Action<string> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public async Task SaveReportsAsync(
        IEnumerable<ScreeningReport> reports,
        string filePath,
        bool writeIndented)
    {
        ArgumentNullException.ThrowIfNull(reports);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = writeIndented
        };

        string json = JsonSerializer.Serialize(reports, options);
        await File.WriteAllTextAsync(filePath, json);

        _logger($"Звіт успішно збережено у JSON-файл: {filePath} (WriteIndented = {writeIndented}).");
    }

    public async Task<List<ScreeningReport>?> LoadReportsAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        try
        {
            string savedJson = await File.ReadAllTextAsync(filePath);
            var restored = JsonSerializer.Deserialize<List<ScreeningReport>>(savedJson);

            if (restored is null)
            {
                Console.WriteLine("Попередження: десеріалізований звіт порожній (null).");
                return null;
            }

            _logger($"Звіт успішно зчитано з файлу: {filePath} (записів: {restored.Count}).");
            return restored;
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"[Помилка файлу - FileNotFoundException]: Файл звіту не знайдено за шляхом "{filePath}".");
            return null;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"[Помилка формату JSON - JsonException]: Файл "{filePath}" містить некоректний JSON. Деталі: {ex.Message}");
            return null;
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"[Помилка доступу - UnauthorizedAccessException]: Немає прав доступу до файлу "{filePath}". {ex.Message}");
            return null;
        }
        catch (IOException ex)
        {
            Console.WriteLine($"[Помилка вводу/виводу - IOException]: Не вдалося прочитати файл "{filePath}". {ex.Message}");
            return null;
        }
    }
}
