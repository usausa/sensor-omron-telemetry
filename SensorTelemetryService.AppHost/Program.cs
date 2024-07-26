var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SensorTelemetryService>("sensortelemetryservice");

builder.Build().Run();
