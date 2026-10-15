@echo off
chcp 65001 > nul
git checkout -b lab/04-linq
git add src/CinemaBooking/CinemaBooking.csproj ModernProgramming.slnx src/CinemaBooking/Domain/ src/CinemaBooking/Pricing/ src/CinemaBooking/Repositories/InMemoryRepository.cs
git commit -m "Add generic in-memory repository"
git add src/CinemaBooking/Services/CinemaDataSeeder.cs
git commit -m "Add domain test data"
git add src/CinemaBooking/Services/CinemaAnalyticsService.cs src/CinemaBooking/Program.cs README.md Answers.md init_git.cmd
git commit -m "Implement LINQ statistics"
