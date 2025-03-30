FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Lumen-API/Lumen-API.csproj", "Lumen-API/"]
RUN dotnet restore "./Lumen-API/Lumen-API.csproj"

COPY . .

WORKDIR "/src/Domain"
RUN dotnet build "Domain.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Infra"
RUN dotnet build "Infra.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Application"
RUN dotnet build "Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Lumen-API"
RUN dotnet build "Lumen-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/Lumen-API"
RUN dotnet publish "Lumen-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lumen-API.dll"]
