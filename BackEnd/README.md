
# Geração de Migrations
cd Data
dotnet ef migrations add InitTables --startup-project ..\BackEnd\ --context Context

# Aplicando Migrations
cd Data
dotnet ef database update --startup-project ..\BackEnd\ --context Context
e.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html -classfilters:"-Data.Migrations.*;-BackEnd.StartupTests"