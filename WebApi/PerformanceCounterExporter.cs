using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Metrics;

namespace WebApi
{
  public class PerformanceCounterExporter : BaseExporter<Metric>
  {

    private PerformanceCounter uptimeCounter;

    private readonly string name;

    public PerformanceCounterExporter (string name = "PerformanceCounterExporter")
    {
      this.name = name;

      CreatePerformanceCounter();
    }

    private void CreatePerformanceCounter ()
    {
      if (!PerformanceCounterCategory.Exists("DummyCategory"))
      {
        var counterDataCollection = new CounterCreationDataCollection();
        var uptimeCounter = new CounterCreationData();
        uptimeCounter.CounterType = PerformanceCounterType.CounterDelta64;
        uptimeCounter.CounterName = "UptimeCounterName";
        counterDataCollection.Add(uptimeCounter);

        var performanceCounterCategory = PerformanceCounterCategory.Create("DummyCategory", "Demonstrates things", PerformanceCounterCategoryType.SingleInstance, counterDataCollection);
      }

      uptimeCounter = new PerformanceCounter("DummyCategory", "UptimeCounterName", false);

      uptimeCounter.RawValue = 0;

    }

    public override ExportResult Export (in Batch<Metric> batch)
    {
      using var scope = SuppressInstrumentationScope.Begin();

      foreach (var metric in batch)
      {

        foreach (ref readonly var metricPoint in metric.GetMetricPoints())
        {

        }
      }

      return ExportResult.Success;
    }
  }
}