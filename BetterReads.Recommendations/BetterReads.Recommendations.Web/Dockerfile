FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

ENV ASPNETCORE_URLS http://*:5003
ENV ASPNETCORE_ENVIRONMENT docker
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "BetterReads.Recommendations/BetterReads.Recommendations.Web/BetterReads.Recommendations.Web.csproj"
RUN dotnet build "BetterReads.Recommendations/BetterReads.Recommendations.Web/BetterReads.Recommendations.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetterReads.Recommendations/BetterReads.Recommendations.Web/BetterReads.Recommendations.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BetterReads.Recommendations.Web.dll"]
