FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Education/FinTrack.Education.csproj FinTrack.Education/
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Database/FinTrack.Database.csproj FinTrack.Database/
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Model/FinTrack.Model.csproj FinTrack.Model/
COPY ../resources/fintrack-projects/fintrackdotnet/FinTrack.Infrastructure/FinTrack.Infrastructure.csproj FinTrack.Infrastructure/

RUN dotnet restore FinTrack.Education/FinTrack.Education.csproj

COPY ../resources/fintrack-projects/fintrackdotnet/ ./

WORKDIR "/src/FinTrack.Education"
RUN dotnet build "FinTrack.Education.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinTrack.Education.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinTrack.Education.dll"]
