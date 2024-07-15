# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ENV ASPNETCORE_ENVIRONMENT=Development
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["TechChallenge.API/TechChallenge.API.csproj", "TechChallenge.API/"]
COPY ["TechChallenge.Infrastructure/TechChallenge.Data.csproj", "TechChallenge.Infrastructure/"]
COPY ["TechChallenge.Core/TechChallenge.Core.csproj", "TechChallenge.Core/"]
COPY ["TechChallenge.Domain/TechChallenge.Domain.csproj", "TechChallenge.Domain/"]
RUN dotnet restore "./TechChallenge.API/TechChallenge.API.csproj"

# Copy the rest of the application code
COPY . .
WORKDIR "/src/TechChallenge.API"
RUN dotnet build "./TechChallenge.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage / runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge.API.dll"]
