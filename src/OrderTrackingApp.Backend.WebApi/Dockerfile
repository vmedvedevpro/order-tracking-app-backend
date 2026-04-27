FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OrderTrackingApp.Backend.WebApi/OrderTrackingApp.Backend.WebApi.csproj", "src/OrderTrackingApp.Backend.WebApi/"]
COPY ["src/OrderTrackingApp.Backend.Application/OrderTrackingApp.Backend.Application.csproj", "src/OrderTrackingApp.Backend.Application/"]
COPY ["src/OrderTrackingApp.Backend.Domain/OrderTrackingApp.Backend.Domain.csproj", "src/OrderTrackingApp.Backend.Domain/"]
COPY ["src/OrderTrackingApp.Backend.Infrastructure/OrderTrackingApp.Backend.Infrastructure.csproj", "src/OrderTrackingApp.Backend.Infrastructure/"]
RUN dotnet restore "src/OrderTrackingApp.Backend.WebApi/OrderTrackingApp.Backend.WebApi.csproj"
COPY . .
WORKDIR "/src/src/OrderTrackingApp.Backend.WebApi"
RUN dotnet build "./OrderTrackingApp.Backend.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrderTrackingApp.Backend.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderTrackingApp.Backend.WebApi.dll"]
