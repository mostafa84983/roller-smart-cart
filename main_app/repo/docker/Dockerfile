# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5138

# Install ping (iputils-ping)
RUN apt-get update && \
    apt-get install -y iputils-ping && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore using solution
RUN dotnet restore SmartCart.sln

# Publish only the main API project
RUN dotnet publish SmartCart/SmartCart.API.csproj -c Release -o /app/publish

# Final stage
FROM base AS final
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Add wait-for-sql script
COPY SmartCart/wait-for-sql.sh /app/wait-for-sql.sh
RUN chmod +x /app/wait-for-sql.sh

# Entry point runs the wait, then launches app
ENTRYPOINT ["/bin/bash", "/app/wait-for-sql.sh", "dotnet", "SmartCart.API.dll"]
