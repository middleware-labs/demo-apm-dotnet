using Middleware.APM;
using MW_WebApp_Nuget.Data;
using MW_WebApp_Nuget.Repository.Interface;
using MW_WebApp_Nuget.Repository;
using System.Text.Json;
using MW_WebApp_Nuget.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.ConfigureMWInstrumentation(configuration);

builder.Logging.ClearProviders(); // Remove all providers
builder.Logging.AddConsole(); // Add console logging
builder.Logging.SetMinimumLevel(LogLevel.Debug); // Set minimum log level

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
