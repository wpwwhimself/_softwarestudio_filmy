@echo off

:: preparing database
cd api
call dotnet ef database update

:: build apps
cd ..
call dotnet build

:: run apps
start "" .\frontend\bin\Debug\net8.0-windows\frontend.exe
call dotnet run --project api
