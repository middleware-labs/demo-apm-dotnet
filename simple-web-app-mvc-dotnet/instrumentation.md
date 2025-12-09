
## OpenTelemetry IIS Express Instrumentation



### PowerShell Installation Steps (Auto-detect .NET Version)

```powershell
$otelBasePath = "C:\otel-dotnet-auto"
New-Item -ItemType Directory -Force -Path $otelBasePath | Out-Null

$moduleUrl = "https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation/releases/latest/download/OpenTelemetry.DotNet.Auto.psm1"
$modulePath = Join-Path $otelBasePath "OpenTelemetry.DotNet.Auto.psm1"
Invoke-WebRequest -Uri $moduleUrl -OutFile $modulePath -UseBasicParsing

Import-Module $modulePath -Force

Install-OpenTelemetryCore

[Environment]::SetEnvironmentVariable("OTEL_SERVICE_NAME", "MyMvcIisService", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:9320", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL", "http/protobuf", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_TRACES_EXPORTER", "otlp", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_METRICS_EXPORTER", "otlp", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_LOGS_EXPORTER", "otlp", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_DOTNET_AUTO_TRACES_ENABLED", "true", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_DOTNET_AUTO_METRICS_ENABLED", "true", "Machine")
[Environment]::SetEnvironmentVariable("OTEL_DOTNET_AUTO_LOGS_ENABLED", "true", "Machine")

$profilerGuid = "{918728DD-259F-4A6A-AC2B-B85E1B658318}"
$profilerPath = "C:\Program Files\OpenTelemetry .NET AutoInstrumentation\win-x64\OpenTelemetry.AutoInstrumentation.Native.dll"

$isNetCore = $false
try {
    $dotnetVersion = & dotnet --version 2>$null
    if ($dotnetVersion) {
        $isNetCore = $true
    }
} catch {}

if ($isNetCore) {
    [Environment]::SetEnvironmentVariable("CORECLR_ENABLE_PROFILING", "1", "Machine")
    [Environment]::SetEnvironmentVariable("CORECLR_PROFILER", $profilerGuid, "Machine")
    [Environment]::SetEnvironmentVariable("CORECLR_PROFILER_PATH", $profilerPath, "Machine")
    [Environment]::SetEnvironmentVariable("CORECLR_PROFILER_PATH_64", $profilerPath, "Machine")
    Write-Host "Set CORECLR_* variables for .NET Core/.NET."
} else {
    [Environment]::SetEnvironmentVariable("COR_ENABLE_PROFILING", "1", "Machine")
    [Environment]::SetEnvironmentVariable("COR_PROFILER", $profilerGuid, "Machine")
    [Environment]::SetEnvironmentVariable("COR_PROFILER_PATH", $profilerPath, "Machine")
    Write-Host "Set COR_* variables for .NET Framework."
}

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