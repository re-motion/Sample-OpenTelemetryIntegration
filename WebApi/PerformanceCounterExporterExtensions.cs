using OpenTelemetry.Metrics;

namespace WebApi
{
  public static class PerformanceCounterExporterExtensions
  {
    public static MeterProviderBuilder AddPerformanceCounterExporter (this MeterProviderBuilder builder, int exportIntervalMilliSeconds = 60000)
    {
      if (builder == null)
      {
        throw new ArgumentNullException(nameof(builder));
      }

      return builder.AddReader(new PeriodicExportingMetricReader(new PerformanceCounterExporter(), exportIntervalMilliSeconds));
    }
  }
}