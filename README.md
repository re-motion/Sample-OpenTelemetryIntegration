# Sample-OpenTelemetryIntegration

This repo contains a sample for OpenTelemetry integration in ASP.NET Core. The telemetry infrastructure uses OTEL-Collector, Prometheus, and Grafana.

# Running the example

Start the telemetry infrastructure by running `docker compose up` in the `Infrastructure/standard` folder. Use the URL shortcuts to browse the Prometheus/Grafana web interfaces.

To report metrics, start the web application while the telemetry infrastructure is running.