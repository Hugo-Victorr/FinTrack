FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Expenses/FinTrack.Expenses.csproj FinTrack.Expenses/
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Database/FinTrack.Database.csproj FinTrack.Database/
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Model/FinTrack.Model.csproj FinTrack.Model/

RUN dotnet restore FinTrack.Expenses/FinTrack.Expenses.csproj

COPY ../resources/fintrack-projects/fintrackdotnet/ ./

WORKDIR "/src/FinTrack.Expenses"
RUN dotnet build "FinTrack.Expenses.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinTrack.Expenses.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinTrack.Expenses.dll"]