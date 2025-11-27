using Azure;
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
using Microsoft.Extensions.Hosting;


namespace MultiQueueDistributor
{
    public class QueueDistributorWorker : BackgroundService
    {
        private readonly ILogger<QueueDistributorWorker> _logger;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly ServiceBusOptions _options;
        private readonly IHostApplicationLifetime _appLifetime;

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
            IOptions<ServiceBusOptions> options,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            _serviceBusClient = serviceBusClient;
            _adminClient = adminClient;
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
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
                MaxConcurrentCalls = 1 
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

                    IncrementAndMaybeStop();
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
                if (IsAuthError(ex))
                {
                    _logger.LogCritical(ex,
                        "Fatal Service Bus authentication/authorization error while processing message {MessageId}. " +
                        "Stopping processor and shutting down host so we don't continue with an invalid token.",
                        message.MessageId);

                    try
                    {
                        if (_processor != null)
                        {
                            await _processor.StopProcessingAsync();
                        }
                    }
                    catch (Exception stopEx)
                    {
                        _logger.LogError(stopEx, "Error while stopping processor after auth failure.");
                    }

                    _appLifetime.StopApplication();

                    throw;
                }

                _logger.LogError(ex,
                    "Error processing message {MessageId}. Abandoning message so it can be retried.",
                    message.MessageId);

                try
                {
                    await args.AbandonMessageAsync(message);
                }
                catch (Exception abandonEx)
                {
                    _logger.LogError(abandonEx,
                        "Error abandoning message {MessageId} after processing failure.",
                        message.MessageId);
                }
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

                if (_processor != null)
                {
                    _ = _processor.StopProcessingAsync();
                }
            }
        }

        private async Task OnErrorAsync(ProcessErrorEventArgs args)
        {
            var ex = args.Exception;

            if (IsAuthError(ex))
            {
                _logger.LogCritical(ex,
                    "Fatal Service Bus authentication/authorization error. Entity: {Entity}, Source: {ErrorSource}. " +
                    "Stopping processor and shutting down host so we don't continue with an invalid token.",
                    args.EntityPath, args.ErrorSource);

                try
                {
                    if (_processor != null)
                    {
                        await _processor.StopProcessingAsync();
                    }
                }
                catch (Exception stopEx)
                {
                    _logger.LogError(stopEx,
                        "Error while stopping processor after auth failure in ProcessErrorAsync.");
                }

                _appLifetime.StopApplication();
                return;
            }

            _logger.LogError(ex,
                "Service Bus processing error. Entity: {Entity}, ErrorSource: {ErrorSource}",
                args.EntityPath, args.ErrorSource);
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
            if (message.ApplicationProperties != null &&
                message.ApplicationProperties.TryGetValue(routingProperty, out var valueFromAppProps) &&
                valueFromAppProps != null)
            {
                var routingKeyFromAppProps = valueFromAppProps.ToString();
                if (!string.IsNullOrWhiteSpace(routingKeyFromAppProps))
                {
                    _logger.LogDebug(
                        "Routing key '{RoutingProperty}' resolved from ApplicationProperties: {RoutingKey}",
                        routingProperty, routingKeyFromAppProps);
                    return routingKeyFromAppProps;
                }
            }

            string bodyText = null;

            try
            {
                bodyText = message.Body.ToString();
                if (string.IsNullOrWhiteSpace(bodyText))
                {
                    _logger.LogWarning(
                        "Message body is empty or null when resolving routing key '{RoutingProperty}'.",
                        routingProperty);
                    return null;
                }

                using var doc = JsonDocument.Parse(bodyText);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty(routingProperty, out var rootProp))
                {
                    var rk = ExtractJsonValue(rootProp);
                    if (!string.IsNullOrWhiteSpace(rk))
                    {
                        _logger.LogDebug(
                            "Routing key '{RoutingProperty}' resolved from root JSON: {RoutingKey}",
                            routingProperty, rk);
                        return rk;
                    }
                }

                if (root.ValueKind == JsonValueKind.Object &&
                    root.TryGetProperty("Params", out var paramsElem) &&
                    paramsElem.ValueKind == JsonValueKind.Object &&
                    paramsElem.TryGetProperty(routingProperty, out var paramsProp))
                {
                    var rk = ExtractJsonValue(paramsProp);
                    if (!string.IsNullOrWhiteSpace(rk))
                    {
                        _logger.LogDebug(
                            "Routing key '{RoutingProperty}' resolved from Params: {RoutingKey}",
                            routingProperty, rk);
                        return rk;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Failed to parse body JSON when resolving routing key '{RoutingProperty}'.",
                    routingProperty);
            }

            try
            {
                if (message.ApplicationProperties != null && message.ApplicationProperties.Count > 0)
                {
                    var keys = string.Join(", ", message.ApplicationProperties.Keys);
                    _logger.LogWarning("Available ApplicationProperties keys: {Keys}", keys);
                }
                else
                {
                    _logger.LogWarning("No ApplicationProperties found on message.");
                }

                if (bodyText == null)
                    bodyText = message.Body.ToString();

                if (!string.IsNullOrWhiteSpace(bodyText))
                {
                    var preview = bodyText.Length > 500 ? bodyText[..500] + "..." : bodyText;
                    _logger.LogWarning("Body preview: {Preview}", preview);
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
        private static bool IsAuthError(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                return true;

            if (ex is RequestFailedException rfe && rfe.Status == 401)
                return true;

            if (ex.InnerException != null)
                return IsAuthError(ex.InnerException);

            return false;
        }
        private static string ExtractJsonValue(JsonElement prop)
        {
            return prop.ValueKind switch
            {
                JsonValueKind.String => prop.GetString(),
                JsonValueKind.Number => prop.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => prop.GetRawText()
            };
        }
    }
}
