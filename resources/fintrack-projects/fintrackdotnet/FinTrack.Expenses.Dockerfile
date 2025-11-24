FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["FinTrack.Model/FinTrack.Model.csproj", "Fintrack.Model/"]
COPY ["FinTrack.Expenses/FinTrack.Expenses.csproj", "FinTrack.Expenses/"]

RUN dotnet restore "FinTrack.Expenses/FinTrack.Expenses.csproj"
COPY . .
WORKDIR "/src/FinTrack.Expenses"
RUN dotnet build "./FinTrack.Expenses.csproj" -c release -o /app/build

FROM build AS publish
RUN dotnet publish "./FinTrack.Expenses.csproj" -c release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinTrack.Expenses.dll"]

