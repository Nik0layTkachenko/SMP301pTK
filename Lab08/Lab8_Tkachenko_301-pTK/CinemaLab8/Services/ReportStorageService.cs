using System.Text.Json;
using CinemaLab8.DTOs;

namespace CinemaLab8.Services;

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

        _logger($"Звіт збережено у файл: {filePath}");
    }
}
