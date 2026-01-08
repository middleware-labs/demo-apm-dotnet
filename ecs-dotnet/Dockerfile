FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DemoApi/DemoApi.csproj", "DemoApi/"]
RUN dotnet restore "DemoApi/DemoApi.csproj"
COPY . .
WORKDIR "/src/DemoApi"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "DemoApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DemoApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install dependencies
RUN apt-get update && apt-get install -y curl unzip && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

# Download and install OpenTelemetry auto-instrumentation
RUN OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto" && \
    curl -sSfL https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/download/v1.8.0/otel-dotnet-auto-install.sh -o otel-dotnet-auto-install.sh && \
    OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto" sh otel-dotnet-auto-install.sh && \
    rm otel-dotnet-auto-install.sh

# Create logs directory
RUN mkdir -p /app/logs

ENTRYPOINT ["/bin/sh", "-c", ". /otel-dotnet-auto/instrument.sh && exec dotnet DemoApi.dll"]