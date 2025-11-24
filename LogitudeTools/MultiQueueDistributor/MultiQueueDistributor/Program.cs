using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace MultiQueueDistributor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .UseWindowsService() // Enables running as Windows Service
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                                       optional: true, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.Configure<ServiceBusOptions>(
                        configuration.GetSection("ServiceBus"));

                    var sbOptions = configuration
                        .GetSection("ServiceBus")
                        .Get<ServiceBusOptions>();

                    if (string.IsNullOrWhiteSpace(sbOptions?.ConnectionString))
                    {
                        throw new InvalidOperationException("ServiceBus:ConnectionString is missing in configuration.");
                    }

                    services.AddSingleton<ServiceBusClient>(_ =>
                        new ServiceBusClient(sbOptions.ConnectionString));

                    services.AddSingleton<ServiceBusAdministrationClient>(_ =>
                        new ServiceBusAdministrationClient(sbOptions.ConnectionString));

                    services.AddHostedService<QueueDistributorWorker>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddEventLog();
                })
                .Build();

            host.Run();
        }
    }

    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string SourceQueue { get; set; } = string.Empty;
        public string QueueNamePrefix { get; set; } = string.Empty;
        public string RoutingProperty { get; set; } = string.Empty;

        public bool Enabled { get; set; } = true;
        public bool DryRun { get; set; } = false;
        public int MaxMessages { get; set; } = 0;
        public bool DeadLetterOnMissingRoutingKey { get; set; } = false;
    }


}
