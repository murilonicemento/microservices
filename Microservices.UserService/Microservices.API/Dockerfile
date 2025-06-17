FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Microservices.API/Microservices.API.csproj", "Microservices.API/"]
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
ENV POSTGRES_HOST=localhost
ENV POSTGRES_PASSWORD=admin
ENV POSTGRES_DATABASE=microservices_users
ENV POSTGRES_USER=postgres
ENV POSTGRES_PORT=5433
ENTRYPOINT ["dotnet", "Microservices.API.dll"]
