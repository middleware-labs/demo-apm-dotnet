
## OpenTelemetry IIS Express Instrumentation

### PowerShell Installation Steps

```powershell
# 1. Create base directory (for module only)
$otelBasePath = "C:\otel-dotnet-auto"
New-Item -ItemType Directory -Force -Path $otelBasePath | Out-Null

# 2. Download the OpenTelemetry module
$moduleUrl = "https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/OpenTelemetry.DotNet.Auto.psm1"
$modulePath = Join-Path $otelBasePath "OpenTelemetry.DotNet.Auto.psm1"
Invoke-WebRequest -Uri $moduleUrl -OutFile $modulePath -UseBasicParsing

# 3. Import the module
Import-Module $modulePath -Force

# 4. Install OpenTelemetry Core (ONLINE - DEFAULT PATH)
Install-OpenTelemetryCore

# 5. Set SYSTEM-WIDE ENV VARIABLES
[Environment]::SetEnvironmentVariable("OTEL_SERVICE_NAME", "MyMvcIisService", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:9320", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL", "http/protobuf", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_TRACES_EXPORTER", "otlp", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_METRICS_EXPORTER", "otlp", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_LOGS_EXPORTER", "otlp", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_DOTNET_AUTO_TRACES_ENABLED", "true", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_DOTNET_AUTO_METRICS_ENABLED", "true", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_DOTNET_AUTO_LOGS_ENABLED", "true", "Machine")

# 6. Register IIS Express profiler
Register-OpenTelemetryForIIS
```

### IIS Express launchSettings.json Example

```json
"IIS Express": {
    "commandName": "IISExpress",
    "launchBrowser": true,
    "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "CORECLR_ENABLE_PROFILING": "1",
        "CORECLR_PROFILER": "{918728DD-259F-4A6A-AC2B-B85E1B658318}",
        "CORECLR_PROFILER_PATH": "C:\\Program Files\\OpenTelemetry .NET AutoInstrumentation\\win-x64\\OpenTelemetry.AutoInstrumentation.Native.dll",
        "CORECLR_PROFILER_PATH_64": "C:\\Program Files\\OpenTelemetry .NET AutoInstrumentation\\win-x64\\OpenTelemetry.AutoInstrumentation.Native.dll",
        "OTEL_SERVICE_NAME": "MyMvcIisService",
        "OTEL_EXPORTER_OTLP_ENDPOINT": "https://sandbox.middleware.io:443",
        "OTEL_EXPORTER_OTLP_PROTOCOL": "grpc",
        "OTEL_TRACES_EXPORTER": "otlp,console",
        "OTEL_METRICS_EXPORTER": "otlp",
        "OTEL_LOGS_EXPORTER": "otlp",
        "OTEL_DOTNET_AUTO_TRACES_ENABLED": "true",
        "OTEL_DOTNET_AUTO_METRICS_ENABLED": "true",
        "OTEL_DOTNET_AUTO_LOGS_ENABLED": "true",
        "OTEL_DOTNET_AUTO_LOG_LEVEL": "debug",
        "OTEL_LOG_LEVEL": "debug",
        "OTEL_TRACES_SAMPLER": "always_on",
        "OTEL_DOTNET_AUTO_LOG_DIRECTORY": "C:\\otel-logs"
    },
    "applicationUrl": "http://localhost:57999/"
}
```

---

**After installation:**
- Close Visual Studio completely
- Reopen it and run using IIS Express