using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace MultiQueueDistributor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var logger = LogManager.Setup()
                                   .LoadConfigurationFromFile("nlog.config")
                                   .GetCurrentClassLogger();

            try
            {
                var hostBuilder = Host.CreateDefaultBuilder(args)
                    .UseWindowsService() 
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var env = hostingContext.HostingEnvironment;

                        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                              .AddJsonFile($"appsettings.{env.EnvironmentName}.json",
                                           optional: true, reloadOnChange: true)
                              .AddEnvironmentVariables();
                    })
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                        logging.AddNLog();
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
                    });

                var host = hostBuilder.Build();

                logger.Info("MultiQueueDistributor starting up.");
                await host.RunAsync();
                logger.Info("MultiQueueDistributor stopped cleanly.");
            }
            catch (Exception ex)
            {
                logger.Error(ex, "MultiQueueDistributor terminated due to an unhandled exception.");
                throw;
            }
            finally
            {
                LogManager.Shutdown(); 
            }
        }
    }
}
