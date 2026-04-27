// @ts-nocheck
import React, { useEffect } from 'react';
import type { ReactNode } from 'react';
import { WebTracerProvider, BatchSpanProcessor } from '@opentelemetry/sdk-trace-web';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { Resource } from '@opentelemetry/resources';
import { SemanticResourceAttributes } from '@opentelemetry/semantic-conventions';
import { registerInstrumentations } from '@opentelemetry/instrumentation';
import { FetchInstrumentation } from '@opentelemetry/instrumentation-fetch';

let initialized = false;

export const TracingProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  useEffect(() => {
    if (initialized) return;
    initialized = true;

    try {
      const provider = new WebTracerProvider({
        resource: new Resource({
          [SemanticResourceAttributes.SERVICE_NAME]: 'nexus-frontend',
        }),
      });

      provider.addSpanProcessor(
        new BatchSpanProcessor(
          new OTLPTraceExporter({
            url: 'http://localhost:4318/v1/traces',
          })
        )
      );

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
