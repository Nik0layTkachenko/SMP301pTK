namespace CinemaLab8.DTOs;

// Зовнішня DTO-модель контакту відвідувача із сервісу JSONPlaceholder
public sealed record ExternalUser(
    int Id,
    string Name,
    string Email);
