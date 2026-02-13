# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ElevaniPaymentGateway.Worker.PayAgency/ElevaniPaymentGateway.Worker.PayAgency.csproj", "src/ElevaniPaymentGateway.Worker.PayAgency/"]
COPY ["src/ElevaniPaymentGateway.Infrastructure/ElevaniPaymentGateway.Infrastructure.csproj", "src/ElevaniPaymentGateway.Infrastructure/"]
COPY ["src/ElevaniPaymentGateway.Core/ElevaniPaymentGateway.Core.csproj", "src/ElevaniPaymentGateway.Core/"]
COPY ["src/ElevaniPaymentGateway.Persistence/ElevaniPaymentGateway.Persistence.csproj", "src/ElevaniPaymentGateway.Persistence/"]
RUN dotnet restore "./src/ElevaniPaymentGateway.Worker.PayAgency/ElevaniPaymentGateway.Worker.PayAgency.csproj"
COPY . .
WORKDIR "/src/src/ElevaniPaymentGateway.Worker.PayAgency"
RUN dotnet build "./ElevaniPaymentGateway.Worker.PayAgency.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ElevaniPaymentGateway.Worker.PayAgency.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElevaniPaymentGateway.Worker.PayAgency.dll"]