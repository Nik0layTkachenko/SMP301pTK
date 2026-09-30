@echo off
git init -b main
git add .gitignore
git commit -m "Create initial repository"

git switch -c lab/02-oop

git add ModernProgramming.slnx src/CinemaBooking.Domain/Movie.cs src/CinemaBooking.Domain/Hall.cs src/CinemaBooking.Domain/Ticket.cs src/CinemaBooking.Domain/ScreeningStatus.cs src/CinemaBooking.Domain/CinemaBooking.Domain.csproj
git commit -m "Add initial domain entities"

git add src/CinemaBooking.Domain/Screening.cs src/CinemaBooking.App/Program.cs src/CinemaBooking.App/CinemaBooking.App.csproj
git commit -m "Implement domain behavior"

git add src/CinemaBooking.Domain/ITicketPricingStrategy.cs src/CinemaBooking.Domain/RegularPricingStrategy.cs src/CinemaBooking.Domain/StudentPricingStrategy.cs src/CinemaBooking.App/Program.cs README.md Answers.md
git commit -m "Add polymorphic strategy"

git switch main
git merge lab/02-oop --no-ff -m "Merge pull request #2 from lab/02-oop"
git tag lab-02

del init_git.cmd
