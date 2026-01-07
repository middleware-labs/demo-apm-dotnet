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

# Set OpenTelemetry environment variables
ENV OTEL_DOTNET_AUTO_HOME="/otel-dotnet-auto"
ENV OTEL_SERVICE_NAME="test-dotnet-ecs-service"
ENV OTEL_TRACES_EXPORTER="otlp"
ENV OTEL_METRICS_EXPORTER="none"
ENV OTEL_LOGS_EXPORTER="none"
ENV OTEL_EXPORTER_OTLP_PROTOCOL="http/protobuf"
ENV OTEL_EXPORTER_OTLP_ENDPOINT="https://123.middleware.io:443"
ENV OTEL_EXPORTER_OTLP_HEADERS="authorization=12345"
ENV OTEL_RESOURCE_ATTRIBUTES="deployment.environment=staging,service.version=1.0.0"

ENTRYPOINT ["/bin/sh", "-c", ". /otel-dotnet-auto/instrument.sh && exec dotnet DemoApi.dll"]