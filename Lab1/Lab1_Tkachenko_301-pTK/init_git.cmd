@echo off
git init -b main
git add .gitignore
git commit -m "Create initial repository"

git switch -c lab/01-git

git add src ModernProgramming.slnx
git commit -m "Add .NET solution and projects"

git add README.md Answers.md
git commit -m "Add project README and answers"

git switch main
git merge lab/01-git --no-ff -m "Merge pull request #1 from lab/01-git"
git tag lab-01

del init_git.cmd
