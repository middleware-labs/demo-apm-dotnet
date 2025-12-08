using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SimpleWebAppMVC.Controllers
{
    public class DiagnosticsController : Controller
    {
        [HttpGet]
        [Route("/diagnostics/env")]
        public IActionResult Env()
        {
            var keys = new[] {
                "CORECLR_ENABLE_PROFILING",
                "CORECLR_PROFILER",
                "CORECLR_PROFILER_PATH",
                "OTEL_SERVICE_NAME",
                "OTEL_EXPORTER_OTLP_ENDPOINT",
                "OTEL_EXPORTER_OTLP_PROTOCOL",
                "OTEL_TRACES_EXPORTER",
                "OTEL_METRICS_EXPORTER",
                "OTEL_LOGS_EXPORTER",
                "OTEL_DOTNET_AUTO_TRACES_ENABLED",
                "OTEL_DOTNET_AUTO_METRICS_ENABLED",
                "OTEL_DOTNET_AUTO_LOGS_ENABLED",
                "OTEL_DOTNET_AUTO_LOG_LEVEL",
                "OTEL_RESOURCE_ATTRIBUTES",
                "OTEL_LOG_LEVEL",
                "OTEL_TRACES_SAMPLER",
                "OTEL_DOTNET_AUTO_LOG_PATH"
            };
            var env = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                env[key] = Environment.GetEnvironmentVariable(key) ?? "<not set>";
            }
            return Json(env);
        }
    }
}
