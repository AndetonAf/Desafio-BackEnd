@echo off
REM Executar teste dotnet e coletar cobertura de código
dotnet test --collect:"XPlat Code Coverage"

REM Gerar o relatório HTML usando reportgenerator
reportgenerator -reports:".\**\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html -classfilters:"-Data.Migrations.*;-BackEnd.StartupTests;-BackEnd.Program;-BackEnd.Startup;-Data.Models.Bases.*;-BackEnd.PubSubBackgroundService"

REM Abrir o relatório gerado
start .\coveragereport\index.html

pause