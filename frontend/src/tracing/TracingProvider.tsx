import React, { useEffect } from 'react';
import type { ReactNode } from 'react';
import { WebTracerProvider, BatchSpanProcessor } from '@opentelemetry/sdk-trace-web';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';

let initialized = false;

export const TracingProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  useEffect(() => {
    if (initialized) return;
    initialized = true;

    try {
      // In newer versions of OTel, spanProcessors are passed to the constructor.
      // We omit 'resource' to avoid Vite build issues with ESM exports,
      // relying on environment variables (OTEL_SERVICE_NAME) or defaults.
      const provider = new WebTracerProvider({
        spanProcessors: [
          new BatchSpanProcessor(
            new OTLPTraceExporter({
              url: 'http://localhost:4318/v1/traces',
            })
          ),
        ],
      });

      provider.register();

      registerInstrumentations({
        instrumentations: [
          new FetchInstrumentation({
            propagateTraceHeaderCorsUrls: [/.*/],
          }),
        ],
      });
      
      console.log('OpenTelemetry tracing initialized successfully.');
    } catch (error) {
      console.error('Failed to initialize OpenTelemetry tracing', error);
    }
  }, []);

  return <>{children}</>;
};
