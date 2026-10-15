@echo off
chcp 65001 > nul
git checkout -b lab/03-modern-csharp
git add src/CinemaBooking/CinemaBooking.csproj ModernProgramming.slnx src/CinemaBooking/Domain/Seat.cs src/CinemaBooking/Pricing/
git commit -m "Add value objects as records"
git add src/CinemaBooking/Domain/Movie.cs src/CinemaBooking/Domain/Hall.cs src/CinemaBooking/Domain/Ticket.cs
git commit -m "Add nullable domain properties"
git add src/CinemaBooking/Domain/ScreeningStatus.cs src/CinemaBooking/Domain/Screening.cs src/CinemaBooking/Program.cs README.md Answers.md init_git.cmd
git commit -m "Implement status pattern matching"
