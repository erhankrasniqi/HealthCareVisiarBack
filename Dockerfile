# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["CQRS-Decorator.sln", "./"]

# Copy all project files
COPY ["CQRS-Decorator.API/CQRS-Decorator.API.csproj", "CQRS-Decorator.API/"]
COPY ["CQRS-Decorator.Application/CQRS-Decorator.Application.csproj", "CQRS-Decorator.Application/"]
COPY ["CQRS-Decorator.Domain/CQRS-Decorator.Domain.csproj", "CQRS-Decorator.Domain/"]
COPY ["CQRS-Decorator.Infrastructure/CQRS-Decorator.Infrastructure.csproj", "CQRS-Decorator.Infrastructure/"]
COPY ["CQRS-Decorator.SharedKernel/CQRS-Decorator.SharedKernel.csproj", "CQRS-Decorator.SharedKernel/"]
COPY ["CQRS-Decorator.Decorators/CQRS-Decorator.Decorators.csproj", "CQRS-Decorator.Decorators/"]
COPY ["CQRS-Decorator.Tests/CQRS-Decorator.Tests.csproj", "CQRS-Decorator.Tests/"]

# Restore dependencies
RUN dotnet restore "CQRS-Decorator.API/CQRS-Decorator.API.csproj"

# Copy everything else
COPY . .

# Build the application
WORKDIR "/src/CQRS-Decorator.API"
RUN dotnet build "CQRS-Decorator.API.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "CQRS-Decorator.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create a non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published output
COPY --from=publish /app/publish .

# Set ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "CQRS-Decorator.API.dll"]
