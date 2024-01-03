curl -o otelcol.tar.gz https://github.com/open-telemetry/opentelemetry-collector-releases/releases/download/v0.91.0/otelcol-contrib_0.91.0_windows_amd64.tar.gz

tar -xzvf otelcol.tar.gz

$dir = Get-Location

sc.exe create OtelCollector displayname=OpenTelemetryCollector start=delayed-auto binPath="$dir\otelcol-contrib.exe --config=file:$dir\otel-collector-config.yaml"
sc.exe start OtelCollector