@echo off
git add .
set /p mensagem="Mensagem do commit: " 
git commit -m "%mensagem%"
git push
git status
pause