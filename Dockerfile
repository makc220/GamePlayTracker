
# Етап 1: Збірка застосунку
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY GamePlayTracker/GamePlayTracker.csproj GamePlayTracker/
RUN dotnet restore GamePlayTracker/GamePlayTracker.csproj

COPY GamePlayTracker/ GamePlayTracker/
WORKDIR /src/GamePlayTracker

RUN dotnet publish GamePlayTracker.csproj -c Release -o /app/publish /p:UseAppHost=false

# Етап 2: Фінальний образ
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

RUN mkdir -p /app/data

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ConnectionStrings__Default="Data Source=/app/data/gameplaytracker.db"

EXPOSE 8080
VOLUME ["/app/data"]

ENTRYPOINT ["dotnet", "GamePlayTracker.dll"]
