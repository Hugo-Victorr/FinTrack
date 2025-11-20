FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ../resources/fintrack-projects/openfinancedotnet/OpenFinance.API/OpenFinance.API.csproj OpenFinance.API/
COPY ../resources/fintrack-projects/openfinancedotnet/OpenFinance.Model/OpenFinance.Model.csproj OpenFinance.Model/

RUN dotnet restore OpenFinance.API/OpenFinance.API.csproj

COPY ../resources/fintrack-projects/openfinancedotnet/ ./

WORKDIR "/src/OpenFinance.API"
RUN dotnet build "OpenFinance.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenFinance.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenFinance.API.dll"]