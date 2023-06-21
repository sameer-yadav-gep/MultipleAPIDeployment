# Build API1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-api1
WORKDIR /src/StudentAPI
COPY ["StudentAPI/StudentAPI.csproj", "."]
RUN dotnet restore "StudentAPI.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/StudentAPI

# Build API2
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-api2
WORKDIR /src/WeatherAPI
COPY ["WeatherAPI/WeatherAPI.csproj", "."]
RUN dotnet restore "WeatherAPI.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/WeatherAPI

# Build API3
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-api3
WORKDIR /src/HostAPI
COPY ["HostAPI/HostAPI.csproj", "."]
RUN dotnet restore "HostAPI.csproj"
COPY . .
RUN dotnet publish -c Release -o /app/HostAPI

# Final container image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-api1 /app/StudentAPI .
COPY --from=build-api2 /app/WeatherAPI .
COPY --from=build-api3 /app/HostAPI .
EXPOSE 6001 6002 6003
ENTRYPOINT ["dotnet", "HostAPI.dll"]