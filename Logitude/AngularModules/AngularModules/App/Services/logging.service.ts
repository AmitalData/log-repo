import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';
import { AppTool } from 'Infrastructure/Tools';

@Injectable() 
export class LogitudeMonitoringService {
  appInsights: ApplicationInsights;
  constructor() {
    if(AppTool.GetLogitudeURL().indexOf('localhost') > -1){
      this.appInsights = new ApplicationInsights({
        config: {
          instrumentationKey: '2ce68a37-58f2-4141-8302-7512e0cd34a5',//2ce68a37-58f2-4141-8302-7512e0cd34a5',
          //connectionString: 'InstrumentationKey=4f44f100-7043-44c7-b913-5480c6f0ad02;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/',
          enableAutoRouteTracking: true // option to log all route changes
        }
      });
    }
    else{
      this.appInsights = new ApplicationInsights({
        config: {
          instrumentationKey: '9e5522d3-edd3-4cf4-ad57-0be639854028',//2ce68a37-58f2-4141-8302-7512e0cd34a5',
          //connectionString: 'InstrumentationKey=4f44f100-7043-44c7-b913-5480c6f0ad02;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/',
          enableAutoRouteTracking: true // option to log all route changes
        }
      });
    }
   
    this.appInsights.loadAppInsights();
  }

  logPageView(name?: string, url?: string) { // option to call manually
    this.appInsights.trackPageView({
      name: name,
      uri: url
    });
  }

  logEvent(name: string, properties?: { [key: string]: any }) {
    this.appInsights.trackEvent({ name: name}, properties);
  }

  logMetric(name: string, average: number, properties?: { [key: string]: any }) {
    this.appInsights.trackMetric({ name: name, average: average }, properties);
  }

  logException(exception: Error, severityLevel?: number) {
    this.appInsights.trackException({ exception: exception, severityLevel: severityLevel });
  }

  logTrace(message: string, properties?: { [key: string]: any }) {
    this.appInsights.trackTrace({ message: message}, properties);
  }
}