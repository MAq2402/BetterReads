FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

ENV ASPNETCORE_URLS http://*:5005
ENV ASPNETCORE_ENVIRONMENT docker
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "BetterReads.Shelves.Web.csproj"
RUN dotnet build "BetterReads.Shelves.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetterReads.Shelves.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetterReads.Shelves.Web.dll"]
