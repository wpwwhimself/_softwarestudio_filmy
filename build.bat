@echo off

:: preparing database
cd api
call dotnet ef database update

:: build apps
cd ..
call dotnet build
