using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithMetrics(
        b =>
        {
          b.SetResourceBuilder(
                  ResourceBuilder.CreateDefault()
                      .AddService(TelemetryConstants.ServiceName, serviceVersion: TelemetryConstants.ServiceVersion, serviceInstanceId: Environment.MachineName + Guid.NewGuid()))
              .AddRuntimeInstrumentation()
              .AddProcessInstrumentation()
              .AddAspNetCoreInstrumentation()
              .AddConsoleExporter()
              .AddOtlpExporter(
                      (options, metricOptions) =>
                      {
                          options.Endpoint = new Uri("http://localhost:4317");
                          options.Protocol = OtlpExportProtocol.Grpc;
                          metricOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
                      })
              .AddOtlpExporter(
                      (options, metricOptions) =>
                      {
                          options.Endpoint = new Uri("http://localhost:4318");
                          options.Protocol = OtlpExportProtocol.Grpc;
                          metricOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
                      });
        })
    .WithTracing(
        b =>
        {
          b.SetResourceBuilder(
                  ResourceBuilder.CreateDefault()
                      .AddService(TelemetryConstants.ServiceName, serviceVersion: TelemetryConstants.ServiceVersion))
              .AddAspNetCoreInstrumentation()
              .AddConsoleExporter()
              .AddOtlpExporter();
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet(
        "/weatherforecast",
        () =>
        {
          using var _ = Telemetry.MyActivitySource.StartActivity("Weatherforecast");
          var forecast = Enumerable.Range(1, 5).Select(
                  index =>
                      new WeatherForecast(
                          DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                          Random.Shared.Next(-20, 55),
                          summaries[Random.Shared.Next(summaries.Length)]
                      ))
              .ToArray();
          return forecast;
        })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet(
        "/GC",
        () =>
        {
          GC.Collect();
        })
    .WithName("TriggerGarbageCollection")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
  public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
}