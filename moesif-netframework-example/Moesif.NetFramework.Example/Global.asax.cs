using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Moesif.NetFramework.Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            // Set environment variables for OpenTelemetry and profiling
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("COR_ENABLE_PROFILING", "1", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("COR_PROFILER", "{918728DD-259F-4A6A-AC2B-B85E1B658318}", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("COR_PROFILER_PATH", "C:\\Program Files\\OpenTelemetry .NET AutoInstrumentation\\win-x64\\OpenTelemetry.AutoInstrumentation.Native.dll", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("COR_PROFILER_PATH_64", "C:\\Program Files\\OpenTelemetry .NET AutoInstrumentation\\win-x64\\OpenTelemetry.AutoInstrumentation.Native.dll", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_SERVICE_NAME", "MyFourMvcIisService", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT", "https://sandbox.middleware.io:443", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_EXPORTER_OTLP_PROTOCOL", "grpc", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_TRACES_EXPORTER", "otlp,console", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_METRICS_EXPORTER", "otlp", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_LOGS_EXPORTER", "otlp", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_TRACES_ENABLED", "true", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_METRICS_ENABLED", "true", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_INSTRUMENTATION_ENABLED", "true", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_LOGS_ENABLED", "true", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_LOG_LEVEL", "debug", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_LOG_LEVEL", "debug", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_TRACES_SAMPLER", "always_on", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("OTEL_DOTNET_AUTO_LOG_DIRECTORY", "C:\\otel-logs", EnvironmentVariableTarget.Process);
            // Log all environment variables
            string[] envNames = new string[] {
                "ASPNETCORE_ENVIRONMENT",
                "COR_ENABLE_PROFILING",
                "COR_PROFILER",
                "COR_PROFILER_PATH",
                "COR_PROFILER_PATH_64",
                "OTEL_SERVICE_NAME",
                "OTEL_EXPORTER_OTLP_ENDPOINT",
                "OTEL_EXPORTER_OTLP_PROTOCOL",
                "OTEL_TRACES_EXPORTER",
                "OTEL_METRICS_EXPORTER",
                "OTEL_LOGS_EXPORTER",
                "OTEL_DOTNET_AUTO_INSTRUMENTATION_ENABLED",
                "OTEL_DOTNET_AUTO_TRACES_ENABLED",
                "OTEL_DOTNET_AUTO_METRICS_ENABLED",
                "OTEL_DOTNET_AUTO_LOGS_ENABLED",
                "OTEL_DOTNET_AUTO_LOG_LEVEL",   
                "OTEL_LOG_LEVEL",
                "OTEL_TRACES_SAMPLER",
                "OTEL_DOTNET_AUTO_LOG_DIRECTORY"
            };
            foreach (var name in envNames)
            {
                string value = Environment.GetEnvironmentVariable(name);
                System.Diagnostics.Debug.WriteLine($"{name}: {value}");
            }
        }
    }
}
