# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Service/Order/order.API/order.API.csproj", "Service/Order/order.API/"]
COPY ["BuildingBlocks/buildingBlock/buildingBlock.csproj", "BuildingBlocks/buildingBlock/"]
COPY ["Service/Order/order.Application/order.Application.csproj", "Service/Order/order.Application/"]
COPY ["BuildingBlocks/buildingBlock.Messaging/buildingBlock.Messaging.csproj", "BuildingBlocks/buildingBlock.Messaging/"]
COPY ["Service/Order/order.Domain/order.Domain.csproj", "Service/Order/order.Domain/"]
COPY ["Service/Order/order.Infrastructure/order.Infrastructure.csproj", "Service/Order/order.Infrastructure/"]
RUN dotnet restore "./Service/Order/order.API/order.API.csproj"
COPY . .
WORKDIR "/src/Service/Order/order.API"
RUN dotnet build "./order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "order.API.dll"]