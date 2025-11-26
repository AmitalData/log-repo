using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MultiQueueDistributor
{
    public class QueueDistributorWorker : BackgroundService
    {
        private readonly ILogger<QueueDistributorWorker> _logger;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly ServiceBusOptions _options;

        private ServiceBusProcessor _processor;
        private readonly ConcurrentDictionary<string, ServiceBusSender> _senderCache =
            new ConcurrentDictionary<string, ServiceBusSender>(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentDictionary<string, bool> _knownQueues =
            new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        private int _processedCount = 0;

        public QueueDistributorWorker(
            ILogger<QueueDistributorWorker> logger,
            ServiceBusClient serviceBusClient,
            ServiceBusAdministrationClient adminClient,
            IOptions<ServiceBusOptions> options)
        {
            _logger = logger;
            _serviceBusClient = serviceBusClient;
            _adminClient = adminClient;
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QueueDistributorWorker is starting.");
            _logger.LogInformation(
                "Config: Enabled={Enabled}, DryRun={DryRun}, MaxMessages={MaxMessages}, DeadLetterOnMissingRoutingKey={DeadLetterOnMissingRoutingKey}",
                _options.Enabled, _options.DryRun, _options.MaxMessages, _options.DeadLetterOnMissingRoutingKey);

            if (!_options.Enabled)
            {
                _logger.LogWarning("QueueDistributorWorker is DISABLED via configuration. No messages will be processed.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_options.SourceQueue))
            {
                throw new InvalidOperationException("ServiceBus:SourceQueue is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_options.QueueNamePrefix))
            {
                throw new InvalidOperationException("ServiceBus:QueueNamePrefix is not configured.");
            }

            if (string.IsNullOrWhiteSpace(_options.RoutingProperty))
            {
                throw new InvalidOperationException("ServiceBus:RoutingProperty is not configured.");
            }

            _processor = _serviceBusClient.CreateProcessor(_options.SourceQueue, new ServiceBusProcessorOptions
            {
                AutoCompleteMessages = false,
                MaxConcurrentCalls = 1 // for safety: only one at a time
            });

            _processor.ProcessMessageAsync += OnMessageReceivedAsync;
            _processor.ProcessErrorAsync += OnErrorAsync;

            await _processor.StartProcessingAsync(cancellationToken);

            _logger.LogInformation(
                "QueueDistributorWorker started. Listening on source queue '{SourceQueue}'.",
                _options.SourceQueue);

            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QueueDistributorWorker is stopping.");

            if (_processor != null)
            {
                await _processor.StopProcessingAsync(cancellationToken);
                await _processor.DisposeAsync();
            }

            foreach (var sender in _senderCache.Values)
            {
                await sender.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);

            _logger.LogInformation("QueueDistributorWorker stopped.");
        }

        private async Task OnMessageReceivedAsync(ProcessMessageEventArgs args)
        {
            var message = args.Message;

            // Safety: respect MaxMessages at the top
            if (_options.MaxMessages > 0 && _processedCount >= _options.MaxMessages)
            {
                _logger.LogWarning(
                    "MaxMessages ({MaxMessages}) already reached. Abandoning message {MessageId} and stopping processor.",
                    _options.MaxMessages, message.MessageId);

                await args.AbandonMessageAsync(message);
                await _processor.StopProcessingAsync();
                return;
            }

            try
            {
                string routingKey = ResolveRoutingKey(message, _options.RoutingProperty);

                if (string.IsNullOrWhiteSpace(routingKey))
                {
                    _logger.LogError(
                        "Message {MessageId} missing routing key '{RoutingProperty}'.",
                        message.MessageId, _options.RoutingProperty);

                    if (_options.DeadLetterOnMissingRoutingKey && !_options.DryRun)
                    {
                        _logger.LogWarning(
                            "DeadLetterOnMissingRoutingKey = true. Sending message {MessageId} to dead-letter queue.",
                            message.MessageId);

                        await args.DeadLetterMessageAsync(
                            message,
                            "MissingRoutingKey",
                            $"The routing property '{_options.RoutingProperty}' was not found on message."
                        );
                    }
                    else
                    {
                        _logger.LogWarning(
                            "DeadLetterOnMissingRoutingKey = false. Abandoning message {MessageId} so it can be retried.",
                            message.MessageId);

                        await args.AbandonMessageAsync(message);
                    }

                    return;
                }

                var destinationQueue = $"{_options.QueueNamePrefix}{routingKey}";

                if (_options.DryRun)
                {
                    _logger.LogInformation(
                        "[DRY-RUN] Would route message {MessageId} to queue {DestinationQueue}. " +
                        "Not creating queues, not forwarding, not completing.",
                        message.MessageId, destinationQueue);

                    await args.AbandonMessageAsync(message);
                    IncrementAndMaybeStop();
                    return;
                }

                _logger.LogInformation(
                    "Routing message {MessageId} to queue {DestinationQueue}.",
                    message.MessageId, destinationQueue);

                await EnsureQueueExistsAsync(destinationQueue);

                var sender = _senderCache.GetOrAdd(
                    destinationQueue,
                    q => _serviceBusClient.CreateSender(q));

                var forwardMessage = CloneMessage(message);

                await sender.SendMessageAsync(forwardMessage);

                await args.CompleteMessageAsync(message);

                _logger.LogInformation(
                    "Message {MessageId} successfully forwarded to {DestinationQueue} and completed.",
                    message.MessageId, destinationQueue);

                IncrementAndMaybeStop();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error processing message {MessageId}. Abandoning message so it can be retried.",
                    message.MessageId);

                await args.AbandonMessageAsync(message);
            }
        }

        private void IncrementAndMaybeStop()
        {
            if (_options.MaxMessages <= 0)
                return;

            int newCount = Interlocked.Increment(ref _processedCount);

            if (newCount >= _options.MaxMessages)
            {
                _logger.LogWarning(
                    "MaxMessages limit reached ({MaxMessages}). Stopping processor.",
                    _options.MaxMessages);

                _ = _processor.StopProcessingAsync();
            }
        }

        private Task OnErrorAsync(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception,
                "Service Bus processing error. Entity: {EntityPath}, ErrorSource: {ErrorSource}",
                args.EntityPath, args.ErrorSource);

            return Task.CompletedTask;
        }

        private async Task EnsureQueueExistsAsync(string queueName)
        {
            if (_knownQueues.ContainsKey(queueName))
            {
                return;
            }

            try
            {
                var existsResponse = await _adminClient.QueueExistsAsync(queueName);
                if (!existsResponse.Value)
                {
                    _logger.LogInformation(
                        "Queue {QueueName} does not exist. Creating...",
                        queueName);

                    await _adminClient.CreateQueueAsync(queueName);

                    _logger.LogInformation(
                        "Queue {QueueName} created.",
                        queueName);
                }
                else
                {
                    _logger.LogDebug(
                        "Queue {QueueName} already exists.",
                        queueName);
                }

                _knownQueues[queueName] = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error ensuring queue {QueueName} exists.",
                    queueName);
                throw;
            }
        }

        private static ServiceBusMessage CloneMessage(ServiceBusReceivedMessage message)
        {
            var clone = new ServiceBusMessage(message)
            {
                MessageId = message.MessageId,
                CorrelationId = message.CorrelationId,
                Subject = message.Subject,
                ContentType = message.ContentType,
                SessionId = message.SessionId,
                ReplyToSessionId = message.ReplyToSessionId,
                ReplyTo = message.ReplyTo
            };

            return clone;
        }

        private string ResolveRoutingKey(ServiceBusReceivedMessage message, string routingProperty)
        {
            // 1. ApplicationProperties
            if (message.ApplicationProperties != null &&
                message.ApplicationProperties.TryGetValue(routingProperty, out var valueFromAppProps) &&
                valueFromAppProps != null)
            {
                var routingKey = valueFromAppProps.ToString();
                if (!string.IsNullOrWhiteSpace(routingKey))
                {
                    _logger.LogDebug(
                        "Routing key '{RoutingProperty}' resolved from ApplicationProperties: {RoutingKey}",
                        routingProperty, routingKey);

                    return routingKey;
                }
            }

            string bodyText = null;

            // 2. JSON body
            try
            {
                bodyText = message.Body.ToString();
                if (!string.IsNullOrWhiteSpace(bodyText))
                {
                    using var doc = JsonDocument.Parse(bodyText);
                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty(routingProperty, out var prop))
                    {
                        string routingKey = prop.ValueKind switch
                        {
                            JsonValueKind.String => prop.GetString(),
                            JsonValueKind.Number => prop.GetRawText(),
                            JsonValueKind.True => "true",
                            JsonValueKind.False => "false",
                            _ => prop.GetRawText()
                        };

                        if (!string.IsNullOrWhiteSpace(routingKey))
                        {
                            _logger.LogDebug(
                                "Routing key '{RoutingProperty}' resolved from JSON body: {RoutingKey}",
                                routingProperty, routingKey);

                            return routingKey;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Failed to parse body as JSON when resolving routing key '{RoutingProperty}'.",
                    routingProperty);
            }

            // === EXTRA LOGGING WHEN WE FAIL ===

            try
            {
                // Log available application property keys
                if (message.ApplicationProperties != null && message.ApplicationProperties.Count > 0)
                {
                    var keys = string.Join(", ", message.ApplicationProperties.Keys);
                    _logger.LogWarning(
                        "Available ApplicationProperties keys: {Keys}",
                        keys);
                }
                else
                {
                    _logger.LogWarning("No ApplicationProperties found on message.");
                }

                // Log a preview of the body
                if (bodyText == null)
                {
                    bodyText = message.Body.ToString();
                }

                if (!string.IsNullOrWhiteSpace(bodyText))
                {
                    var preview = bodyText.Length > 500 ? bodyText.Substring(0, 500) + "..." : bodyText;
                    _logger.LogWarning("Body preview: {Preview}", preview);
                }
                else
                {
                    _logger.LogWarning("Message body is empty or null.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed while logging message metadata for routing debug.");
            }

            _logger.LogWarning(
                "Could not resolve routing key for property '{RoutingProperty}'.",
                routingProperty);

            return null;
        }
    }
}
