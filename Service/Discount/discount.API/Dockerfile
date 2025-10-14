FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY ["Service/Discount/discount.API/discount.API.csproj", "Service/Discount/discount.API/"]
COPY . .
WORKDIR "/src/Service/Discount/discount.API"
RUN dotnet publish "./discount.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "discount.API.dll"]