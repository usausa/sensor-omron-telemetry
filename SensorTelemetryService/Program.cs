using System.Runtime.InteropServices;

using OpenTelemetry.Metrics;
using OpenTelemetryExtension.Instrumentation.SensorOmron;

using SensorTelemetryService;

// Builder
Directory.SetCurrentDirectory(AppContext.BaseDirectory);
var builder = Host.CreateApplicationBuilder(args);

// Setting
var setting = builder.Configuration.GetSection("Service").Get<ServiceSetting>()!;

// Service
builder.Services
    .AddWindowsService()
    .AddSystemd();

// Logging

// Metrics
builder.Services
    .AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddSensorOmronInstrumentation(setting.Port);

        metrics.AddPrometheusHttpListener(options =>
        {
            options.UriPrefixes = setting.EndPoints;
        });
    });

//builder.AddServiceDefaults();
// TODO

// Build
var host = builder.Build();

// Startup
var log = host.Services.GetRequiredService<ILogger<Program>>();
log.InfoServiceStart();
log.InfoServiceSettingsRuntime(RuntimeInformation.OSDescription, RuntimeInformation.FrameworkDescription, RuntimeInformation.RuntimeIdentifier);
log.InfoServiceSettingsEnvironment(typeof(Program).Assembly.GetName().Version, Environment.CurrentDirectory);

host.Run();
