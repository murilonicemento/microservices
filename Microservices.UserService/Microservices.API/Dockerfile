﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservices.API/Microservices.API.csproj", "Microservices.API/"]
COPY ["Microservices.Core/Microservices.Core.csproj", "Microservices.Core/"]
COPY ["Microservices.Infrastructure/Microservices.Infrastructure.csproj", "Microservices.Infrastructure/"]
RUN dotnet restore "Microservices.API/Microservices.API.csproj"
COPY . .
WORKDIR "/src/Microservices.API"
RUN dotnet build "./Microservices.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Microservices.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Microservices.API.dll"]
