using Middleware.APM;
using MW_WebApp_Nuget.Data;
using MW_WebApp_Nuget.Repository.Interface;
using MW_WebApp_Nuget.Repository;
using System.Text.Json;
using MW_WebApp_Nuget.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Configuration.AddEnvironmentVariables();

var mwApiKey = Environment.GetEnvironmentVariable("MW_API_KEY") ?? builder.Configuration.GetSection("MW")["ApiKey"];
var runtimeMetricsStr = Environment.GetEnvironmentVariable("RUN_METRICS_DOTNET") ?? builder.Configuration.GetSection("MW")["RunTime_DotNet_Metrics"];
var projectName = Environment.GetEnvironmentVariable("PROJECT_NAME")
?? builder.Configuration.GetSection("MW")["Project_Name"];
var serviceName = Environment.GetEnvironmentVariable("SERVICE_NAME") ?? builder.Configuration.GetSection("MW")["Service_Name"];
var target = Environment.GetEnvironmentVariable("MW_TARGET") ?? builder.Configuration.GetSection("MW")["Target_URL"];
var consoleExporterStr = Environment.GetEnvironmentVariable("MW_CONSOLE_EXPORTER") ?? builder.Configuration.GetSection("MW")["Console_Exporter"];
var excludeLinksEnv = Environment.GetEnvironmentVariable("MW_EXCLUDE_LINKS");

string[]? excludeLinks = !string.IsNullOrEmpty(excludeLinksEnv)
? JsonSerializer.Deserialize<string[]>(excludeLinksEnv) : builder.Configuration.GetSection("MW:Exclude_Links").Get<string[]>();


var attributes = new Dictionary<string, object>
{
    { "mw.account_key", mwApiKey ?? string.Empty },
    { "runtime.metrics.dotnet", Convert.ToBoolean(runtimeMetricsStr)},
    { "project.name", projectName ?? string.Empty },
    { "service.name", serviceName ?? string.Empty },
    { "target", target ?? string.Empty },
    { "console.exporter", Convert.ToBoolean(consoleExporterStr)},
    { "exclude.links", excludeLinks ?? Array.Empty<string>() }
};

builder.Services.ConfigureMWInstrumentation(attributes);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<DapperContext>();
builder.Services.AddTransient<IPersonsRepository, PersonsRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.WebHost.ConfigureKestrel(options =>
{
    var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    // Configuration for both development and production
    if (!isDocker)
    {
        options.ListenAnyIP(7126, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            listenOptions.UseHttps();
        });
    }
    
    options.ListenAnyIP(5192, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });

    // gRPC endpoint
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
var app = builder.Build();

Logger.Init(app.Services.GetRequiredService<ILoggerFactory>());

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<WeatherServiceImplGrpc>();

app.Run();
