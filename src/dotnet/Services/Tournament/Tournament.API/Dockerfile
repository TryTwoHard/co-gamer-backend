FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Services/Tournament.API/Tournament.API.csproj", "Tournament.API/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj", "Contracts/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj", "Common.Logging/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Services/Tournament.API/Tournament.API.csproj"
COPY . .
WORKDIR "/src/Tournament.API"
RUN dotnet build "Tournament.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tournament.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tournament.API.dll"]
