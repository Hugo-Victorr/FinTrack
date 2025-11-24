FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FinTrack.Model/FinTrack.Model.csproj", "Fintrack.Model/"]
COPY ["FinTrack.Database/FinTrack.Database.csproj", "Fintrack.Database/"]
COPY ["FinTrack.Infrastructure/FinTrack.Infrastructure.csproj", "Fintrack.Infrastructure/"]
COPY ["FinTrack.Education/FinTrack.Education.csproj", "FinTrack.Education/"]

RUN dotnet restore "FinTrack.Education/FinTrack.Education.csproj"
COPY . .
WORKDIR "/src/FinTrack.Education"
RUN dotnet build "./FinTrack.Education.csproj" -c release -o /app/build

FROM build AS publish
RUN dotnet publish "./FinTrack.Education.csproj" -c release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinTrack.Education.dll"]

