using System.Diagnostics;

namespace WebApi;

public static class Telemetry
{
  public static readonly ActivitySource MyActivitySource = new(TelemetryConstants.ServiceName);
}