@echo off
REM Executar teste dotnet e coletar cobertura de c�digo
dotnet test --collect:"XPlat Code Coverage"

REM Gerar o relat�rio HTML usando reportgenerator
reportgenerator -reports:".\**\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html -classfilters:"-Data.Migrations.*;-BackEnd.StartupTests;-BackEnd.Program;-BackEnd.Startup;-Data.Models.Bases.*;-BackEnd.PubSubBackgroundService"

REM Abrir o relat�rio gerado
start .\coveragereport\index.html

pause